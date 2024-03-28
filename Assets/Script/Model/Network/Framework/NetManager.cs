using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;

public static class NetManager
{
    static Socket socket;
    static ByteArray readBuff;
    static Queue<ByteArray> writeQueue;
    static List<MsgBase> msgList;

    //是否正在连接
    static bool isConnecting = false;
    //是否正在关闭
    static bool isClosing = false;
    //消息列表中的消息数量
    static int msgCount = 0;
    //每帧最大消息处理数量
    readonly static int MAX_MESSAGE_DISPATCH = 10;
    //是否启用心跳
    public static bool isUsePing = true;
    //心跳间隔时间
    public static int pingInterval = 30;
    //上一次发送PING的时间
    static float lastPingTime = 0;
    //上一次收到PONG的时间
    static float lastPongTime = 0;

    //初始化状态
    private static void InitState()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //接收缓冲区
        readBuff = new ByteArray();
        //写入队列
        writeQueue = new Queue<ByteArray>();
        //消息列表
        msgList = new List<MsgBase>();

        isConnecting = false;
        isClosing = false;
        msgCount = 0;
        lastPingTime = Time.time;
        lastPongTime = Time.time;

        //监听PONG协议
        if(!NetMsg.Instance.dic.ContainsKey("MsgPong"))
        {
            NetMsg.Instance.AddEventListener("MsgPong", OnMsgPong);
        }
    }

    //监听PONG协议
    private static void OnMsgPong(MsgBase msgBase)
    {
        lastPongTime = Time.time;
        Debug.Log("Receive MsgPong");
    }

    #region 异步对外接口及相应回调函数
    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public static void Connect(string ip, int port)
    {
        //状态判断
        if (socket != null && socket.Connected)
        {
            Debug.Log("Connect fail, already Connected");
            return;
        }
        if (isConnecting)
        {
            Debug.Log("Connect fail, isConnecting");
            return;
        }
        //初始化成员
        InitState();
        //参数设置
        socket.NoDelay = true;
        //连接
        isConnecting = true;
        socket.BeginConnect(ip, port, ConncetCallback, socket);
    }

    private static void ConncetCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ");
            NetEvent.Instance.Dispatch(NetEventType.ConnectSucc, "Conncet Success!");
            isConnecting = false;

            //开始接收
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail " + ex.ToString());
            NetEvent.Instance.Dispatch(NetEventType.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            //获取接收数据长度
            int count = socket.EndReceive(ar);
            if (count == 0)
            {
                Close();
                return;
            }
            readBuff.writeIdx += count;
            //处理二进制信息
            OnReceiveData();
            //继续接收数据
            if (readBuff.remainEnough)
            {
                readBuff.MoveBytes();
                readBuff.ReSize(readBuff.length * 2);
            }
            socket.BeginReceive(readBuff.bytes, readBuff.writeIdx, readBuff.remain, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail " + ex.ToString());
        }
    }

    //数据处理
    private static void OnReceiveData()
    {
        if (readBuff.length <= 2)
        {
            return;
        }
        //获取消息体长度
        int readIdx = readBuff.readIdx;
        byte[] bytes = readBuff.bytes;
        Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | (bytes[readIdx]));
        if (readBuff.length < bodyLength)
        {
            return;
        }
        readBuff.readIdx += 2;
        //解析协议名
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out int nameCount);
        if (protoName == "")
        {
            Debug.Log("OnReceiveData MsgBase.DecodeName fail");
            return;
        }
        readBuff.readIdx += nameCount;
        //解析协议体
        int bodyCount = bodyLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();
        //添加到消息队列
        lock (msgList)
        {
            msgList.Add(msgBase);
        }
        msgCount++;
        //继续读取消息
        if (readBuff.length > 2)
        {
            OnReceiveData();
        }
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Close()
    {
        if (socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting)
        {
            return;
        }
        //判断是否有未发送数据
        if (writeQueue.Count > 0)
        {
            isClosing = true;
        }
        else
        {
            socket.Close();
            NetEvent.Instance.Dispatch(NetEventType.Close, "");
        }
    }

    public static void Send(MsgBase msg)
    {
        if (socket == null || !socket.Connected)
        { return; }
        if (isConnecting)
        { return; }
        if (isClosing)
        { return; }
        //数据编码
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];
        //组装长度(小端编码)
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);
        //组装名字
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        //组装消息体
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
        //写入队列
        ByteArray ba = new ByteArray(sendBytes);
        int WQCount = 0;
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            WQCount = writeQueue.Count;
        }
        //发送(首次启动，之后的在回调函数中再调用发送)
        if (WQCount == 1)
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        Socket socket = (Socket)ar.AsyncState;
        if (socket == null || !socket.Connected)
        { return; }
        int count = socket.EndSend(ar);
        //获取写入队列的第一条数据
        ByteArray ba;
        lock (writeQueue)
        {
            ba = writeQueue.First();
        }
        //完整发送
        ba.readIdx += count;
        //获取写入队列的下一条数据
        if (ba.length == 0)
        {
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        //还有数据则继续发送，发送完且正在关闭则关闭连接
        if (ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
        }
        else if (isClosing)
        {
            socket.Close();
        }
    }
    #endregion

    #region 内部Update封装
    private static void MsgUpdate()
    {
        if (msgCount == 0) { return; }
        //重复处理消息
        for (int i = 0; i < MAX_MESSAGE_DISPATCH; i++)
        {
            //获取第一条消息
            MsgBase msgBase = null;
            lock (msgList)
            {
                if (msgList.Count > 0)
                {
                    msgBase = msgList[0];
                    msgList.RemoveAt(0);
                    msgCount--;
                }
            }
            //分发消息
            if (msgBase != null)
            {
                NetMsg.Instance.Dispatch(msgBase.protoName, msgBase);
            }
            else
            {
                break;
            }
        }
    }

    private static void PingUpdate()
    {
        //是否启用
        if (!isUsePing) { return; }
        if (socket == null || socket.Connected == false) { return; }
        //发送PING
        if (Time.time - lastPingTime > pingInterval)
        {
            MsgPing msgPing = new();
            Send(msgPing);
            Debug.Log("Send MsgPing");
            lastPingTime = Time.time;
            Debug.Log($"PongTimeInterval:{Time.time - lastPongTime},TimeNow:{Time.time},LastPongTime:{lastPongTime}");
        }
        //检测PONG时间
        if (Time.time - lastPongTime > pingInterval * 4)
        {
            Close();
        }
    } 
    #endregion

    //Update
    public static void Update()
    {
        MsgUpdate();
        PingUpdate();
    }
}
public enum NetEventType
{
    ConnectSucc = 1,
    ConnectFail = 2,
    Close = 3
}
public class NetEvent : EventBase<NetEvent, string, NetEventType>
{

}

public class NetMsg : EventBase<NetMsg, MsgBase, string>
{

}