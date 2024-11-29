using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptAttachmentFinder : EditorWindow
{
    private MonoScript targetScript; // 検索対象のスクリプト
    private List<Object> foundObjects = new List<Object>(); // 検索で見つかったオブジェクト
    private Vector2 scrollPosition;

    [MenuItem("Tools/Script Attachment Finder")]
    public static void ShowWindow()
    {
        GetWindow<ScriptAttachmentFinder>("Script Attachment Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Search for Objects with Script Attached", EditorStyles.boldLabel);

        // スクリプトの選択
        targetScript = (MonoScript)EditorGUILayout.ObjectField("Target Script", targetScript, typeof(MonoScript), false);

        // 検索ボタン
        if (GUILayout.Button("Find Attached Objects"))
        {
            FindAttachedObjects();
        }

        // 結果の表示
        if (foundObjects.Count > 0)
        {
            GUILayout.Label($"Found {foundObjects.Count} objects:", EditorStyles.boldLabel);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
            foreach (var obj in foundObjects)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(obj.name, GUILayout.Width(300));

                // Pingボタン
                if (GUILayout.Button("Ping", GUILayout.Width(50)))
                {
                    EditorGUIUtility.PingObject(obj);
                }

                // Focusボタン
                if (GUILayout.Button("Focus", GUILayout.Width(50)))
                {
                    FocusOnObject(obj);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
    }

    private void FindAttachedObjects()
    {
        if (targetScript == null)
        {
            Debug.LogWarning("Please select a script to search for.");
            return;
        }

        foundObjects.Clear();

        // 対象スクリプトの型を取得
        var targetType = targetScript.GetClass();
        if (targetType == null)
        {
            Debug.LogWarning("The selected script does not define a valid MonoBehaviour class.");
            return;
        }

        // プロジェクト内のすべてのプレハブを検索
        string[] prefabPaths = AssetDatabase.GetAllAssetPaths();
        foreach (string path in prefabPaths)
        {
            if (path.EndsWith(".prefab"))
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null && HasScriptAttached(prefab, targetType))
                {
                    foundObjects.Add(prefab);
                }
            }
        }

        // シーン内のオブジェクトを検索
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (HasScriptAttached(obj, targetType))
            {
                foundObjects.Add(obj);
            }
        }

        Debug.Log($"Search completed. Found {foundObjects.Count} objects with {targetType.Name} attached.");
    }

    private bool HasScriptAttached(GameObject obj, System.Type targetType)
    {
        // GameObjectに指定されたスクリプトがアタッチされているか確認
        var components = obj.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            if (component != null && component.GetType() == targetType)
            {
                return true;
            }
        }
        return false;
    }

    private void FocusOnObject(Object obj)
    {
        if (obj is GameObject go)
        {
            Selection.activeGameObject = go; // 選択

            // Boundsの作成
            Bounds bounds = new Bounds(go.transform.position, Vector3.one); // オブジェクトの位置を中心とした小さなBounds

            // シーンビューでフォーカス
            SceneView.lastActiveSceneView?.Frame(bounds, true);
        }
        else
        {
            Debug.LogWarning("Focus is only supported for GameObjects.");
        }
    }
}
