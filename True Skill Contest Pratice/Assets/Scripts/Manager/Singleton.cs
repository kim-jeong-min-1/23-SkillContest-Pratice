using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    protected void SetInstance()
    {
        if(instance != null) 
        {
            Destroy(this.gameObject);
            return;
        }

        if(instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}

public class DestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected void SetInstance()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (instance == null)
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }
        }
    }
}
