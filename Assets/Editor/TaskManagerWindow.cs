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

    private Task selectedTask;  // 選択されたタスクを保持

    // 優先度の選択肢
    private string[] priorityOptions = { "低", "中", "高" };
    private int selectedPriorityIndex = 0;  // 初期は「低」

    // 難易度の選択肢
    private string[] difficultyOptions = { "簡単", "中", "難しい" };
    private int selectedDifficultyIndex = 0;  // 初期は「簡単」

    // ソートオプション
    private enum SortBy { Priority, Difficulty }  // 並べ替え基準の列挙体
    private SortBy selectedSortBy = SortBy.Priority;  // 初期は優先度でソート
    private bool sortDescending = false;  // 昇順ならfalse, 降順ならtrue

    [MenuItem("Tools/Task Manager")]
    public static void ShowWindow()
    {
        GetWindow<TaskManagerWindow>("Task Manager");
    }

    private void OnEnable()
    {
        // TaskDataをプロジェクトからロードまたは新規作成
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

        // タスク追加フォーム
        GUILayout.Label("新しいタスクを追加");
        newTaskTitle = EditorGUILayout.TextField("タイトル", newTaskTitle);
        newTaskDescription = EditorGUILayout.TextField("説明", newTaskDescription);
        selectedCategoryIndex = EditorGUILayout.Popup("カテゴリー", selectedCategoryIndex, categories);

        // 優先度を選択するドロップダウンメニューを追加
        selectedPriorityIndex = EditorGUILayout.Popup("優先度", selectedPriorityIndex, priorityOptions);

        // 難易度を選択するドロップダウンメニューを追加
        selectedDifficultyIndex = EditorGUILayout.Popup("難易度", selectedDifficultyIndex, difficultyOptions);

        if (GUILayout.Button("タスクを追加"))
        {
            AddTask();
        }

        GUILayout.Space(10);

        // フィルタセクション
        GUILayout.Label("フィルタ", EditorStyles.boldLabel);
        string[] filterOptions = new[] { "すべて" }.Concat(categories).ToArray();
        int selectedFilterIndex = EditorGUILayout.Popup("カテゴリー", Array.IndexOf(filterOptions, filterCategory), filterOptions);
        filterCategory = filterOptions[selectedFilterIndex];

        GUILayout.Space(10);

        // ソートオプション
        GUILayout.Label("ソート", EditorStyles.boldLabel);
        selectedSortBy = (SortBy)EditorGUILayout.EnumPopup("ソート基準", selectedSortBy);  // 優先度と難易度の選択肢
        sortDescending = EditorGUILayout.Toggle("降順でソート", sortDescending);

        GUILayout.Space(10);

        // タスクリスト表示
        GUILayout.Label("タスクリスト", EditorStyles.boldLabel);

        // 表頭 (ヘッダー) 追加
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("完了", GUILayout.Width(50));
        GUILayout.Label("タイトル", GUILayout.Width(150));
        GUILayout.Label("カテゴリー", GUILayout.Width(100));
        GUILayout.Label("優先度", GUILayout.Width(60));  // 優先度
        GUILayout.Label("難易度", GUILayout.Width(80));  // 難易度
        GUILayout.Label("操作", GUILayout.Width(120));  // 操作 (選択、削除)
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // フィルタ適用
        var filteredTasks = filterCategory == "すべて"
            ? taskData.Tasks
            : taskData.Tasks.Where(task => task.Category == filterCategory).ToList();

        // ソートの適用
        if (selectedSortBy == SortBy.Priority)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Priority).ToList()  // 優先度で降順
                : filteredTasks.OrderBy(task => task.Priority).ToList();  // 優先度で昇順
        }
        else if (selectedSortBy == SortBy.Difficulty)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Difficulty).ToList()  // 難易度で降順
                : filteredTasks.OrderBy(task => task.Difficulty).ToList();  // 難易度で昇順
        }

        // タスクリスト表示
        foreach (var task in filteredTasks)
        {
            EditorGUILayout.BeginHorizontal();

            // 完了状態を表示
            task.IsCompleted = EditorGUILayout.Toggle(task.IsCompleted, GUILayout.Width(50));

            // タスクのタイトル、カテゴリー、優先度、難易度
            GUILayout.Label(task.Title, GUILayout.Width(150));
            GUILayout.Label(task.Category, GUILayout.Width(100));
            GUILayout.Label(task.Priority.ToString(), GUILayout.Width(60));  // 優先度
            GUILayout.Label(task.Difficulty.ToString(), GUILayout.Width(80));  // 難易度

            // 操作ボタン（選択、削除）
            if (GUILayout.Button("選択", GUILayout.Width(60)))
            {
                selectedTask = task;  // タスクを選択
            }

            if (GUILayout.Button("削除", GUILayout.Width(60)))
            {
                RemoveTask(task);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // 選択したタスクの詳細を表示
        if (selectedTask != null)
        {
            GUILayout.Label("選択されたタスクの詳細", EditorStyles.boldLabel);
            GUILayout.Label("タイトル: " + selectedTask.Title);
            GUILayout.Label("カテゴリー: " + selectedTask.Category);
            GUILayout.Label("説明: " + selectedTask.Description);
            GUILayout.Label("優先度: " + selectedTask.Priority.ToString());  // 優先度の表示
            GUILayout.Label("難易度: " + selectedTask.Difficulty.ToString());  // 難易度の表示

            if (GUILayout.Button("タスクの削除"))
            {
                RemoveTask(selectedTask);
                selectedTask = null;  // 削除後に選択を解除
            }
        }
    }

    private void AddTask()
    {
        if (string.IsNullOrEmpty(newTaskTitle)) return;

        Task newTask = new Task
        {
            Title = newTaskTitle,
            Description = newTaskDescription,
            Category = categories[selectedCategoryIndex],
            Priority = (PriorityLevel)selectedPriorityIndex,  // 優先度を設定
            Difficulty = (DifficultyLevel)selectedDifficultyIndex,  // 難易度を設定
            IsCompleted = false
        };

        taskData.Tasks.Add(newTask);
        newTaskTitle = "";
        newTaskDescription = "";

        EditorUtility.SetDirty(taskData);
    }

    private void RemoveTask(Task task)
    {
        taskData.Tasks.Remove(task);
        EditorUtility.SetDirty(taskData);
    }
}
