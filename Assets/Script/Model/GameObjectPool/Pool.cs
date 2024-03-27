using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
/// <summary>
/// 可重置
/// </summary>
public interface IResetable
{
    void OnReset();
}
/// <summary>
/// 对象池
/// </summary>
public class Pool : UnitySingleton<Pool>
{
    private Dictionary<string, List<GameObject>> cache;

    public override void Awake()
    {
        base.Awake();
        cache = new();
    }
    /// <summary>
    /// 在指定位置与旋转下创建对象
    /// </summary>
    /// <param name="key">类别</param>
    /// <param name="prefab">需要创建实例的预制件</param>
    /// <param name="pos">位置</param>
    /// <param name="rotate">旋转</param>
    /// <returns></returns>
    public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate)
    {
        GameObject go;
        go = FindUsableObject(key);

        if (go == null)
        {
            go = AddObject(key, prefab);
        }

        //使用
        UseObject(pos, rotate, go);
        return go;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="go">需要被回收的游戏对象</param>
    /// <param name="delay">延迟时间 默认为0</param>
    public void CollectObject(GameObject go, float delay = 0)
    {
        StartCoroutine(CollectObjectDelay(go, delay));
    }

    private IEnumerator CollectObjectDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    //使用对象
    private void UseObject(Vector3 pos, Quaternion rotate, GameObject go)
    {
        go.transform.position = pos;
        go.transform.rotation = rotate;
        go.SetActive(true);
        //遍历执行物体中所有需要重置的逻辑
        foreach (var item in go.GetComponents<IResetable>())
        {
            item.OnReset();
        }
    }

    //添加对象
    private GameObject AddObject(string key, GameObject prefab)
    {
        //创建对象
        GameObject go = Instantiate(prefab);
        //加入池中
        if (!cache.ContainsKey(key))
            cache.Add(key, new List<GameObject>());
        cache[key].Add(go);
        return go;
    }

    //查找指定类别中可以使用的对象
    private GameObject FindUsableObject(string key)
    {
        if (cache.ContainsKey(key))
            return cache[key].Find(go => !go.activeInHierarchy);
        return null;
    }

    /// <summary>
    /// 清除某一类别对象
    /// </summary>
    /// <param name="key">类别</param>
    public void Clear(string key)
    {
        if (!cache.ContainsKey(key)) return;
        for (int i = cache[key].Count - 1; i >= 0; i--)
        {
            Destroy(cache[key][i]);
        }

        cache.Remove(key);
    }

    /// <summary>
    /// 清空所有对象
    /// </summary>
    public void ClearAll()
    {
        foreach (var key in new List<string>(cache.Keys))
        {
            Clear(key);
        }
    }
}