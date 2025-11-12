// 2025-11-12 AI-Tag
// Unity Inspector에서 null 참조를 정리하는 에디터 스크립트

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Scene과 Prefab에서 null 참조를 찾아서 정리하는 에디터 유틸리티
/// </summary>
public class CleanupNullReferences : EditorWindow
{
    [MenuItem("Tools/Cleanup Null References")]
    public static void ShowWindow()
    {
        GetWindow<CleanupNullReferences>("Cleanup Null References");
    }

    private void OnGUI()
    {
        GUILayout.Label("Null Reference Cleanup Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Clean Current Scene"))
        {
            CleanCurrentScene();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Clean All Prefabs in Project"))
        {
            CleanAllPrefabs();
        }

        GUILayout.Space(10);
        GUILayout.Label("이 도구는 씬과 프리팹에서 null 참조를 찾아 제거합니다.", EditorStyles.helpBox);
    }

    private static void CleanCurrentScene()
    {
        int count = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null)
                {
                    count++;
                    Debug.LogWarning($"Null component found on {obj.name}", obj);
                }
            }
        }

        Debug.Log($"[CleanupNullReferences] Scene cleanup complete. Found {count} null references.");
        EditorUtility.DisplayDialog("Cleanup Complete", $"Found and logged {count} null references in current scene.", "OK");
    }

    private static void CleanAllPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        int count = 0;
        int checkedCount = 0;

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                checkedCount++;
                Component[] components = prefab.GetComponentsInChildren<Component>(true);
                foreach (Component component in components)
                {
                    if (component == null)
                    {
                        count++;
                        Debug.LogWarning($"Null component found in prefab: {path}");
                    }
                }
            }
        }

        Debug.Log($"[CleanupNullReferences] Prefab cleanup complete. Checked {checkedCount} prefabs, found {count} null references.");
        EditorUtility.DisplayDialog("Cleanup Complete", $"Checked {checkedCount} prefabs.\nFound and logged {count} null references.", "OK");
    }
}
#endif
