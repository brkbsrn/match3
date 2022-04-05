using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : Component
{
    private T Prefab;
    private List<T> Objects;
    private Transform Parent;

    public Pool(T prefab, Transform parent, int poolCount)
    {
        Prefab = prefab;
        Parent = parent;
        Objects = new List<T>();
        AddObjects(poolCount);
    }

    private void AddObjects(int poolCount)
    {
        for (int i = 0; i < poolCount; i++)
        {
            T instance = GameObject.Instantiate(Prefab, Parent);
            instance.gameObject.SetActive(false);
            Objects.Add(instance);
        }
    }

    public T GetInstance() 
    {
        T instance;
        for (int i = 0; i < Objects.Count; i++)
        {
            if (!Objects[i].gameObject.activeSelf)
            {
                instance = Objects[i];
                instance.gameObject.SetActive(true);
                return instance;
            }
        }

        Debug.Log("Pool expanded : " + Prefab.name);

        AddObjects(1);
        instance = Objects[Objects.Count - 1];
        instance.gameObject.SetActive(true);
        return instance;
    }
}
