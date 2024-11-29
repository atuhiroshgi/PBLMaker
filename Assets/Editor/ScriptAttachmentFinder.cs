using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptAttachmentFinder : EditorWindow
{
    private MonoScript targetScript; // �����Ώۂ̃X�N���v�g
    private List<Object> foundObjects = new List<Object>(); // �����Ō��������I�u�W�F�N�g
    private Vector2 scrollPosition;

    [MenuItem("Tools/Script Attachment Finder")]
    public static void ShowWindow()
    {
        GetWindow<ScriptAttachmentFinder>("Script Attachment Finder");
    }

    private void OnGUI()
    {
        GUILayout.Label("Search for Objects with Script Attached", EditorStyles.boldLabel);

        // �X�N���v�g�̑I��
        targetScript = (MonoScript)EditorGUILayout.ObjectField("Target Script", targetScript, typeof(MonoScript), false);

        // �����{�^��
        if (GUILayout.Button("Find Attached Objects"))
        {
            FindAttachedObjects();
        }

        // ���ʂ̕\��
        if (foundObjects.Count > 0)
        {
            GUILayout.Label($"Found {foundObjects.Count} objects:", EditorStyles.boldLabel);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
            foreach (var obj in foundObjects)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(obj.name, GUILayout.Width(300));

                // Ping�{�^��
                if (GUILayout.Button("Ping", GUILayout.Width(50)))
                {
                    EditorGUIUtility.PingObject(obj);
                }

                // Focus�{�^��
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

        // �ΏۃX�N���v�g�̌^���擾
        var targetType = targetScript.GetClass();
        if (targetType == null)
        {
            Debug.LogWarning("The selected script does not define a valid MonoBehaviour class.");
            return;
        }

        // �v���W�F�N�g���̂��ׂẴv���n�u������
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

        // �V�[�����̃I�u�W�F�N�g������
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
        // GameObject�Ɏw�肳�ꂽ�X�N���v�g���A�^�b�`����Ă��邩�m�F
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
            Selection.activeGameObject = go; // �I��

            // Bounds�̍쐬
            Bounds bounds = new Bounds(go.transform.position, Vector3.one); // �I�u�W�F�N�g�̈ʒu�𒆐S�Ƃ���������Bounds

            // �V�[���r���[�Ńt�H�[�J�X
            SceneView.lastActiveSceneView?.Frame(bounds, true);
        }
        else
        {
            Debug.LogWarning("Focus is only supported for GameObjects.");
        }
    }
}
