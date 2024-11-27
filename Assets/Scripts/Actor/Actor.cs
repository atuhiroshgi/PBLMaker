using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public void AddGoal()
    {
        this.AddComponent<Goal>();
    }
}
