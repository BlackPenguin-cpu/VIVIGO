using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    public T instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    Instance = obj.AddComponent<T>();
                }
                else
                {
                    Instance = obj.GetComponent<T>();
                }
            }
            return Instance;
        }
    }
    protected virtual void Awake()
    {
        if (Instance == null) Instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }

}
