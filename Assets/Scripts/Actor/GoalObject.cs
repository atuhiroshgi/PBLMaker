using UnityEngine;

public class GoalObject : Actor
{
    private void Awake()
    {
        AddGoal();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // プレイヤーのスクリプトに目標地点を設定する
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.FaceTarget(transform.position);
            }
        }
    }
}
