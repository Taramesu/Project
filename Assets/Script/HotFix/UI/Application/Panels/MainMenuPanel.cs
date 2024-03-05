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
        UITool.GetOrAddComponentInChildren<Button>("Btn_����", panel).onClick.AddListener(() =>
        {
            Debug.Log("����");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new MainCityPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_����", panel).onClick.AddListener(() =>
        {
            Debug.Log("����");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new MainCityPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_��ɫ", panel).onClick.AddListener(() =>
        {
            Debug.Log("��ɫ");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new CharacterPanel(),false);
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_�̵�", panel).onClick.AddListener(() =>
        {
            Debug.Log("�̵�");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_�˵�", panel).onClick.AddListener(() =>
        {
            Debug.Log("�˵�");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_�ظ�����", panel).onClick.AddListener(() =>
        {
            Debug.Log("�ظ�����");
            PanelManager.Instance.Push(new RecoverPowerPanel());
        });
    }
}
