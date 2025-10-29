// 2025-10-26 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;

/// <summary>
/// DialogueEvent represents a story event that triggers when conditions are met.
/// It is serializable and intended to be configured in the inspector via StoryManager.
/// </summary>
[Serializable]
public class DialogueEvent
{
    // Unique identifier for the event
    public int EventId;

    // Trigger conditions (>=). Set 0 to ignore.
    public int RequiredGold = 0;
    public int RequiredStage = 0;

    // Texts to show in sequence when event triggers
    public string[] DialogueTexts;

    /// <summary>
    /// Returns true if current GameManager state satisfies the event condition.
    /// </summary>
    public bool IsConditionMet()
    {
        var gm = GameManager.Instance;
        if (gm == null) return false;

        if (RequiredGold > 0 && gm.Gold < RequiredGold) return false;
        if (RequiredStage > 0 && gm.Stage < RequiredStage) return false;

        return true;
    }
}
