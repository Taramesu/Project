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

    //�Ƿ���������
    static bool isConnecting = false;
    //�Ƿ����ڹر�
    static bool isClosing = false;
    //��Ϣ�б��е���Ϣ����
    static int msgCount = 0;
    //ÿ֡�����Ϣ��������
    readonly static int MAX_MESSAGE_DISPATCH = 10;
    //�Ƿ���������
    public static bool isUsePing = true;
    //�������ʱ��
    public static int pingInterval = 30;
    //��һ�η���PING��ʱ��
    static float lastPingTime = 0;
    //��һ���յ�PONG��ʱ��
    static float lastPongTime = 0;

    //��ʼ��״̬
    private static void InitState()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //���ջ�����
        readBuff = new ByteArray();
        //д�����
        writeQueue = new Queue<ByteArray>();
        //��Ϣ�б�
        msgList = new List<MsgBase>();

        isConnecting = false;
        isClosing = false;
        msgCount = 0;
        lastPingTime = Time.time;
        lastPongTime = Time.time;

        //����PONGЭ��
        if(!NetMsg.Instance.dic.ContainsKey("MsgPong"))
        {
            NetMsg.Instance.AddEventListener("MsgPong", OnMsgPong);
        }
    }

    //����PONGЭ��
    private static void OnMsgPong(MsgBase msgBase)
    {
        lastPongTime = Time.time;
        Debug.Log("Receive MsgPong");
    }

    #region �첽����ӿڼ���Ӧ�ص�����
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public static void Connect(string ip, int port)
    {
        //״̬�ж�
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
        //��ʼ����Ա
        InitState();
        //��������
        socket.NoDelay = true;
        //����
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

            //��ʼ����
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
            //��ȡ�������ݳ���
            int count = socket.EndReceive(ar);
            if (count == 0)
            {
                Close();
                return;
            }
            readBuff.writeIdx += count;
            //�����������Ϣ
            OnReceiveData();
            //������������
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

    //���ݴ���
    private static void OnReceiveData()
    {
        if (readBuff.length <= 2)
        {
            return;
        }
        //��ȡ��Ϣ�峤��
        int readIdx = readBuff.readIdx;
        byte[] bytes = readBuff.bytes;
        Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | (bytes[readIdx]));
        if (readBuff.length < bodyLength)
        {
            return;
        }
        readBuff.readIdx += 2;
        //����Э����
        string protoName = MsgBase.DecodeName(readBuff.bytes, readBuff.readIdx, out int nameCount);
        if (protoName == "")
        {
            Debug.Log("OnReceiveData MsgBase.DecodeName fail");
            return;
        }
        readBuff.readIdx += nameCount;
        //����Э����
        int bodyCount = bodyLength - nameCount;
        MsgBase msgBase = MsgBase.Decode(protoName, readBuff.bytes, readBuff.readIdx, bodyCount);
        readBuff.readIdx += bodyCount;
        readBuff.CheckAndMoveBytes();
        //��ӵ���Ϣ����
        lock (msgList)
        {
            msgList.Add(msgBase);
        }
        msgCount++;
        //������ȡ��Ϣ
        if (readBuff.length > 2)
        {
            OnReceiveData();
        }
    }

    /// <summary>
    /// �ر�
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
        //�ж��Ƿ���δ��������
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
        //���ݱ���
        byte[] nameBytes = MsgBase.EncodeName(msg);
        byte[] bodyBytes = MsgBase.Encode(msg);
        int len = nameBytes.Length + bodyBytes.Length;
        byte[] sendBytes = new byte[2 + len];
        //��װ����(С�˱���)
        sendBytes[0] = (byte)(len % 256);
        sendBytes[1] = (byte)(len / 256);
        //��װ����
        Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
        //��װ��Ϣ��
        Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
        //д�����
        ByteArray ba = new ByteArray(sendBytes);
        int WQCount = 0;
        lock (writeQueue)
        {
            writeQueue.Enqueue(ba);
            WQCount = writeQueue.Count;
        }
        //����(�״�������֮����ڻص��������ٵ��÷���)
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
        //��ȡд����еĵ�һ������
        ByteArray ba;
        lock (writeQueue)
        {
            ba = writeQueue.First();
        }
        //��������
        ba.readIdx += count;
        //��ȡд����е���һ������
        if (ba.length == 0)
        {
            lock (writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        //����������������ͣ������������ڹر���ر�����
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

    #region �ڲ�Update��װ
    private static void MsgUpdate()
    {
        if (msgCount == 0) { return; }
        //�ظ�������Ϣ
        for (int i = 0; i < MAX_MESSAGE_DISPATCH; i++)
        {
            //��ȡ��һ����Ϣ
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
            //�ַ���Ϣ
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
        //�Ƿ�����
        if (!isUsePing) { return; }
        if (socket == null || socket.Connected == false) { return; }
        //����PING
        if (Time.time - lastPingTime > pingInterval)
        {
            MsgPing msgPing = new();
            Send(msgPing);
            Debug.Log("Send MsgPing");
            lastPingTime = Time.time;
            Debug.Log($"PongTimeInterval:{Time.time - lastPongTime},TimeNow:{Time.time},LastPongTime:{lastPongTime}");
        }
        //���PONGʱ��
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