// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// UI_ClickerGame is a UI class for managing the main clicker game interface.
/// It adheres to the following rules:
/// [1] Understands GameManager's code.
/// [2] Uses TextMeshPro for text elements.
/// [3] Uses [SerializeField] for private variables to enable debugging in the Unity Editor.
/// [4] Implements functionality for switching tabs and updating UI elements.
/// [5] Handles Gold increment based on GPC when GameArea is clicked.
/// [6] Handles Gold increment based on GPS every second.
/// [7] Updates StageText, GoldPerClickText, GoldPerSecText, and GoldText based on GameManager values.
/// [8] Inherits from UI_Base and follows the common requirements.
///
/// This version uses GameManager's subscriber pattern (events) to update UI when values change.
/// </summary>

public class UI_ClickerGame : UI_Base
{
    [SerializeField] private Button _gpcButton; // Button to activate GPCTab
    [SerializeField] private Button _gpsButton; // Button to activate GPSTab
    [SerializeField] private Button _processButton; // Button to activate ProcessTab
    [SerializeField] private Button _shopButton; // Button to activate ShopTab
    [SerializeField] private GameObject _gpcTab; // GPCTab GameObject
    [SerializeField] private GameObject _gpsTab; // GPSTab GameObject
    [SerializeField] private GameObject _processTab; // ProcessTab GameObject
    [SerializeField] private GameObject _shopTab; // ShopTab GameObject
    [SerializeField] private Button _gameArea; // GameArea for clicking to earn Gold
    [SerializeField] private TMP_Text _stageText; // TextMeshPro for displaying Stage
    [SerializeField] private TMP_Text _goldPerClickText; // TextMeshPro for displaying GoldPerClick
    [SerializeField] private TMP_Text _goldPerSecText; // TextMeshPro for displaying GoldPerSecond
    [SerializeField] private TMP_Text _goldText; // TextMeshPro for displaying Gold

    private float _goldIncrementTimer = 0f; // Timer for GPS-based gold increment

    protected override void Awake()
    {
        base.Awake();

        // Dynamically find child objects and components
        _gpcButton = FindChildComponent<Button>("GPCButton");
        _gpsButton = FindChildComponent<Button>("GPSButton");
        _processButton = FindChildComponent<Button>("ProcessButton");
        _shopButton = FindChildComponent<Button>("ShopButton");
        _gpcTab = FindChildGameObject("GPCTab");
        _gpsTab = FindChildGameObject("GPSTab");
        _processTab = FindChildGameObject("ProcessTab");
        _shopTab = FindChildGameObject("ShopTab");
        _gameArea = FindChildComponent<Button>("GameArea");
        _stageText = FindChildComponent<TMP_Text>("StageText");
        _goldPerClickText = FindChildComponent<TMP_Text>("GoldPerClickText");
        _goldPerSecText = FindChildComponent<TMP_Text>("GoldPerSecText");
        _goldText = FindChildComponent<TMP_Text>("GoldText");
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
        SubscribeToGameManager();
        RefreshUI();
    }

    protected override void Update()
    {
        base.Update();
        UpdateGoldPerSecond();
    }

    // UI_Base does not define OnDestroy as virtual, so do not use override here.
    protected virtual void OnDestroy()
    {
        UnsubscribeFromGameManager();
    }

    /// <summary>
    /// Binds UI events such as button click listeners.
    /// </summary>
    protected override void BindUIEvents()
    {
        if (_gpcButton != null)
        {
            _gpcButton.onClick.RemoveAllListeners();
            _gpcButton.onClick.AddListener(() => ActivateTab(_gpcTab));
        }

        if (_gpsButton != null)
        {
            _gpsButton.onClick.RemoveAllListeners();
            _gpsButton.onClick.AddListener(() => ActivateTab(_gpsTab));
        }

        if (_processButton != null)
        {
            _processButton.onClick.RemoveAllListeners();
            _processButton.onClick.AddListener(() => ActivateTab(_processTab));
        }

        if (_shopButton != null)
        {
            _shopButton.onClick.RemoveAllListeners();
            _shopButton.onClick.AddListener(() => ActivateTab(_shopTab));
        }

        if (_gameArea != null)
        {
            _gameArea.onClick.RemoveAllListeners();
            _gameArea.onClick.AddListener(() => AddGoldOnClick());
        }
    }

    /// <summary>
    /// Refreshes the UI elements to reflect the current game state.
    /// </summary>
    protected override void RefreshUI()
    {
        if (_stageText != null)
        {
            _stageText.text = $"Stage: {GameManager.Instance.Stage}";
        }

        if (_goldPerClickText != null)
        {
            _goldPerClickText.text = $"Gold Per Click: {GameManager.Instance.GoldPerClick}";
        }

        if (_goldPerSecText != null)
        {
            _goldPerSecText.text = $"Gold Per Second: {GameManager.Instance.GoldPerSecond}";
        }

        if (_goldText != null)
        {
            _goldText.text = $"Gold: {GameManager.Instance.Gold}";
        }
    }

    /// <summary>
    /// Activates the specified tab and deactivates others.
    /// </summary>
    /// <param name="tab">The tab GameObject to activate.</param>
    private void ActivateTab(GameObject tab)
    {
        if (_gpcTab != null) _gpcTab.SetActive(false);
        if (_gpsTab != null) _gpsTab.SetActive(false);
        if (_processTab != null) _processTab.SetActive(false);
        if (_shopTab != null) _shopTab.SetActive(false);

        if (tab != null)
        {
            tab.SetActive(true);
        }
    }

    /// <summary>
    /// Adds Gold based on GoldPerClick when the GameArea is clicked.
    /// Uses GameManager events to update UI (no direct RefreshUI call here).
    /// </summary>
    private void AddGoldOnClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Gold += GameManager.Instance.GoldPerClick;
        }
    }

    /// <summary>
    /// Adds Gold based on GoldPerSecond every second.
    /// Uses GameManager events to update UI (no direct RefreshUI call here).
    /// </summary>
    private void UpdateGoldPerSecond()
    {
        _goldIncrementTimer += Time.deltaTime;
        if (_goldIncrementTimer >= 1f)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Gold += GameManager.Instance.GoldPerSecond;
            }
            _goldIncrementTimer = 0f;
        }
    }

    /// <summary>
    /// Subscribes to GameManager events so UI updates when data changes.
    /// </summary>
    private void SubscribeToGameManager()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGoldChanged += HandleGoldChanged;
        GameManager.Instance.OnGoldPerClickChanged += HandleGoldPerClickChanged;
        GameManager.Instance.OnGoldPerSecondChanged += HandleGoldPerSecondChanged;
        GameManager.Instance.OnStageChanged += HandleStageChanged;
    }

    /// <summary>
    /// Unsubscribes from GameManager events.
    /// </summary>
    private void UnsubscribeFromGameManager()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnGoldChanged -= HandleGoldChanged;
        GameManager.Instance.OnGoldPerClickChanged -= HandleGoldPerClickChanged;
        GameManager.Instance.OnGoldPerSecondChanged -= HandleGoldPerSecondChanged;
        GameManager.Instance.OnStageChanged -= HandleStageChanged;
    }

    private void HandleGoldChanged()
    {
        if (_goldText != null)
        {
            _goldText.text = $"Gold: {GameManager.Instance.Gold}";
        }
    }

    private void HandleGoldPerClickChanged()
    {
        if (_goldPerClickText != null)
        {
            _goldPerClickText.text = $"Gold Per Click: {GameManager.Instance.GoldPerClick}";
        }
    }

    private void HandleGoldPerSecondChanged()
    {
        if (_goldPerSecText != null)
        {
            _goldPerSecText.text = $"Gold Per Second: {GameManager.Instance.GoldPerSecond}";
        }
    }

    private void HandleStageChanged()
    {
        if (_stageText != null)
        {
            _stageText.text = $"Stage: {GameManager.Instance.Stage}";
        }
    }
}
