using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class test : MonoBehaviour
{
    //��ʼ
    void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc,
        OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail,
        OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close,
        OnConnectClose);
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
}