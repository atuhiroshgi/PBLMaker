using System.Runtime.CompilerServices;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Player player;
    private bool isGoal = false;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReachedPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ReachedPlayer();
        }
    }

    /// <summary>
    /// �S�[���ɓ���
    /// </summary>
    private void ReachedPlayer()
    {
        isGoal = true;
        player.ReachGoal();
    }
}
