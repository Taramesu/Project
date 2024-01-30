using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UIFrameWork;

namespace Unity.Hotfix
{
    public class Init 
    {
        static Transform canvas;
        static Transform gameApp;
        public static void Start() {

            Debug.Log("热更新dll的入口!");
            canvas = GameObject.Find("Canvas").transform;
            gameApp = GameObject.Find("GameApp").transform;
            gameApp.gameObject.AddComponent<UIComponent>();
            Debug.Log("添加ui组件");

            UnityEngine.Object.Destroy(GameObject.Find("Canvas/HotfixPanel").gameObject);
            PanelManager.Instance.Push(new MainMenuPanel());
            PanelManager.Instance.Push(new MainCityPanel(),false);
            //PanelManager.Instance.Push(new CharacterPanel(),false);

            //UnityEngine.Object go = ResourcesComponent.Instance.GetAsset("LoginPanel", "prefab/LoginPanel");
            //UnityEngine.Object go = res.GetAsset("LoadTest", "prefab/LoadTest");
            //GameObject loginPanel=(GameObject)GameObject.Instantiate(go);

            //loginPanel.transform.SetParent(canvas, false);

            //loginPanel.transform.Find("logonBtn").GetComponent<Button>().onClick.AddListener(loginBtnOnClick);
            //res.UnloadBundle("prefab/LoginPanel");
        }

        private static void loginBtnOnClick()
        {
            //res.GetAsset("mobaScene", "scene/mobaScene",true);
            //SceneManager.LoadScene("mobaScene");
            Debug.Log("进入场景!");
            //res.UnloadBundle("prefab/LoginPanel");

        }
    }
}