using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    _Instance = obj.AddComponent<T>();
                }
                else
                {
                    _Instance = obj.GetComponent<T>();
                }
            }
            return _Instance;
        }
        set { _Instance = value; }
    }
    protected virtual void Awake()
    {
        if (_Instance == null) _Instance = GetComponent<T>();
        //DontDestroyOnLoad(gameObject);
    }

}
