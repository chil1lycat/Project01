// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// StoryManager inspects DialogueEvents and triggers UI_Dialogue when conditions are met.
/// Inherits from Singleton<StoryManager>.
/// Configure DialogueEvents in the inspector.
/// </summary>
public class StoryManager : Singleton<StoryManager>
{
    public List<DialogueEvent> DialogueEvents = new List<DialogueEvent>();

    // Keep track of events already triggered
    private HashSet<int> _triggered = new HashSet<int>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        // SaveManager now applies StoryManager triggered IDs immediately during Load, so no pending consumption here.

        Subscribe();
        CheckAllEvents();
    }

    private void Subscribe()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGoldChanged += CheckAllEvents;
        GameManager.Instance.OnStageChanged += CheckAllEvents;
    }

    private void Unsubscribe()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGoldChanged -= CheckAllEvents;
        GameManager.Instance.OnStageChanged -= CheckAllEvents;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void CheckAllEvents()
    {
        if (DialogueEvents == null || DialogueEvents.Count == 0) return;

        foreach (var evt in DialogueEvents)
        {
            if (evt == null) continue;
            if (_triggered.Contains(evt.EventId)) continue;
            if (evt.IsConditionMet())
            {
                Trigger(evt);
                _triggered.Add(evt.EventId);
                // Only trigger first matching event; continue if you want multiple at once
            }
        }
    }

    private void Trigger(DialogueEvent evt)
    {
        var ui = FindAnyObjectByType<UI_Dialogue>(FindObjectsInactive.Include);
        if (ui != null)
        {
            ui.Show(evt);
        }
        else
        {
            Debug.LogWarning("StoryManager: UI_Dialogue not found in scene.");
        }
    }

    // Added: expose triggered event IDs so they can be saved/loaded by SaveManager
    public List<int> GetTriggeredEventIds()
    {
        return new List<int>(_triggered);
    }

    public void LoadTriggeredEventIds(IEnumerable<int> ids)
    {
        _triggered.Clear();
        if (ids == null) return;
        foreach (var id in ids) _triggered.Add(id);
    }
}
