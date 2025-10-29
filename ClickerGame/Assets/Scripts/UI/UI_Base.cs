// 2025-10-25 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This is a base class for UI components. It provides utility methods for finding child components
/// and game objects, as well as virtual methods for binding UI events and refreshing UI.
/// Derived classes should use TextMeshPro for text elements and follow the specified coding conventions.
/// </summary>

public abstract class UI_Base : MonoBehaviour
{
    /// <summary>
    /// Finds a child component of type T by name. Considers inactive objects.
    /// </summary>
    /// <typeparam name="T">Type of the component to find.</typeparam>
    /// <param name="name">Name of the child GameObject.</param>
    /// <returns>Found component of type T.</returns>
    protected T FindChildComponent<T>(string name) where T : Component
    {
        T[] components = GetComponentsInChildren<T>(true);
        foreach (var component in components)
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }

    /// <summary>
    /// Finds a child GameObject by name. Considers inactive objects.
    /// </summary>
    /// <param name="name">Name of the child GameObject.</param>
    /// <returns>Found GameObject.</returns>
    protected GameObject FindChildGameObject(string name)
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        foreach (var transform in transforms)
        {
            if (transform.gameObject.name == name)
            {
                return transform.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Finds and activates a UI popup of type T.
    /// </summary>
    /// <typeparam name="T">Type of the UI popup.</typeparam>
    /// <returns>Activated UI popup of type T.</returns>
    public T ShowPopup<T>() where T : UI_Base
    {
        T popup = FindAnyObjectByType<T>(FindObjectsInactive.Include);
        if (popup != null)
        {
            popup.gameObject.SetActive(true);
        }
        return popup;
    }

    /// <summary>
    /// Finds and deactivates a UI popup of type T.
    /// </summary>
    /// <typeparam name="T">Type of the UI popup.</typeparam>
    /// <returns>Deactivated UI popup of type T.</returns>
    public T ClosePopup<T>() where T : UI_Base
    {
        T popup = FindAnyObjectByType<T>(FindObjectsInactive.Include);
        if (popup != null)
        {
            popup.gameObject.SetActive(false);
        }
        return popup;
    }

    /// <summary>
    /// Virtual method to bind UI events such as button click listeners.
    /// Should be called in the Start method.
    /// </summary>
    protected virtual void BindUIEvents()
    {
        // Implement event binding logic here
    }

    /// <summary>
    /// Virtual method to refresh UI elements when data changes.
    /// Should be called in the Start method.
    /// </summary>
    protected virtual void RefreshUI()
    {
        // Implement UI refresh logic here
    }

    protected virtual void Awake()
    {
        // Implement initialization logic here
    }

    protected virtual void Start()
    {
        BindUIEvents();
        RefreshUI();
    }

    protected virtual void Update()
    {
        // Implement per-frame logic here
    }
}
