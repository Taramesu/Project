using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class MainCityPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/MainCityPanel";
    public MainCityPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Btn_�ؿ�", panel).onClick.AddListener(() =>
        {
            Debug.Log("�ؿ�");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_�", panel).onClick.AddListener(() =>
        {
            Debug.Log("�");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_����ս", panel).onClick.AddListener(() =>
        {
            Debug.Log("����ս");
        });

    }
}
