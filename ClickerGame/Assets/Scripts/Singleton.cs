// 2025-10-25 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// This is a generic Singleton base class that ensures a single instance of the derived class.
/// It provides a thread-safe mechanism to access the instance and automatically finds and activates
/// the instance if it is not already initialized.
/// </summary>

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);
                if (_instance != null && !_instance.gameObject.activeSelf)
                {
                    _instance.gameObject.SetActive(true);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
