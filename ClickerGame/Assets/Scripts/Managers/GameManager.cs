// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// GameManager is a Singleton class responsible for managing the game state and related data.
/// It provides public access to all variables through getters and setters, implements observer pattern
/// using event Actions to notify UI updates, and manages data using Lists and Dictionaries.
/// </summary>

public enum EGPCUpgradeType { Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7, Tier8, Tier9, Tier10 }
public enum EGPSUpgradeType { Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7, Tier8, Tier9, Tier10 }
public enum EProcessUpgradeType { Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7, Tier8, Tier9, Tier10 }

public class GameManager : Singleton<GameManager>
{
    // Gold-related variables (double로 변경하여 큰 숫자 지원)
    [SerializeField] private double _gold;
    public double Gold
    {
        get => _gold;
        set
        {
            if (System.Math.Abs(_gold - value) > 0.001)
            {
                _gold = value;
                OnGoldChanged?.Invoke();
            }
        }
    }
    public event Action OnGoldChanged;

    [SerializeField] private double _goldPerClick;
    public double GoldPerClick
    {
        get => _goldPerClick;
        set
        {
            if (System.Math.Abs(_goldPerClick - value) > 0.001)
            {
                _goldPerClick = value;
                OnGoldPerClickChanged?.Invoke();
            }
        }
    }
    public event Action OnGoldPerClickChanged;

    [SerializeField] private double _goldPerSecond;
    public double GoldPerSecond
    {
        get => _goldPerSecond;
        set
        {
            if (System.Math.Abs(_goldPerSecond - value) > 0.001)
            {
                _goldPerSecond = value;
                OnGoldPerSecondChanged?.Invoke();
            }
        }
    }
    public event Action OnGoldPerSecondChanged;

    // Stage-related variable
    [SerializeField] private int _stage = 1;
    public int Stage
    {
        get => _stage;
        set
        {
            if (_stage != value)
            {
                _stage = value;
                OnStageChanged?.Invoke();
            }
        }
    }
    public event Action OnStageChanged;

    // Revive-related variable
    [SerializeField] private int _reviveCount = 0;
    public int ReviveCount
    {
        get => _reviveCount;
        private set
        {
            if (_reviveCount != value)
            {
                _reviveCount = value;
                OnReviveCountChanged?.Invoke();
            }
        }
    }
    public event Action OnReviveCountChanged;

    // Language-related variable
    [SerializeField] private ELanguage _currentLanguage = ELanguage.Korean;
    public ELanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                if (LocalizationManager.Instance != null)
                {
                    LocalizationManager.Instance.CurrentLanguage = value;
                }
                OnLanguageChanged?.Invoke();
            }
        }
    }
    public event Action OnLanguageChanged;

    // Upgrade-related variables
    [SerializeField] private Dictionary<EGPCUpgradeType, int> _gpcUpgrades = new Dictionary<EGPCUpgradeType, int>
    {
        { EGPCUpgradeType.Tier1, 1 },
        { EGPCUpgradeType.Tier2, 1 },
        { EGPCUpgradeType.Tier3, 1 },
        { EGPCUpgradeType.Tier4, 1 },
        { EGPCUpgradeType.Tier5, 1 },
        { EGPCUpgradeType.Tier6, 1 },
        { EGPCUpgradeType.Tier7, 1 },
        { EGPCUpgradeType.Tier8, 1 },
        { EGPCUpgradeType.Tier9, 1 },
        { EGPCUpgradeType.Tier10, 1 }
    };
    public Dictionary<EGPCUpgradeType, int> GPCUpgrades => _gpcUpgrades;

    [SerializeField] private Dictionary<EGPSUpgradeType, int> _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
    {
        { EGPSUpgradeType.Tier1, 1 },
        { EGPSUpgradeType.Tier2, 1 },
        { EGPSUpgradeType.Tier3, 1 },
        { EGPSUpgradeType.Tier4, 1 },
        { EGPSUpgradeType.Tier5, 1 },
        { EGPSUpgradeType.Tier6, 1 },
        { EGPSUpgradeType.Tier7, 1 },
        { EGPSUpgradeType.Tier8, 1 },
        { EGPSUpgradeType.Tier9, 1 },
        { EGPSUpgradeType.Tier10, 1 }
    };
    public Dictionary<EGPSUpgradeType, int> GPSUpgrades => _gpsUpgrades;

    [SerializeField] private Dictionary<EProcessUpgradeType, int> _processUpgrades = new Dictionary<EProcessUpgradeType, int>
    {
        { EProcessUpgradeType.Tier1, 1 },
        { EProcessUpgradeType.Tier2, 1 },
        { EProcessUpgradeType.Tier3, 1 },
        { EProcessUpgradeType.Tier4, 1 },
        { EProcessUpgradeType.Tier5, 1 },
        { EProcessUpgradeType.Tier6, 1 },
        { EProcessUpgradeType.Tier7, 1 },
        { EProcessUpgradeType.Tier8, 1 },
        { EProcessUpgradeType.Tier9, 1 },
        { EProcessUpgradeType.Tier10, 1 }
    };
    public Dictionary<EProcessUpgradeType, int> ProcessUpgrades => _processUpgrades;

    /// <summary>
    /// Gets all GPC upgrades.
    /// </summary>
    /// <returns>Dictionary of all GPC upgrades and their levels.</returns>
    public Dictionary<EGPCUpgradeType, int> GetAllGPCUpgrades() => new Dictionary<EGPCUpgradeType, int>(_gpcUpgrades);

    /// <summary>
    /// Gets all GPS upgrades.
    /// </summary>
    /// <returns>Dictionary of all GPS upgrades and their levels.</returns>
    public Dictionary<EGPSUpgradeType, int> GetAllGPSUpgrades() => new Dictionary<EGPSUpgradeType, int>(_gpsUpgrades);

    /// <summary>
    /// Gets all Process upgrades.
    /// </summary>
    /// <returns>Dictionary of all Process upgrades and their levels.</returns>
    public Dictionary<EProcessUpgradeType, int> GetAllProcessUpgrades() => new Dictionary<EProcessUpgradeType, int>(_processUpgrades);

    /// <summary>
    /// Ensures that upgrade collections and other data are initialized.
    /// Call this after loading save data to guarantee collections are not null
    /// and fire change events so subscribers update their UI.
    /// </summary>
    public void InitializeGameData()
    {
        if (_gpcUpgrades == null)
        {
            _gpcUpgrades = new Dictionary<EGPCUpgradeType, int>
            {
                { EGPCUpgradeType.Tier1, 1 },
                { EGPCUpgradeType.Tier2, 1 },
                { EGPCUpgradeType.Tier3, 1 },
                { EGPCUpgradeType.Tier4, 1 },
                { EGPCUpgradeType.Tier5, 1 },
                { EGPCUpgradeType.Tier6, 1 },
                { EGPCUpgradeType.Tier7, 1 },
                { EGPCUpgradeType.Tier8, 1 },
                { EGPCUpgradeType.Tier9, 1 },
                { EGPCUpgradeType.Tier10, 1 }
            };
        }

        if (_gpsUpgrades == null)
        {
            _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
            {
                { EGPSUpgradeType.Tier1, 1 },
                { EGPSUpgradeType.Tier2, 1 },
                { EGPSUpgradeType.Tier3, 1 },
                { EGPSUpgradeType.Tier4, 1 },
                { EGPSUpgradeType.Tier5, 1 },
                { EGPSUpgradeType.Tier6, 1 },
                { EGPSUpgradeType.Tier7, 1 },
                { EGPSUpgradeType.Tier8, 1 },
                { EGPSUpgradeType.Tier9, 1 },
                { EGPSUpgradeType.Tier10, 1 }
            };
        }

        if (_processUpgrades == null)
        {
            _processUpgrades = new Dictionary<EProcessUpgradeType, int>
            {
                { EProcessUpgradeType.Tier1, 1 },
                { EProcessUpgradeType.Tier2, 1 },
                { EProcessUpgradeType.Tier3, 1 },
                { EProcessUpgradeType.Tier4, 1 },
                { EProcessUpgradeType.Tier5, 1 },
                { EProcessUpgradeType.Tier6, 1 },
                { EProcessUpgradeType.Tier7, 1 },
                { EProcessUpgradeType.Tier8, 1 },
                { EProcessUpgradeType.Tier9, 1 },
                { EProcessUpgradeType.Tier10, 1 }
            };
        }

        // Fire change events so UI updates after initialization
        OnGoldChanged?.Invoke();
        OnGoldPerClickChanged?.Invoke();
        OnGoldPerSecondChanged?.Invoke();
        OnStageChanged?.Invoke();
        OnReviveCountChanged?.Invoke();
    }

    /// <summary>
    /// Resets all progress values to their default starting values.
    /// This is used during a revive: revive count increases but all other progress resets.
    /// </summary>
    public void ResetProgress()
    {
        // Reset primary progress values
        Gold = 0;
        GoldPerClick = 1; // default starting click value
        GoldPerSecond = 0;
        Stage = 1;

        // Reset upgrades to default levels (1)
        _gpcUpgrades = new Dictionary<EGPCUpgradeType, int>
        {
            { EGPCUpgradeType.Tier1, 1 },
            { EGPCUpgradeType.Tier2, 1 },
            { EGPCUpgradeType.Tier3, 1 },
            { EGPCUpgradeType.Tier4, 1 },
            { EGPCUpgradeType.Tier5, 1 },
            { EGPCUpgradeType.Tier6, 1 },
            { EGPCUpgradeType.Tier7, 1 },
            { EGPCUpgradeType.Tier8, 1 },
            { EGPCUpgradeType.Tier9, 1 },
            { EGPCUpgradeType.Tier10, 1 }
        };

        _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
        {
            { EGPSUpgradeType.Tier1, 1 },
            { EGPSUpgradeType.Tier2, 1 },
            { EGPSUpgradeType.Tier3, 1 },
            { EGPSUpgradeType.Tier4, 1 },
            { EGPSUpgradeType.Tier5, 1 },
            { EGPSUpgradeType.Tier6, 1 },
            { EGPSUpgradeType.Tier7, 1 },
            { EGPSUpgradeType.Tier8, 1 },
            { EGPSUpgradeType.Tier9, 1 },
            { EGPSUpgradeType.Tier10, 1 }
        };

        _processUpgrades = new Dictionary<EProcessUpgradeType, int>
        {
            { EProcessUpgradeType.Tier1, 1 },
            { EProcessUpgradeType.Tier2, 1 },
            { EProcessUpgradeType.Tier3, 1 },
            { EProcessUpgradeType.Tier4, 1 },
            { EProcessUpgradeType.Tier5, 1 },
            { EProcessUpgradeType.Tier6, 1 },
            { EProcessUpgradeType.Tier7, 1 },
            { EProcessUpgradeType.Tier8, 1 },
            { EProcessUpgradeType.Tier9, 1 },
            { EProcessUpgradeType.Tier10, 1 }
        };

        // Reset GPCItemData levels
        if (GPCItemDataManager.Instance != null)
        {
            GPCItemDataManager.Instance.ResetAllLevels();
        }

        // Fire upgrade-change events indirectly by invoking the primary events
        OnGoldChanged?.Invoke();
        OnGoldPerClickChanged?.Invoke();
        OnGoldPerSecondChanged?.Invoke();
        OnStageChanged?.Invoke();
    }

    /// <summary>
    /// Performs a revive: increments revive count and resets all progress.
    /// </summary>
    public void Revive()
    {
        ReviveCount = ReviveCount + 1;
        ResetProgress();
        // Notify revive changed already via property setter
    }

    /// <summary>
    /// Loads/sets revive count from external source (e.g., SaveManager).
    /// This sets the internal revive counter directly and fires the change event.
    /// </summary>
    /// <param name="count">Revive count to set.</param>
    public void LoadReviveCount(int count)
    {
        // Set backing field directly to avoid increment logic in Revive()
        if (_reviveCount != count)
        {
            _reviveCount = count;
            OnReviveCountChanged?.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // Additional initialization logic if needed
    }

    protected virtual void Start()
    {
        // Initialization logic for GameManager
    }

    protected virtual void Update()
    {
        // Per-frame logic for GameManager
    }
}
