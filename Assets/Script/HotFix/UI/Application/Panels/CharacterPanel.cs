using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/CharacterPanel";
    public CharacterPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Btn_������", panel).onClick.AddListener(() =>
        {
            Debug.Log("������");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_�ȼ�ǿ��", panel).onClick.AddListener(() =>
        {
            Debug.Log("�ȼ�ǿ��");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_����ѧϰ", panel).onClick.AddListener(() =>
        {
            Debug.Log("����ѧϰ");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_װ������/һ��", panel).onClick.AddListener(() =>
        {
            Debug.Log("װ������/һ��");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_����", panel).onClick.AddListener(() =>
        {
            Debug.Log("����");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_��ɫһ��", panel).onClick.AddListener(() =>
        {
            Debug.Log("��ɫһ��");
        });

    }
}
