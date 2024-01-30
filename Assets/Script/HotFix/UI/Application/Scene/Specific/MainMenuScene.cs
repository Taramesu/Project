using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using Unity.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Hotfix
{
    public class MainMenuScene : SceneBase
    {
        private static readonly string sceneName = "MainMenuScene";
        public override void OnEnter()
        {
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                UIComponent.Instance.SwitchScene(sceneName);
                SceneManager.sceneLoaded += SceneLoaded;
            }
            else
            {
                PanelManager.Instance.Push(new MainMenuPanel());
            }
        }

        public override void OnExit()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            PanelManager.Instance.Clear();
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PanelManager.Instance.Push(new MainMenuPanel());
        }
    }
}

