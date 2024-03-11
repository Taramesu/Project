
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Channels;

public class NetManager
{
    public static Socket listenfd;
    public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
    
    static List<Socket> checkRead = new List<Socket>();

    public static void StartLoop(int listenPort)
    {
        //Socket
        listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Bind
        IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);
        listenfd.Bind(ipEp);
        //Listen
        listenfd.Listen(0);
        Console.WriteLine("[服务器]启动成功");
        //循环
        while (true)
        {
            //重置checkRead
            ResetCheckRead();
            Socket.Select(checkRead, null, null, 1000);
            //检查可读对象
            for(int i = checkRead.Count - 1; i >= 0; i--) 
            {
                Socket s = checkRead[i];
                if(s == listenfd)
                {
                    ReadListenfd(s);
                }
                else
                {
                    ReadClientfd(s);
                }
            }
            //超时
            Timer();
        }
    }

    //处理客户端消息
    private static void ReadClientfd(Socket clientfd)
    {
        ClientState state = clients[clientfd];
        ByteArray readBuff = state.readBuff;
        //接收
        int count = 0;
        //缓冲区不够，清除，仍不够，提示返回
        if(readBuff.remain <= 0)
        {
            OnReceiveData(state);
            readBuff.MoveBytes();
        }
        if(readBuff.remain <= 0) 
        {
            Console.WriteLine("Receive fail, maybe msg length > buff capacity");
            Close(state);
            return;
        }

        try
        {
            count = clientfd.Receive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0);
        }
        catch(SocketException e) 
        {
            Console.WriteLine("Receive SocketException" + e.ToString());
            Close(state);
            return;
        }

        //客户端关闭
        if(count <= 0)
        {
            Console.WriteLine("Socket Close"+clientfd.RemoteEndPoint.ToString());
            Close(state);
            return;
        }
        //消息处理
        readBuff.writeIdx += count;
        //处理二进制消息
        OnReceiveData(state);
        //移动缓冲区
        readBuff.CheckAndMoveBytes();
    }

    public static void Close(ClientState state)
    {
        //事件分发
        MethodInfo mi = typeof(EventHandler).GetMethod("OnDisconnect");
        object[] ob = { state };
        mi.Invoke(null, ob);
        //关闭
        state.socket.Close();
        clients.Remove(state.socket);
    }

    public static void Send(ClientState cs, MsgBase msg)
    {
        if(cs == null)
        {
            return;
        }
        if(!cs.socket.Connected)
        {
            return;
        }
        //数据编码
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];
        //组装长度
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);
        //组装名字
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        //组装消息体
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
        try
        {
            cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
        }
        catch(SocketException e) 
        {
            Console.WriteLine("Socket Close On BeginSend"+e.ToString());
        }
    }

    //数据处理
    private static void OnReceiveData(ClientState state)
    {
        ByteArray readBuff = state.readBuff;
        //消息长度
        if(readBuff.length <= 2)
        {
            return;
        }
        Int16 bodyLength = readBuff.ReadInt16();
        //消息体
        if(readBuff.length < bodyLength)
        {
            return;
        }
        //解析协议名
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out int nameCount);
        if (protoName == "") 
        {
            Console.WriteLine("OnReceiveData MsgBase.DecodeName fail");
            Close(state);
        }
        readBuff.readIdx += nameCount;
        //解析协议体
        int bodyCount = bodyLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();
        //分发消息
        MethodInfo mi = typeof(MsgHandler).GetMethod(protoName);
        object[] ob = { state, msgBase};
        Console.WriteLine("Receive" + protoName);
        if(mi != null)
        {
            mi.Invoke(null, ob);
        }
        else
        {
            Console.WriteLine("OnReceiveData Invoke fail"+protoName);
        }
        //继续读取消息
        if(readBuff.length > 2)
        {
            OnReceiveData(state);
        }
    }

    //处理监听消息
    private static void ReadListenfd(Socket listenfd)
    {
        try
        {
            Socket clientfd = listenfd.Accept();
            Console.WriteLine("Accept" + clientfd.RemoteEndPoint.ToString());
            ClientState state = new ClientState();
            state.socket = clientfd;
            clients.Add(clientfd,state);
        }
        catch (SocketException e) 
        {
            Console.WriteLine("Accept fail" + e.ToString());
        }
    }

    //填充checkRead列表
    private static void ResetCheckRead()
    {
        checkRead.Clear();
        checkRead.Add(listenfd);
        foreach (ClientState s in clients.Values)
        {
            checkRead.Add(s.socket);
        }
    }

    //定时器
    static void Timer()
    {
        //消息分发
        MethodInfo mi = typeof(EventHandler).GetMethod("OnTimer");
        object[] ob = { };
        mi.Invoke(null, ob);
    }
}
