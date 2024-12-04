using UnityEngine;

public class FlyEnemy : MoveEnemy
{
    [SerializeField, Header("飛ぶ速さ")]
    private float speed = 5f;

    private float fixedY;

    protected override void Awake()
    {
        base.Awake();
        fixedY = transform.position.y; // 初期Y座標を記録
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

        // Y座標を固定し、X座標を移動
        transform.position = new Vector3(
            transform.position.x - speed * Time.deltaTime,
            fixedY,
            transform.position.z
        );

        // 壁に当たったら死ぬ
        if (IsHittingWall())
        {
            SetIsDead(true);
        }
    }
}
