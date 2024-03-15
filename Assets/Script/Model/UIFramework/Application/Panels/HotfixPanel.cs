using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class HotfixPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/HotfixPanel";
    public HotfixPanel() : base(new UIType(path,false))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        BundleDownloaderComponent.Instance.state = UITool.GetOrAddComponentInChildren<Text>("Txt_State", panel);
        BundleDownloaderComponent.Instance.size = UITool.GetOrAddComponentInChildren<Text>("Txt_Size", panel);
        BundleDownloaderComponent.Instance.progressBar = UITool.GetOrAddComponentInChildren<Image>("Img_ProgressBar", panel);

        BundleDownloaderComponent.Instance.StartDownload();
    }
}