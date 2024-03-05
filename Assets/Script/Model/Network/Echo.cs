using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Echo : MonoBehaviour
{
    Socket socket;

    public InputField inputField;
    public Text text;

    byte[] readBuff = new byte[1024];
    string recvStr = "";

    public void Connection()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.BeginConnect("127.0.0.1", 8888, ConnectCallback, socket);
    }

    public void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallback, socket);
        }  
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail : " + ex.ToString());
        }
    }

    public void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            string s = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            recvStr = s + "\n" + recvStr;
            Debug.Log("get message:"+recvStr+"½ÓÊÕ×Ö·ûÊý£º"+count);
            Debug.Log("Socket Receive Succ");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail : " + ex.ToString());
        }
    }

    public void Send()
    {
        string sendStr = inputField.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr, 0, sendStr.Length);
        socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
        Debug.Log("Socket send succ");

        //readBuff = new byte[1024];
        //int count = socket.Receive(readBuff);
        //recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
    }

    public void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            Debug.Log("Socket Send succ" + count);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Send fail : " + ex.ToString());
        }
    }

    public void Update()
    {
        text.text = recvStr;
    }
}