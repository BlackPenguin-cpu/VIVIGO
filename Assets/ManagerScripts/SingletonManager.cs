using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance;
    public static T _Instance
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
        set { Instance = value; }
    }
    protected virtual void Awake()
    {
        if (Instance == null) Instance = GetComponent<T>();
        //DontDestroyOnLoad(gameObject);
    }

}
