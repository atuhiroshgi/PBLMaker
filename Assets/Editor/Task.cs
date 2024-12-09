using System;

[System.Serializable]
public class Task
{
    public string Title;
    public string Description;
    public string Category;
    public bool IsHidden;
    public PriorityLevel Priority;
    public DifficultyLevel Difficulty;  // ��Փx�v���p�e�B��ǉ�
}


// �D��x���`����񋓑�
public enum PriorityLevel
{
    Low,      // ��
    Medium,   // ��
    High      // ��
}

// ��Փx�̗񋓑�
public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
