using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class ConnectFailPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/ConnectFailPanel";
    public ConnectFailPanel() : base(new UIType(path,false))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Btn_OK", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
    }
}