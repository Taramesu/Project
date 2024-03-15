using System;
using UIFrameWork;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/LoginPanel";
    public LoginPanel() : base(new UIType(path,false))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        InputField acInput = UITool.GetOrAddComponentInChildren<InputField>("Ipt_Account", panel);
        InputField pwInput = UITool.GetOrAddComponentInChildren<InputField>("Ipt_Password", panel);

        UITool.GetOrAddComponentInChildren<Button>("Btn_Login", panel).onClick.AddListener(() =>
        {
            Debug.Log("���Ե�¼");
            MsgLogin msg = new MsgLogin();
            msg.id = acInput.text;
            msg.pw = pwInput.text;
            NetManager.Send(msg);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_Register", panel).onClick.AddListener(() =>
        {
            MsgRegister msg = new MsgRegister();
            msg.id = acInput.text;
            msg.pw = pwInput.text;
            NetManager.Send(msg);
            Debug.Log("����ע��");
        });

        NetMsg.Instance.AddEventListener("MsgLogin", OnMsgLogin);
        NetMsg.Instance.AddEventListener("MsgRegister", OnMsgRegister);
    }

    private void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        Text tips = UITool.GetOrAddComponentInChildren<Text>("Txt_Tips", panel);
        if (msg.result == 0)
        {
            Debug.Log("ע��ɹ�");
            tips.text = "ע��ɹ�";
        }
        else
        {
            Debug.Log("ע��ʧ��");
            tips.text = "ע��ʧ��";
        }
        //try
        //{
        //    Loom.QueueOnMainThread((param) =>
        //    {
        //        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        //        Text tips = UITool.GetOrAddComponentInChildren<Text>("Txt_Tips", panel);
        //        if (msg.result == 0)
        //        {
        //            Debug.Log("ע��ɹ�");
        //            tips.text = "ע��ɹ�";
        //        }
        //        else
        //        {
        //            Debug.Log("ע��ʧ��");
        //            tips.text = "ע��ʧ��";
        //        }
        //    },null);
            
        //}
        //catch (Exception ex) 
        //{
        //    Debug.Log(ex.Message);
        //}
    }

    private void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;

        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        Text tips = UITool.GetOrAddComponentInChildren<Text>("Txt_Tips", panel);
        if (msg.result == 0) 
        {
            Debug.Log("��¼�ɹ�");
            tips.text = "��¼�ɹ�";

            PanelManager.Instance.Pop();
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new HotfixPanel());
        }
        else
        {
            Debug.Log("��¼ʧ��");
            tips.text = "��¼ʧ��";
        }
    }
}