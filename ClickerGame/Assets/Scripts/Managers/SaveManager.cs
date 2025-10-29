// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SaveManager is a Singleton responsible for saving and loading game data periodically.
/// It saves GameManager's serializable data to a JSON file every SaveInterval seconds and
/// loads the data on startup to restore game state.
/// </summary>
public class SaveManager : Singleton<SaveManager>
{
    [SerializeField] private float _saveInterval = 10f;
    public float SaveInterval { get => _saveInterval; set => _saveInterval = value; }

    private string _saveFilePath => Path.Combine(Application.persistentDataPath, "save_game.json");

    protected override void Awake()
    {
        base.Awake();
		Load();
	}

    protected virtual void Start()
	{
        StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_saveInterval);
            Save();
        }
    }

    [Serializable]
    private class SaveData
    {
        public int Gold;
        public int GoldPerClick;
        public int GoldPerSecond;
        public int Stage;
        public int ReviveCount;
        public Dictionary<EGPCUpgradeType, int> GPCUpgrades;
        public Dictionary<EGPSUpgradeType, int> GPSUpgrades;
        public Dictionary<EProcessUpgradeType, int> ProcessUpgrades;

        // Added: store StoryManager triggered event IDs
        public List<int> TriggeredEventIds;
    }

    public void Save()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("SaveManager: GameManager.Instance is null, skipping save.");
            return;
        }

        var data = new SaveData
        {
            Gold = GameManager.Instance.Gold,
            GoldPerClick = GameManager.Instance.GoldPerClick,
            GoldPerSecond = GameManager.Instance.GoldPerSecond,
            Stage = GameManager.Instance.Stage,
            ReviveCount = GameManager.Instance.ReviveCount,
            GPCUpgrades = new Dictionary<EGPCUpgradeType, int>(GameManager.Instance.GPCUpgrades),
            GPSUpgrades = new Dictionary<EGPSUpgradeType, int>(GameManager.Instance.GPSUpgrades),
            ProcessUpgrades = new Dictionary<EProcessUpgradeType, int>(GameManager.Instance.ProcessUpgrades),

            // Save StoryManager triggered events if available
            TriggeredEventIds = StoryManager.Instance != null ? StoryManager.Instance.GetTriggeredEventIds() : new List<int>()
        };

        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json, Encoding.UTF8);
#if UNITY_EDITOR
            UnityEngine.Debug.Log($"Game saved to {_saveFilePath}");
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: Failed to save game - {ex.Message}");
        }
    }

    public void Load()
    {
        if (!File.Exists(_saveFilePath))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("SaveManager: No save file found, initializing default data.");
#endif
            GameManager.Instance.InitializeGameData();
            return;
        }

        try
        {
            string json = File.ReadAllText(_saveFilePath, Encoding.UTF8);
            var data = JsonUtility.FromJson<SaveData>(json);
            if (data == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("SaveManager: Failed to parse save data, initializing default data.");
#endif
                GameManager.Instance.InitializeGameData();
                return;
            }

            // Apply StoryManager triggered events immediately so StoryManager has data as early as possible
            if (data.TriggeredEventIds != null)
            {
                // Prefer the Singleton instance; fall back to FindObjectOfType if needed
                try
                {
                    if (StoryManager.Instance != null)
                    {
                        StoryManager.Instance.LoadTriggeredEventIds(data.TriggeredEventIds);
                    }
                    else
                    {
                        var found = UnityEngine.Object.FindObjectOfType<StoryManager>(true);
                        if (found != null)
                        {
                            found.LoadTriggeredEventIds(data.TriggeredEventIds);
                        }
                        else
                        {
#if UNITY_EDITOR
                            UnityEngine.Debug.LogWarning("SaveManager: StoryManager not present to load triggered events.");
#endif
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"SaveManager: Failed to apply StoryManager triggered events immediately - {ex.Message}");
                }
            }

            // Apply loaded values to GameManager
            GameManager.Instance.Gold = data.Gold;
            GameManager.Instance.GoldPerClick = data.GoldPerClick;
            GameManager.Instance.GoldPerSecond = data.GoldPerSecond;
            GameManager.Instance.Stage = data.Stage;
            GameManager.Instance.LoadReviveCount(data.ReviveCount);

            // Dictionaries may be null when deserialized by JsonUtility, handle defensively
            if (data.GPCUpgrades != null)
            {
                // Clear and copy
                GameManager.Instance.GPCUpgrades.Clear();
                foreach (var kv in data.GPCUpgrades) GameManager.Instance.GPCUpgrades[kv.Key] = kv.Value;
            }

            if (data.GPSUpgrades != null)
            {
                GameManager.Instance.GPSUpgrades.Clear();
                foreach (var kv in data.GPSUpgrades) GameManager.Instance.GPSUpgrades[kv.Key] = kv.Value;
            }

            if (data.ProcessUpgrades != null)
            {
                GameManager.Instance.ProcessUpgrades.Clear();
                foreach (var kv in data.ProcessUpgrades) GameManager.Instance.ProcessUpgrades[kv.Key] = kv.Value;
            }

            GameManager.Instance.InitializeGameData();

#if UNITY_EDITOR
            UnityEngine.Debug.Log($"SaveManager: Loaded save from {_saveFilePath}");
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError($"SaveManager: Failed to load save - {ex.Message}");
            GameManager.Instance.InitializeGameData();
        }
    }
}
