using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class StartGamePanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/StartGamePanel";
    public StartGamePanel() : base(new UIType(path,false))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Btn_StartGame", panel).onClick.AddListener(() =>
        {
            Debug.Log("¿ªÊ¼ÓÎÏ·");
            PanelManager.Instance.Push(new ConnectingPanel());
        });
    }
}