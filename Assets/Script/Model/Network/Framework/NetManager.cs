using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Linq;

public static class NetManager
{
    static Socket socket;
    static ByteArray readBuff;
    static Queue<ByteArray> wirteQueue;
    
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
        if(eventListeners.ContainsKey(netEvent)) 
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
        if(eventListeners.ContainsKey(netEvent)) 
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
}

//事件
public enum NetEvent
{
    ConnectSucc = 1,
    ConnectFail = 2,
    Close = 3
}