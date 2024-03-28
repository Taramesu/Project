using System;
using UIFrameWork;
using UnityEngine;

public class GameLaunch : UnitySingleton<GameLaunch>
{
    public override void Awake()
    {
        base.Awake();
        Loom.Initialize();
        //加载开始页面
        PanelManager.Instance.Push(new StartGamePanel());
        NetworkInit();
    }

    private void NetworkInit()
    {
        NetEvent.Instance.AddEventListener(NetEventType.ConnectSucc, OnConnectSucc);
        NetEvent.Instance.AddEventListener(NetEventType.ConnectFail, OnConnectFail);
        NetEvent.Instance.AddEventListener(NetEventType.Close, OnConnectClose);
        NetMsg.Instance.AddEventListener("MsgKick", OnKick);
    }

    private void Update()
    {
        //驱动网络管理器消息分发
        NetManager.Update();
    }

    private void OnConnectSucc(string err)
    {
        //Debug.Log(err);
    }

    private void OnConnectFail(string err)
    {
        //Debug.Log(err);
    }

    private void OnConnectClose(string err)
    {
        throw new NotImplementedException();
    }

    private void OnKick(MsgBase msg)
    {

    }
}
