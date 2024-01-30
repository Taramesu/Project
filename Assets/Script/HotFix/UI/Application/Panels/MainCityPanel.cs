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

        UITool.GetOrAddComponentInChildren<Button>("Btn_关卡", panel).onClick.AddListener(() =>
        {
            Debug.Log("关卡");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_活动", panel).onClick.AddListener(() =>
        {
            Debug.Log("活动");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_领主战", panel).onClick.AddListener(() =>
        {
            Debug.Log("领主战");
        });

    }
}
