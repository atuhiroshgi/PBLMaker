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
    private string[] categories = { "�o�O�C��", "�@�\����", "���̑�" };
    private int selectedCategoryIndex = 0;
    private string filterCategory = "���ׂ�";

    private Task selectedTask;

    private string[] priorityOptions = { "��", "��", "��" };
    private int selectedPriorityIndex = 0;

    private string[] difficultyOptions = { "�ȒP", "��", "���" };
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
        GUILayout.Label("�^�X�N�Ǘ��c�[��", EditorStyles.boldLabel);

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
        GUILayout.Label("�V�����^�X�N��ǉ�");
        newTaskTitle = EditorGUILayout.TextField("�^�C�g��", newTaskTitle);
        newTaskDescription = EditorGUILayout.TextField("����", newTaskDescription);
        selectedCategoryIndex = EditorGUILayout.Popup("�J�e�S���[", selectedCategoryIndex, categories);
        selectedPriorityIndex = EditorGUILayout.Popup("�D��x", selectedPriorityIndex, priorityOptions);
        selectedDifficultyIndex = EditorGUILayout.Popup("��Փx", selectedDifficultyIndex, difficultyOptions);

        if (GUILayout.Button("�^�X�N��ǉ�"))
        {
            AddTask();
        }
    }

    private void DrawFilterSection()
    {
        GUILayout.Label("�t�B���^", EditorStyles.boldLabel);
        string[] filterOptions = new[] { "���ׂ�" }.Concat(categories).ToArray();
        int selectedFilterIndex = Array.IndexOf(filterOptions, filterCategory);
        selectedFilterIndex = EditorGUILayout.Popup("�J�e�S���[", selectedFilterIndex, filterOptions);
        filterCategory = filterOptions[Mathf.Clamp(selectedFilterIndex, 0, filterOptions.Length - 1)];
    }

    private void DrawSortSection()
    {
        GUILayout.Label("�\�[�g", EditorStyles.boldLabel);
        selectedSortBy = (SortBy)EditorGUILayout.EnumPopup("�\�[�g�", selectedSortBy);
        sortDescending = EditorGUILayout.Toggle("�~���Ń\�[�g", sortDescending);
    }

    private void DrawTaskList()
    {
        GUILayout.Label("�^�X�N���X�g", EditorStyles.boldLabel);

        // �w�b�_�[�̍쐬
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("�^�C�g��", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("�J�e�S���[", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("�D��x", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.Label("��Փx", EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.FlexibleSpace(); // �X�y�[�X��ǉ����Ē���
        GUILayout.Label("����", EditorStyles.boldLabel, GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();

        // �X�N���[���r���[�̊J�n
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

        var filteredTasks = GetFilteredAndSortedTasks();

        foreach (var task in filteredTasks)
        {
            if (task == null || taskData == null) continue; // �s���ȃf�[�^�����O

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(task.Title, GUILayout.Width(200));
            GUILayout.Label(task.Category, GUILayout.Width(150));
            GUILayout.Label(task.Priority.ToString(), GUILayout.Width(80));
            GUILayout.Label(task.Difficulty.ToString(), GUILayout.Width(100));
            GUILayout.FlexibleSpace(); // �X�y�[�X��ǉ����Ē���

            if (GUILayout.Button("�I��", GUILayout.Width(70)))
            {
                selectedTask = task;
            }

            if (GUILayout.Button("�폜", GUILayout.Width(70)))
            {
                RemoveTask(task);
                break; // ���X�g�ύX��͍ĕ`�悪�K�v
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }


    private void DrawSelectedTaskDetails()
    {
        if (selectedTask != null)
        {
            GUILayout.Label("�I�����ꂽ�^�X�N�̏ڍ�", EditorStyles.boldLabel);
            GUILayout.Label("�^�C�g��: " + selectedTask.Title);
            GUILayout.Label("�J�e�S���[: " + selectedTask.Category);
            GUILayout.Label("����: " + selectedTask.Description);
            GUILayout.Label("�D��x: " + selectedTask.Priority.ToString());
            GUILayout.Label("��Փx: " + selectedTask.Difficulty.ToString());

            if (GUILayout.Button("�^�X�N�̍폜"))
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
        AssetDatabase.SaveAssets(); // �f�[�^���m���ɕۑ�
    }

    private void RemoveTask(Task task)
    {
        if (taskData.Tasks.Contains(task))
        {
            taskData.Tasks.Remove(task);
            EditorUtility.SetDirty(taskData);
            AssetDatabase.SaveAssets(); // �폜��Ƀf�[�^��ۑ�
        }
    }

    private System.Collections.Generic.List<Task> GetFilteredAndSortedTasks()
    {
        if (taskData == null || taskData.Tasks == null)
            return new System.Collections.Generic.List<Task>();

        var filteredTasks = filterCategory == "���ׂ�"
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
