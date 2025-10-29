// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// GameManager is a Singleton class responsible for managing the game state and related data.
/// It provides public access to all variables through getters and setters, implements observer pattern
/// using event Actions to notify UI updates, and manages data using Lists and Dictionaries.
/// </summary>

public enum EGPCUpgradeType { A, B, C, D, E }
public enum EGPSUpgradeType { A, B, C, D, E }
public enum EProcessUpgradeType { A, B, C, D, E }

public class GameManager : Singleton<GameManager>
{
    // Gold-related variables
    [SerializeField] private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold != value)
            {
                _gold = value;
                OnGoldChanged?.Invoke();
            }
        }
    }
    public event Action OnGoldChanged;

    [SerializeField] private int _goldPerClick;
    public int GoldPerClick
    {
        get => _goldPerClick;
        set
        {
            if (_goldPerClick != value)
            {
                _goldPerClick = value;
                OnGoldPerClickChanged?.Invoke();
            }
        }
    }
    public event Action OnGoldPerClickChanged;

    [SerializeField] private int _goldPerSecond;
    public int GoldPerSecond
    {
        get => _goldPerSecond;
        set
        {
            if (_goldPerSecond != value)
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

    // Upgrade-related variables
    [SerializeField] private Dictionary<EGPCUpgradeType, int> _gpcUpgrades = new Dictionary<EGPCUpgradeType, int>
    {
        { EGPCUpgradeType.A, 1 },
        { EGPCUpgradeType.B, 1 },
        { EGPCUpgradeType.C, 1 },
        { EGPCUpgradeType.D, 1 },
        { EGPCUpgradeType.E, 1 }
    };
    public Dictionary<EGPCUpgradeType, int> GPCUpgrades => _gpcUpgrades;

    [SerializeField] private Dictionary<EGPSUpgradeType, int> _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
    {
        { EGPSUpgradeType.A, 1 },
        { EGPSUpgradeType.B, 1 },
        { EGPSUpgradeType.C, 1 },
        { EGPSUpgradeType.D, 1 },
        { EGPSUpgradeType.E, 1 }
    };
    public Dictionary<EGPSUpgradeType, int> GPSUpgrades => _gpsUpgrades;

    [SerializeField] private Dictionary<EProcessUpgradeType, int> _processUpgrades = new Dictionary<EProcessUpgradeType, int>
    {
        { EProcessUpgradeType.A, 1 },
        { EProcessUpgradeType.B, 1 },
        { EProcessUpgradeType.C, 1 },
        { EProcessUpgradeType.D, 1 },
        { EProcessUpgradeType.E, 1 }
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
                { EGPCUpgradeType.A, 1 },
                { EGPCUpgradeType.B, 1 },
                { EGPCUpgradeType.C, 1 },
                { EGPCUpgradeType.D, 1 },
                { EGPCUpgradeType.E, 1 }
            };
        }

        if (_gpsUpgrades == null)
        {
            _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
            {
                { EGPSUpgradeType.A, 1 },
                { EGPSUpgradeType.B, 1 },
                { EGPSUpgradeType.C, 1 },
                { EGPSUpgradeType.D, 1 },
                { EGPSUpgradeType.E, 1 }
            };
        }

        if (_processUpgrades == null)
        {
            _processUpgrades = new Dictionary<EProcessUpgradeType, int>
            {
                { EProcessUpgradeType.A, 1 },
                { EProcessUpgradeType.B, 1 },
                { EProcessUpgradeType.C, 1 },
                { EProcessUpgradeType.D, 1 },
                { EProcessUpgradeType.E, 1 }
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
            { EGPCUpgradeType.A, 1 },
            { EGPCUpgradeType.B, 1 },
            { EGPCUpgradeType.C, 1 },
            { EGPCUpgradeType.D, 1 },
            { EGPCUpgradeType.E, 1 }
        };

        _gpsUpgrades = new Dictionary<EGPSUpgradeType, int>
        {
            { EGPSUpgradeType.A, 1 },
            { EGPSUpgradeType.B, 1 },
            { EGPSUpgradeType.C, 1 },
            { EGPSUpgradeType.D, 1 },
            { EGPSUpgradeType.E, 1 }
        };

        _processUpgrades = new Dictionary<EProcessUpgradeType, int>
        {
            { EProcessUpgradeType.A, 1 },
            { EProcessUpgradeType.B, 1 },
            { EProcessUpgradeType.C, 1 },
            { EProcessUpgradeType.D, 1 },
            { EProcessUpgradeType.E, 1 }
        };

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
