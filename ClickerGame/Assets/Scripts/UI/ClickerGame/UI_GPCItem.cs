// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// UI_GPCItem is a UI class that manages the GPC upgrade items.
/// It adheres to the following rules:
/// [1] Understands GameManager's code.
/// [2] Uses TextMeshPro for text elements.
/// [3] Uses [SerializeField] for private variables to enable debugging in the Unity Editor.
/// [4] Uses UnityEngine, UnityEngine.UI, and TMPro namespaces.
/// [5] Implements utility functions for finding child components, game objects, and managing popups.
/// [6] Implements virtual functions for binding UI events and refreshing UI.
/// [7] Provides virtual Unity event functions (Awake, Start, Update) for derived classes.
/// </summary>

public class UI_GPCItem : UI_Base
{
    [SerializeField] private EGPCUpgradeType _gpcUpgradeType; // GPCUpgrade type, editable in Unity Editor
    [SerializeField] private Button _clickButton; // Button to trigger GPC upgrade
    [SerializeField] private int _upgradeLevel; // Level of the GPC upgrade

    protected override void Awake()
    {
        base.Awake();
        // Dynamically find child objects and components
        _clickButton = FindChildComponent<Button>("ClickButton");
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
    }

    protected override void Update()
    {
        base.Update();
        // Additional per-frame logic if needed
    }

    /// <summary>
    /// Binds UI events such as button click listeners.
    /// </summary>
    protected override void BindUIEvents()
    {
        if (_clickButton != null)
        {
            _clickButton.onClick.RemoveAllListeners();
            _clickButton.onClick.AddListener(() => IncreaseGPC());
        }
    }

    /// <summary>
    /// Increases the GoldPerClick (GPC) based on the upgrade type and level.
    /// </summary>
    private void IncreaseGPC()
    {
        int increaseAmount = 0;

        switch (_gpcUpgradeType)
        {
            case EGPCUpgradeType.A:
                increaseAmount = _upgradeLevel * 10;
                break;
            case EGPCUpgradeType.B:
                increaseAmount = _upgradeLevel * 20;
                break;
            case EGPCUpgradeType.C:
                increaseAmount = _upgradeLevel * 30;
                break;
            case EGPCUpgradeType.D:
                increaseAmount = _upgradeLevel * 40;
                break;
            case EGPCUpgradeType.E:
                increaseAmount = _upgradeLevel * 50;
                break;
        }

        GameManager.Instance.GoldPerClick += increaseAmount;
        _upgradeLevel++;
    }
}
