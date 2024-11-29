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

    private Task selectedTask;  // �I�����ꂽ�^�X�N��ێ�

    // �D��x�̑I����
    private string[] priorityOptions = { "��", "��", "��" };
    private int selectedPriorityIndex = 0;  // �����́u��v

    // ��Փx�̑I����
    private string[] difficultyOptions = { "�ȒP", "��", "���" };
    private int selectedDifficultyIndex = 0;  // �����́u�ȒP�v

    // �\�[�g�I�v�V����
    private enum SortBy { Priority, Difficulty }  // ���בւ���̗񋓑�
    private SortBy selectedSortBy = SortBy.Priority;  // �����͗D��x�Ń\�[�g
    private bool sortDescending = false;  // �����Ȃ�false, �~���Ȃ�true

    [MenuItem("Tools/Task Manager")]
    public static void ShowWindow()
    {
        GetWindow<TaskManagerWindow>("Task Manager");
    }

    private void OnEnable()
    {
        // TaskData���v���W�F�N�g���烍�[�h�܂��͐V�K�쐬
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

        // �^�X�N�ǉ��t�H�[��
        GUILayout.Label("�V�����^�X�N��ǉ�");
        newTaskTitle = EditorGUILayout.TextField("�^�C�g��", newTaskTitle);
        newTaskDescription = EditorGUILayout.TextField("����", newTaskDescription);
        selectedCategoryIndex = EditorGUILayout.Popup("�J�e�S���[", selectedCategoryIndex, categories);

        // �D��x��I������h���b�v�_�E�����j���[��ǉ�
        selectedPriorityIndex = EditorGUILayout.Popup("�D��x", selectedPriorityIndex, priorityOptions);

        // ��Փx��I������h���b�v�_�E�����j���[��ǉ�
        selectedDifficultyIndex = EditorGUILayout.Popup("��Փx", selectedDifficultyIndex, difficultyOptions);

        if (GUILayout.Button("�^�X�N��ǉ�"))
        {
            AddTask();
        }

        GUILayout.Space(10);

        // �t�B���^�Z�N�V����
        GUILayout.Label("�t�B���^", EditorStyles.boldLabel);
        string[] filterOptions = new[] { "���ׂ�" }.Concat(categories).ToArray();
        int selectedFilterIndex = EditorGUILayout.Popup("�J�e�S���[", Array.IndexOf(filterOptions, filterCategory), filterOptions);
        filterCategory = filterOptions[selectedFilterIndex];

        GUILayout.Space(10);

        // �\�[�g�I�v�V����
        GUILayout.Label("�\�[�g", EditorStyles.boldLabel);
        selectedSortBy = (SortBy)EditorGUILayout.EnumPopup("�\�[�g�", selectedSortBy);  // �D��x�Ɠ�Փx�̑I����
        sortDescending = EditorGUILayout.Toggle("�~���Ń\�[�g", sortDescending);

        GUILayout.Space(10);

        // �^�X�N���X�g�\��
        GUILayout.Label("�^�X�N���X�g", EditorStyles.boldLabel);

        // �\�� (�w�b�_�[) �ǉ�
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("����", GUILayout.Width(50));
        GUILayout.Label("�^�C�g��", GUILayout.Width(150));
        GUILayout.Label("�J�e�S���[", GUILayout.Width(100));
        GUILayout.Label("�D��x", GUILayout.Width(60));  // �D��x
        GUILayout.Label("��Փx", GUILayout.Width(80));  // ��Փx
        GUILayout.Label("����", GUILayout.Width(120));  // ���� (�I���A�폜)
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // �t�B���^�K�p
        var filteredTasks = filterCategory == "���ׂ�"
            ? taskData.Tasks
            : taskData.Tasks.Where(task => task.Category == filterCategory).ToList();

        // �\�[�g�̓K�p
        if (selectedSortBy == SortBy.Priority)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Priority).ToList()  // �D��x�ō~��
                : filteredTasks.OrderBy(task => task.Priority).ToList();  // �D��x�ŏ���
        }
        else if (selectedSortBy == SortBy.Difficulty)
        {
            filteredTasks = sortDescending
                ? filteredTasks.OrderByDescending(task => task.Difficulty).ToList()  // ��Փx�ō~��
                : filteredTasks.OrderBy(task => task.Difficulty).ToList();  // ��Փx�ŏ���
        }

        // �^�X�N���X�g�\��
        foreach (var task in filteredTasks)
        {
            EditorGUILayout.BeginHorizontal();

            // ������Ԃ�\��
            task.IsCompleted = EditorGUILayout.Toggle(task.IsCompleted, GUILayout.Width(50));

            // �^�X�N�̃^�C�g���A�J�e�S���[�A�D��x�A��Փx
            GUILayout.Label(task.Title, GUILayout.Width(150));
            GUILayout.Label(task.Category, GUILayout.Width(100));
            GUILayout.Label(task.Priority.ToString(), GUILayout.Width(60));  // �D��x
            GUILayout.Label(task.Difficulty.ToString(), GUILayout.Width(80));  // ��Փx

            // ����{�^���i�I���A�폜�j
            if (GUILayout.Button("�I��", GUILayout.Width(60)))
            {
                selectedTask = task;  // �^�X�N��I��
            }

            if (GUILayout.Button("�폜", GUILayout.Width(60)))
            {
                RemoveTask(task);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // �I�������^�X�N�̏ڍׂ�\��
        if (selectedTask != null)
        {
            GUILayout.Label("�I�����ꂽ�^�X�N�̏ڍ�", EditorStyles.boldLabel);
            GUILayout.Label("�^�C�g��: " + selectedTask.Title);
            GUILayout.Label("�J�e�S���[: " + selectedTask.Category);
            GUILayout.Label("����: " + selectedTask.Description);
            GUILayout.Label("�D��x: " + selectedTask.Priority.ToString());  // �D��x�̕\��
            GUILayout.Label("��Փx: " + selectedTask.Difficulty.ToString());  // ��Փx�̕\��

            if (GUILayout.Button("�^�X�N�̍폜"))
            {
                RemoveTask(selectedTask);
                selectedTask = null;  // �폜��ɑI��������
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
            Priority = (PriorityLevel)selectedPriorityIndex,  // �D��x��ݒ�
            Difficulty = (DifficultyLevel)selectedDifficultyIndex,  // ��Փx��ݒ�
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
