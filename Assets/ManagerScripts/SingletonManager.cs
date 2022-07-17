using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{

    public static T Instance;
    protected virtual void Awake()
    {
        if (Instance == null) Instance = GetComponent<T>();
        //DontDestroyOnLoad(gameObject);
    }

}
