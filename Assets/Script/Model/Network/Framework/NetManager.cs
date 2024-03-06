using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;

public static class NetManager
{
    //�¼�
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

    //�¼�ί������
    public delegate void EventListener(string err);
    //�¼������б�
    private static Dictionary<NetEvent, EventListener> eventListeners = new Dictionary<NetEvent, EventListener>();

    /// <summary>
    /// ����¼�����
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
    /// ɾ���¼�����
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
    /// �ַ��¼�
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

    /// <summary>
    /// �ر�
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
        //�ж��Ƿ���δ��������
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
        Array.Copy(bodyBytes, 0, sendBytes, 2+nameBytes.Length, bodyBytes.Length);
        //д�����
        ByteArray ba = new ByteArray(sendBytes);
        int WQCount = 0;
        lock(writeQueue)
        {
            writeQueue.Enqueue(ba);
            WQCount = writeQueue.Count;
        }
        //����
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
        //��ȡд����еĵ�һ������
        ByteArray ba;
        lock(writeQueue) 
        {
            ba = writeQueue.First();
        }
        //��������
        ba.readIdx += count;
        //��ȡд����е���һ������
        if(ba.length == 0)
        {
            lock(writeQueue)
            {
                writeQueue.Dequeue();
                ba = writeQueue.First();
            }
        }
        //����������������ͣ������������ڹر���ر�����
        if(ba != null)
        {
            socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
        }
        else if(isClosing)
        {
            socket.Close();
        }
    }

    //��ʼ��״̬
    private static void InitState()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //���ջ�����
        readBuff = new ByteArray();
        //д�����
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