using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//游戏的根管理器
namespace Unity.Model
{
    public class UIComponent : UnitySingleton<UIComponent>
    {
        //public static GameRoot Instance { get; private set; }

        private new void Awake()
        {
            //    if (Instance == null)
            //        Instance = this;
            //    else
            //        Destroy(gameObject);
            //    DontDestroyOnLoad(gameObject);
            base.Awake();
        }
        private void Start()
        {
            //SceneSystem.Instance.SetScene(new MainScene());
        }

        public void SwitchScene(string sceneName)
        {
            StartCoroutine(Delay(sceneName));
        }

        private IEnumerator Delay(string sceneName)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
            while (!ao.isDone)
            {
                yield return new WaitForSeconds(3.0f);
            }

        }
    }
}

