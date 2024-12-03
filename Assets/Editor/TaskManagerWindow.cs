using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

public class TaskManagerWindow : EditorWindow
{
    private TaskData taskData;
    private Vector2 scrollPosition;
    private string newTaskTitle = "";
    private string newTaskDescription = "";
    private string[] categories = { "バグ修正", "機能実装", "その他" };
    private int selectedCategoryIndex = 0;
    private string filterCategory = "すべて";

    private Task selectedTask;

    private string[] priorityOptions = { "低", "中", "高" };
    private int selectedPriorityIndex = 0;

    private string[] difficultyOptions = { "簡単", "中", "難しい" };
    private int selectedDifficultyIndex = 0;

    private enum SortBy { Priority, Difficulty }
    private SortBy selectedSortBy = SortBy.Priority;
    private bool sortDescending = false;

    [MenuItem("Tools/Task Manager")]
    public static void ShowWindow()
    {
        GetWindow<TaskManagerWindow>("Task Manager");
    }

    private void OnEnable()
    {
        taskData = AssetDatabase.LoadAssetAtPath<TaskData>("Assets/TaskData.asset");
        if (taskData == null)
        {
            taskData = CreateInstance<TaskData>();
            AssetDatabase.CreateAsset(taskData, "Assets/TaskData.asset");
            AssetDatabase.SaveAssets();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("タスク管理ツール", EditorStyles.boldLabel);

        DrawTaskAdditionSection();
        GUILayout.Space(10);

        DrawFilterSection();
        GUILayout.Space(10);

        DrawSortSection();
        GUILayout.Space(10);

        DrawTaskList();

        GUILayout.Space(10);

        DrawSelectedTaskDetails();
    }

    private void DrawTaskAdditionSection()
    {
        GUILayout.Label("新しいタスクを追加");
        newTaskTitle = EditorGUILayout.TextField("タイトル", newTaskTitle);
        newTaskDescription = EditorGUILayout.TextField("説明", newTaskDescription);
        selectedCategoryIndex = EditorGUILayout.Popup("カテゴリー", selectedCategoryIndex, categories);
        selectedPriorityIndex = EditorGUILayout.Popup("優先度", selectedPriorityIndex, priorityOptions);
        selectedDifficultyIndex = EditorGUILayout.Popup("難易度", selectedDifficultyIndex, difficultyOptions);

        if (GUILayout.Button("タスクを追加"))
        {
            AddTask();
        }
    }

    private void DrawFilterSection()
    {
        GUILayout.Label("フィルタ", EditorStyles.boldLabel);
        string[] filterOptions = new[] { "すべて" }.Concat(categories).ToArray();
        int selectedFilterIndex = Array.IndexOf(filterOptions, filterCategory);
        selectedFilterIndex = EditorGUILayout.Popup("カテゴリー", selectedFilterIndex, filterOptions);
        filterCategory = filterOptions[Mathf.Clamp(selectedFilterIndex, 0, filterOptions.Length - 1)];
    }

    private void DrawSortSection()
    {
        GUILayout.Label("ソート", EditorStyles.boldLabel);
        selectedSortBy = (SortBy)EditorGUILayout.EnumPopup("ソート基準", selectedSortBy);
        sortDescending = EditorGUILayout.Toggle("降順でソート", sortDescending);
    }

    private void DrawTaskList()
    {
        GUILayout.Label("タスクリスト", EditorStyles.boldLabel);

        // ヘッダーの作成
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("タイトル", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("カテゴリー", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("優先度", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.Label("難易度", EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.FlexibleSpace(); // スペースを追加して調整
        GUILayout.Label("操作", EditorStyles.boldLabel, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();

        // スクロールビューの開始
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

        var filteredTasks = GetFilteredAndSortedTasks();

        foreach (var task in filteredTasks)
        {
            if (task == null || taskData == null) continue; // 不正なデータを除外

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(task.Title, GUILayout.Width(200));
            GUILayout.Label(task.Category, GUILayout.Width(150));
            GUILayout.Label(task.Priority.ToString(), GUILayout.Width(80));
            GUILayout.Label(task.Difficulty.ToString(), GUILayout.Width(100));
            GUILayout.FlexibleSpace(); // スペースを追加して調整

            if (GUILayout.Button("選択", GUILayout.Width(70)))
            {
                selectedTask = task;
            }

            if (GUILayout.Button("削除", GUILayout.Width(70)))
            {
                RemoveTask(task);
                break; // リスト変更後は再描画が必要
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }


    private void DrawSelectedTaskDetails()
    {
        if (selectedTask != null)
        {
            GUILayout.Label("選択されたタスクの詳細", EditorStyles.boldLabel);
            GUILayout.Label("タイトル: " + selectedTask.Title);
            GUILayout.Label("カテゴリー: " + selectedTask.Category);
            GUILayout.Label("説明: " + selectedTask.Description);
            GUILayout.Label("優先度: " + selectedTask.Priority.ToString());
            GUILayout.Label("難易度: " + selectedTask.Difficulty.ToString());

            if (GUILayout.Button("タスクの削除"))
            {
                RemoveTask(selectedTask);
                selectedTask = null;
            }
        }
    }

    private void AddTask()
    {
        if (string.IsNullOrEmpty(newTaskTitle)) return;

        var newTask = new Task
        {
            Title = newTaskTitle,
            Description = newTaskDescription,
            Category = categories[selectedCategoryIndex],
            Priority = (PriorityLevel)selectedPriorityIndex,
            Difficulty = (DifficultyLevel)selectedDifficultyIndex
        };

        taskData.Tasks.Add(newTask);
        newTaskTitle = "";
        newTaskDescription = "";

        EditorUtility.SetDirty(taskData);
        AssetDatabase.SaveAssets(); // データを確実に保存
    }

    private void RemoveTask(Task task)
    {
        if (taskData.Tasks.Contains(task))
        {
            taskData.Tasks.Remove(task);
            EditorUtility.SetDirty(taskData);
            AssetDatabase.SaveAssets(); // 削除後にデータを保存
        }
    }

    private System.Collections.Generic.List<Task> GetFilteredAndSortedTasks()
    {
        if (taskData == null || taskData.Tasks == null)
            return new System.Collections.Generic.List<Task>();

        var filteredTasks = filterCategory == "すべて"
            ? taskData.Tasks
            : taskData.Tasks.Where(task => task.Category == filterCategory).ToList();

        if (selectedSortBy == SortBy.Priority)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Priority).ToList()
                : filteredTasks.OrderBy(task => task.Priority).ToList();
        }
        else if (selectedSortBy == SortBy.Difficulty)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Difficulty).ToList()
                : filteredTasks.OrderBy(task => task.Difficulty).ToList();
        }

        return filteredTasks;
    }
}
