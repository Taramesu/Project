using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class RecoverPowerPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/RecoverPowerPanel";
    public RecoverPowerPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Btn_取消", panel).onClick.AddListener(() =>
        {
            Debug.Log("取消");
            PanelManager.Instance.Pop();
        });
    }
}
