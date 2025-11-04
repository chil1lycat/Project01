// 2025-11-04 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using TMPro;

/// <summary>
/// LocalizedText component automatically updates TMP_Text with localized text.
/// Attach to GameObject with TMP_Text component.
/// Set localization key in inspector, and text will update automatically when language changes.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _localizationKey;
    public string LocalizationKey
    {
        get => _localizationKey;
        set
        {
            _localizationKey = value;
            UpdateText();
        }
    }

    private TMP_Text _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        // Subscribe to language change event
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        }

        // Initial update
        UpdateText();
    }

    private void OnDestroy()
    {
        // Unsubscribe from language change event
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
        }
    }

    /// <summary>
    /// Update text with localized value from LocalizationManager.
    /// </summary>
    private void UpdateText()
    {
        if (_textComponent == null) return;
        if (string.IsNullOrEmpty(_localizationKey)) return;
        if (LocalizationManager.Instance == null) return;

        string localizedText = LocalizationManager.Instance.GetText(_localizationKey);
        _textComponent.text = localizedText;
    }

    /// <summary>
    /// Set localization key and update text immediately.
    /// Useful for dynamic text changes.
    /// </summary>
    public void SetKey(string key)
    {
        LocalizationKey = key;
    }
}
