using System;

[System.Serializable]
public class Task
{
    public string Title;
    public string Description;
    public string Category;
    public bool IsHidden;
    public PriorityLevel Priority;
    public DifficultyLevel Difficulty;  // 難易度プロパティを追加
}


// 優先度を定義する列挙体
public enum PriorityLevel
{
    Low,      // 低
    Medium,   // 中
    High      // 高
}

// 難易度の列挙体
public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
