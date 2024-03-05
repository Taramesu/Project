using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/TestPanel";
    public TestPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Btn_Exit", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_Switch", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new TestPanel2());
        });
    }
}
