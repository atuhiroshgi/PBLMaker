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
            // �v���C���[�̃X�N���v�g�ɖڕW�n�_��ݒ肷��
            Player player = collision.GetComponent<Player>();
            if(player != null)
            {
                player.FaceTarget(transform.position);
            }
        }
    }
}
