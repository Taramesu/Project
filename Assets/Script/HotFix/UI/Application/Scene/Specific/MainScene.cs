using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Hotfix
{
    public class MainScene : SceneBase
    {
        private static readonly string sceneName = "MainScene";
        public override void OnEnter()
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                Model.UIComponent.Instance.SwitchScene(sceneName);
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
}

