using System;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/ConnectingPanel";
    public ConnectingPanel() : base(new UIType(path,false))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Text>("Txt_Loading", panel).text = "连接中，请稍等";
        Button ok = UITool.GetOrAddComponentInChildren<Button>("Btn_OK", panel);
        ok.onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        ok.gameObject.SetActive(false);

        //尝试连接服务器
        NetManager.Connect("127.0.0.1", 8888);
        NetEvent.Instance.AddEventListener(NetEventType.ConnectSucc, OnConnectSucc);
        NetEvent.Instance.AddEventListener(NetEventType.ConnectFail, OnConnectFail);
    }

    private void OnConnectSucc(string err)
    {
        Debug.Log(err);
        Loom.QueueOnMainThread((param) =>
        {
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new LoginPanel());
        },null);
    }

    private void OnConnectFail(string err)
    {
        Debug.Log(err);
        Loom.QueueOnMainThread((param) =>
        {
            PanelManager.Instance.Pop();    
            PanelManager.Instance.Push(new ConnectFailPanel());
        },null);
    }
}