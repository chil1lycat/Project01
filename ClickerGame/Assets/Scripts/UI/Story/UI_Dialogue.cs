// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI_Dialogue shows dialogue texts from a DialogueEvent one by one.
/// Inherits from UI_Base. Expects child components named "DialogueText" and "NextButton".
/// </summary>
public class UI_Dialogue : UI_Base
{
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Button _nextButton;

    private DialogueEvent _currentEvent;
    private int _currentIndex;

    protected override void Awake()
    {
        base.Awake();
        // Start inactive
        gameObject.SetActive(false);

        // Auto-find children
        _dialogueText = FindChildComponent<TMP_Text>("DialogueText");
        _nextButton = FindChildComponent<Button>("NextButton");
    }

    protected override void Start()
    {
        base.Start();
        BindUIEvents();
    }

    protected override void BindUIEvents()
    {
        if (_nextButton != null)
        {
            _nextButton.onClick.RemoveAllListeners();
            _nextButton.onClick.AddListener(OnNextClicked);
        }
    }

    /// <summary>
    /// Show the dialogue event starting from the first text.
    /// </summary>
    public void Show(DialogueEvent dialogueEvent)
    {
        if (dialogueEvent == null || dialogueEvent.DialogueTexts == null || dialogueEvent.DialogueTexts.Length == 0)
        {
            Debug.LogWarning("UI_Dialogue: invalid dialogue event");
            return;
        }

        _currentEvent = dialogueEvent;
        _currentIndex = 0;
        UpdateDialogueText();
        gameObject.SetActive(true);
    }

    private void OnNextClicked()
    {
        if (_currentEvent == null) return;

        _currentIndex++;
        if (_currentIndex >= _currentEvent.DialogueTexts.Length)
        {
            Close();
            return;
        }

        UpdateDialogueText();
    }

    private void UpdateDialogueText()
    {
        if (_dialogueText != null && _currentEvent != null && _currentEvent.DialogueTexts != null)
        {
            _dialogueText.text = _currentEvent.DialogueTexts[_currentIndex] ?? string.Empty;
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
        _currentEvent = null;
        _currentIndex = 0;
    }
}
