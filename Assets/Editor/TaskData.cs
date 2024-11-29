using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Task Manager/Task Data", order = 1)]
public class TaskData : ScriptableObject
{
    public List<Task> Tasks = new List<Task>();
}
