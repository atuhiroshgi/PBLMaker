using UnityEngine;

public class FlyEnemy : MoveEnemy
{
    [SerializeField, Header("��ԑ���")]
    private float speed = 5f;

    private float fixedY;

    protected override void Awake()
    {
        base.Awake();
        fixedY = transform.position.y; // ����Y���W���L�^
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected void Move()
    {
        if (!ExecuteManager.Instance.GetIsExecute())
        {
            return;
        }
        else
        {
            StopMovement();
        }

        // Y���W���Œ肵�AX���W���ړ�
        transform.position = new Vector3(
            transform.position.x - speed * Time.deltaTime,
            fixedY,
            transform.position.z
        );

        // �ǂɓ��������玀��
        if (IsHittingWall())
        {
            SetIsDead(true);
        }
    }
}
