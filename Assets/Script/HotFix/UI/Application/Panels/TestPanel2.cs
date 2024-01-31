using UIFrameWork;
using UnityEngine;

public class TestPanel2 : BasePanel
{
    private static readonly string path = "Prefab/Panel/TestPanel2";
    public TestPanel2() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        
    }
}
