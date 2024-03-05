using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/MainMenuPanel";
    public MainMenuPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Btn_返回", panel).onClick.AddListener(() =>
        {
            Debug.Log("返回");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new MainCityPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_主城", panel).onClick.AddListener(() =>
        {
            Debug.Log("主城");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new MainCityPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_角色", panel).onClick.AddListener(() =>
        {
            Debug.Log("角色");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new CharacterPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_商店", panel).onClick.AddListener(() =>
        {
            Debug.Log("商店");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_菜单", panel).onClick.AddListener(() =>
        {
            Debug.Log("菜单");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_回复体力", panel).onClick.AddListener(() =>
        {
            Debug.Log("回复体力");
            PanelManager.Instance.Push(new RecoverPowerPanel());
        });
    }
}
