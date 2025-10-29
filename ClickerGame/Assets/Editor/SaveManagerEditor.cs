#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SaveManagerEditor
{
    [MenuItem("Tools/SaveManager/Delete Save File")] 
    private static void DeleteSaveFile()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "save_game.json");
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            Debug.Log($"Deleted save file at {path}");
        }
        else
        {
            Debug.Log($"Save file not found at {path}");
        }
    }
}
#endif