using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;

public static class NetManager
{
    //事件
    public enum NetEvent
    {
        ConnectSucc = 1,
        ConnectFail = 2,
        Close = 3
    }

    static Socket socket;
    static ByteArray readBuff;
    static Queue<ByteArray> writeQueue;

    static bool isConnecting = false;
    static bool isClosing = false;

    //事件委托类型
    public delegate void EventListener(string err);
    //事件监听列表
    private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="listener"></param>
    public static void AddEventListener(NetEvent netEvent, EventListener listener)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] += listener;
        }
        else
        {
            eventListeners[netEvent] = listener;
        }
    }

    /// <summary>
    /// 删除事件监听
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="listener"></param>
    public static void RemoveEventListener(NetEvent netEvent, EventListener listener)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent] -= listener;
            if (eventListeners[netEvent] == null)
            {
                eventListeners.Remove(netEvent);
            }
        }
    }

    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="netEvent"></param>
    /// <param name="err"></param>
    private static void FireEvent(NetEvent netEvent, string err)
    {
        if (eventListeners.ContainsKey(netEvent))
        {
            eventListeners[netEvent](err);
        }
    }

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

    /// <summary>
    /// 关闭
    /// </summary>
    public static void Close()
    {
        if(socket == null || !socket.Connected)
        {
            return;
        }
        if (isConnecting) 
        {
            return;
        }
        //判断是否有未发送数据
        if(writeQueue.Count > 0) 
        {
            isClosing = true;
        }
        else
        {
            socket.Close();
            FireEvent(NetEvent.Close,"");
        }
    }

    public static void Send(MsgBase msg) 
    {
        if(socket == null || !socket.Connected)
        { return; }
        if(isConnecting)
        { return; }
        if(isClosing)
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
        Array.Copy(bodyBytes, 0, sendBytes, 2+nameBytes.Length, bodyBytes.Length);
        //写入队列
        ByteArray ba = new ByteArray(sendBytes);
        int WQCount = 0;
        lock(writeQueue)
        {
            writeQueue.Enqueue(ba);
            WQCount = writeQueue.Count;
        }
        //发送
        if(WQCount == 1) 
        {
            socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        Socket socket = (Socket)ar.AsyncState;
        if(socket == null || !socket.Connected)
        {  return; }
        int count = socket.EndSend(ar);
        //获取写入队列的第一条数据
        ByteArray ba;
        lock(writeQueue) 
        {
            ba = writeQueue.First();
        }
        //完整发送
        ba.readIdx += count;
        //获取写入队列的下一条数据
        if(ba.length == 0)
        {
            lock(writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        //还有数据则继续发送，发送完且正在关闭则关闭连接
        if(ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
        }
        else if(isClosing)
        {
            socket.Close();
        }
    }

    //初始化状态
    private static void InitState()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //接收缓冲区
        readBuff = new ByteArray();
        //写入队列
        writeQueue = new Queue<ByteArray>();
        isConnecting = false;
        isClosing = false;
    }

    private static void ConncetCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ");
            FireEvent(NetEvent.ConnectSucc, "");
            isConnecting = false;
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail " + ex.ToString());
            FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }
}