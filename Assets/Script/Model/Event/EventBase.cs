using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class EventBase<T, P, X> where T : new() where P : class
{


    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    //�洢�¼�ID ���з���(ί��) 
    //ʹ���̰߳�ȫ���ֵ� �����Ժ���̻߳����³�������
    public ConcurrentDictionary<X, List<Action<P>>> dic = new ConcurrentDictionary<X, List<Action<P>>>();

    //����¼�
    public void AddEventListener(X key, Action<P> handle)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(handle);
        }
        else
        {
            List<Action<P>> actions = new List<Action<P>>();
            actions.Add(handle);
            dic[key] = actions;
        }
    }


    //�Ƴ��¼�
    public void RemoveEventListener(X key, Action<P> handle)
    {
        if (dic.ContainsKey(key))
        {
            List<Action<P>> actions = dic[key];
            actions.Remove(handle);

            if (actions.Count == 0)
            {
                List<Action<P>> removeActions;
                dic.TryRemove(key, out removeActions);
            }
        }
    }

    //�ɷ��¼��Ľӿ�-���в���
    public void Dispatch(X key, P p)
    {
        if (dic.ContainsKey(key))
        {
            List<Action<P>> actions = dic[key];
            if (actions != null && actions.Count > 0)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] != null)
                    {
                        actions[i](p);
                    }
                }
            }
        }
    }

    //�ɷ��¼��Ľӿ�-û�в�����
    public void Dispatch(X key)
    {
        Dispatch(key, null);
    }
}
