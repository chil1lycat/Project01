// 2025-11-04 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// LocalizationManager manages multi-language text using CSV files.
/// Singleton pattern. Loads CSV from Resources/Localization folder.
/// Supports language switching at runtime with observer pattern.
/// </summary>
public class LocalizationManager : Singleton<LocalizationManager>
{
    // Current language
    [SerializeField] private ELanguage _currentLanguage = ELanguage.Korean;
    public ELanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                OnLanguageChanged?.Invoke();
            }
        }
    }
    public event Action OnLanguageChanged;

    // Localization data: Key -> (Language -> Text)
    private Dictionary<string, Dictionary<ELanguage, string>> _localizationData = new Dictionary<string, Dictionary<ELanguage, string>>();

    // CSV file name in Resources/Localization/
    [SerializeField] private string _csvFileName = "LocalizationData";

    protected override void Awake()
    {
        base.Awake();
        LoadLocalizationData();
    }

    /// <summary>
    /// Load CSV file from Resources/Localization folder and parse it.
    /// </summary>
    private void LoadLocalizationData()
    {
        _localizationData.Clear();

        // Load CSV from Resources
        TextAsset csvFile = Resources.Load<TextAsset>($"Localization/{_csvFileName}");
        if (csvFile == null)
        {
            Debug.LogWarning($"LocalizationManager: CSV file not found at Resources/Localization/{_csvFileName}.csv");
            return;
        }

        ParseCSV(csvFile.text);
        Debug.Log($"LocalizationManager: Loaded {_localizationData.Count} localization keys.");
    }

    /// <summary>
    /// Parse CSV text and build localization dictionary.
    /// CSV format: Key,Korean,English,Japanese
    /// </summary>
    private void ParseCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogWarning("LocalizationManager: CSV file is empty or has no data rows.");
            return;
        }

        // First line is header: Key,Korean,English,Japanese
        string[] headers = lines[0].Split(',');
        Dictionary<int, ELanguage> columnToLanguage = new Dictionary<int, ELanguage>();

        // Map column index to language
        for (int i = 1; i < headers.Length; i++)
        {
            string header = headers[i].Trim().Replace("\r", "");
            if (Enum.TryParse<ELanguage>(header, true, out ELanguage lang))
            {
                columnToLanguage[i] = lang;
            }
        }

        // Parse data rows
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 2) continue;

            string key = values[0].Trim();
            if (string.IsNullOrEmpty(key)) continue;

            Dictionary<ELanguage, string> translations = new Dictionary<ELanguage, string>();

            for (int col = 1; col < values.Length; col++)
            {
                if (columnToLanguage.ContainsKey(col))
                {
                    ELanguage lang = columnToLanguage[col];
                    string text = values[col].Trim().Replace("\r", "");
                    translations[lang] = text;
                }
            }

            _localizationData[key] = translations;
        }
    }

    /// <summary>
    /// Get localized text by key using current language.
    /// Returns key if not found.
    /// </summary>
    public string GetText(string key)
    {
        return GetText(key, _currentLanguage);
    }

    /// <summary>
    /// Get localized text by key and specific language.
    /// Returns key if not found.
    /// </summary>
    public string GetText(string key, ELanguage language)
    {
        if (string.IsNullOrEmpty(key)) return string.Empty;

        if (_localizationData.ContainsKey(key))
        {
            if (_localizationData[key].ContainsKey(language))
            {
                return _localizationData[key][language];
            }
            else
            {
                Debug.LogWarning($"LocalizationManager: Language '{language}' not found for key '{key}'");
                return key;
            }
        }
        else
        {
            Debug.LogWarning($"LocalizationManager: Key '{key}' not found in localization data.");
            return key;
        }
    }

    /// <summary>
    /// Check if a key exists in localization data.
    /// </summary>
    public bool HasKey(string key)
    {
        return _localizationData.ContainsKey(key);
    }

    /// <summary>
    /// Reload CSV file (useful for editor testing).
    /// </summary>
    public void ReloadLocalizationData()
    {
        LoadLocalizationData();
        OnLanguageChanged?.Invoke();
    }
}

/// <summary>
/// Supported languages enum. Add more languages as needed.
/// </summary>
public enum ELanguage
{
    Korean,
    English,
    Japanese
}
