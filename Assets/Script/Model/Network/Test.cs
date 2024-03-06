using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class test : MonoBehaviour
{
    //开始
    void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc,
        OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail,
        OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close,
        OnConnectClose);
    }
    //玩家点击连接按钮
    public void OnConnectClick()
    {
        NetManager.Connect("127.0.0.1", 8888);
        //TODO:开始转圈圈，提示“连接中”
    }
    //连接成功回调
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
        //TODO:进入游戏
    }
    //连接失败回调
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail" + err);
        //TODO:弹出提示框(连接失败，请重试)
    }
    //关闭连接
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
        //TODO:弹出提示框(网络断开)
        //TODO:弹出按钮(重新连接)
    }
}