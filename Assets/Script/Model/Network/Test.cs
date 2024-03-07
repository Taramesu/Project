using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class test : MonoBehaviour
{
    //��ʼ
    void Start()
    {
        NetEvent.Instance.AddEventListener(NetEventType.ConnectSucc, OnConnectSucc);
        NetEvent.Instance.AddEventListener(NetEventType.ConnectFail, OnConnectFail);
        NetEvent.Instance.AddEventListener(NetEventType.Close, OnConnectClose);
        NetMsg.Instance.AddEventListener("MsgMove", OnMsgMove);
    }

    private void Update()
    {
        NetManager.Update();
    }

    private void OnMsgMove(MsgBase msgBase)
    {
        MsgMove msg = (MsgMove)msgBase;
        Debug.Log("OnMsgMove msg.x = " + msg.x);
        Debug.Log("OnMsgMove msg.y = " + msg.y);
        Debug.Log("OnMsgMove msg.z = " + msg.z);
    }

    //��ҵ�����Ӱ�ť
    public void OnConnectClick()
    {
        NetManager.Connect("127.0.0.1", 8888);
        //TODO:��ʼתȦȦ����ʾ�������С�
    }
    //���ӳɹ��ص�
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
        //TODO:������Ϸ
    }
    //����ʧ�ܻص�
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail" + err);
        //TODO:������ʾ��(����ʧ�ܣ�������)
    }
    //�ر�����
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
        //TODO:������ʾ��(����Ͽ�)
        //TODO:������ť(��������)
    }

    //�����ر�
    public void OnCloseClick()
    {
        NetManager.Close();
    }

    public void OnMoveClick()
    {
        MsgMove msg = new MsgMove();
        msg.x = 120;
        msg.y = 123;
        msg.z = -6;
        NetManager.Send(msg);
    }
}