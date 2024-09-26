using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Singleton class that inherits from MonoBehaviour
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    // protected virtual void Awake() is a method that can pnly be called by the class itself or by a derived class
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    public bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}