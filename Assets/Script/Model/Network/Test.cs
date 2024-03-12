using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public InputField idInput;
    public InputField pwInput;
    public InputField textInput;
    //��ʼ
    void Start()
    {
        NetEvent.Instance.AddEventListener(NetEventType.ConnectSucc, OnConnectSucc);
        NetEvent.Instance.AddEventListener(NetEventType.ConnectFail, OnConnectFail);
        NetEvent.Instance.AddEventListener(NetEventType.Close, OnConnectClose);
        NetMsg.Instance.AddEventListener("MsgRegister", OnMsgRegister);
        NetMsg.Instance.AddEventListener("MsgLogin", OnMsgLogin);
        NetMsg.Instance.AddEventListener("MsgKick", OnMsgKick);
        NetMsg.Instance.AddEventListener("MsgGetText", OnMsgGetText);
        NetMsg.Instance.AddEventListener("MsgSaveText", OnMsgSaveText);
    }

    private void Update()
    {
        NetManager.Update();
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

    //����ע��Э��
    public void OnRegisterClick()
    {
        MsgRegister msg = new MsgRegister();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //�յ�ע��Э��
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("ע��ɹ�");
        }
        else
        {
            Debug.Log("ע��ʧ��");
        }
    }

    //���͵�¼Э��
    public void OnLoginClick()
    {
        MsgLogin msg = new MsgLogin();
        msg.id = idInput.text;
        msg.pw = pwInput.text;
        NetManager.Send(msg);
    }

    //�յ���¼Э��
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("��¼�ɹ�");
            //������±��ı�
            MsgGetText msgGetText = new MsgGetText();
            NetManager.Send(msgGetText);
        }
        else
        {
            Debug.Log("��¼ʧ��");
        }
    }

    //��������
    void OnMsgKick(MsgBase msgBase)
    {
        Debug.Log("��������");
    }

    //�յ����±��ı�Э��
    public void OnMsgGetText(MsgBase msgBase)
    {
        MsgGetText msg = (MsgGetText)msgBase;
        textInput.text = msg.text;
    }

    //���ͱ���Э��
    public void OnSaveClick()
    {
        MsgSaveText msg = new MsgSaveText();
        msg.text = textInput.text;
        NetManager.Send(msg);
    }

    //�յ�����Э��
    public void OnMsgSaveText(MsgBase msgBase)
    {
        MsgSaveText msg = (MsgSaveText)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("����ɹ�");
        }
        else
        {
            Debug.Log("����ʧ��");
        }
    }
}