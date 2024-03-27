using System;
using UIFrameWork;
using UnityEngine;

public class GameLaunch : UnitySingleton<GameLaunch>
{
    public override void Awake()
    {
        base.Awake();
        Loom.Initialize();
        //���ؿ�ʼҳ��
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
        //���������������Ϣ�ַ�
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
