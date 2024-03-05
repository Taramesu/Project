using UIFrameWork;
using UnityEngine.SceneManagement;

public class MainScene : SceneBase
{
    private static readonly string sceneName = "MainScene";
    public override void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            Unity.Model.UIComponent.Instance.SwitchScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            //PanelManager.Instance.Push(new MainMenuPanel());
        }
    }

    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        PanelManager.Instance.Clear();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //PanelManager.Instance.Push(new MainMenuPanel());
    }
}

