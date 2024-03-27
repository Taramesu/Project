using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
/// <summary>
/// ������
/// </summary>
public interface IResetable
{
    void OnReset();
}
/// <summary>
/// �����
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
    /// ��ָ��λ������ת�´�������
    /// </summary>
    /// <param name="key">���</param>
    /// <param name="prefab">��Ҫ����ʵ����Ԥ�Ƽ�</param>
    /// <param name="pos">λ��</param>
    /// <param name="rotate">��ת</param>
    /// <returns></returns>
    public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate)
    {
        GameObject go;
        go = FindUsableObject(key);

        if (go == null)
        {
            go = AddObject(key, prefab);
        }

        //ʹ��
        UseObject(pos, rotate, go);
        return go;
    }

    /// <summary>
    /// ���ն���
    /// </summary>
    /// <param name="go">��Ҫ�����յ���Ϸ����</param>
    /// <param name="delay">�ӳ�ʱ�� Ĭ��Ϊ0</param>
    public void CollectObject(GameObject go, float delay = 0)
    {
        StartCoroutine(CollectObjectDelay(go, delay));
    }

    private IEnumerator CollectObjectDelay(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

    //ʹ�ö���
    private void UseObject(Vector3 pos, Quaternion rotate, GameObject go)
    {
        go.transform.position = pos;
        go.transform.rotation = rotate;
        go.SetActive(true);
        //����ִ��������������Ҫ���õ��߼�
        foreach (var item in go.GetComponents<IResetable>())
        {
            item.OnReset();
        }
    }

    //���Ӷ���
    private GameObject AddObject(string key, GameObject prefab)
    {
        //��������
        GameObject go = Instantiate(prefab);
        //�������
        if (!cache.ContainsKey(key))
            cache.Add(key, new List<GameObject>());
        cache[key].Add(go);
        return go;
    }

    //����ָ������п���ʹ�õĶ���
    private GameObject FindUsableObject(string key)
    {
        if (cache.ContainsKey(key))
            return cache[key].Find(go => !go.activeInHierarchy);
        return null;
    }

    /// <summary>
    /// ���ĳһ������
    /// </summary>
    /// <param name="key">���</param>
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
    /// ������ж���
    /// </summary>
    public void ClearAll()
    {
        foreach (var key in new List<string>(cache.Keys))
        {
            Clear(key);
        }
    }
}