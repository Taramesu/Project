using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}

public class SingletonBase<T> where T : class, new()
{
    private static T _instance;
    private static readonly object syslock = new object();

    protected SingletonBase()
    {

    }
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                        _instance = new T();
                }
            }
            return _instance;
        }
    }
}
