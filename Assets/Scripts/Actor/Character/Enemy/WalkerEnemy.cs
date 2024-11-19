using UnityEngine;

public class WalkerEnemy : MoveEnemy
{
    [SerializeField, Header("移動速度")]
    protected float moveSpeed = 2f;
    [SerializeField, Header("空中での移動速度の割合")]
    protected float airMoveFactor = 0.5f;

    protected float currentMoveSpeed;       // 現在の移動速度

    protected override void Update()
    {
        base.Update();

        if (ExecuteManager.Instance.GetIsExecute())
        {
            Patrol();
        }
        else
        {
            StopMovement();
            SetFacingLeft();
        }
    }

    /// <summary>
    /// 壁に当たったら向きを変えながら巡回するための処理
    /// </summary>
    protected void Patrol()
    {
        if (player.GetIsDead()) return;     //プレイヤーが死んでいたらパトロールを停止

        // 壁に当たったかどうかをレイキャストで判断して反転
        if (IsHittingWall())
        {
            Flip();
        }

        // 現在の方向に従って移動する
        direction = isFacingLeft ? -1f : 1f;
        currentMoveSpeed = groundCheck.GetIsGround() ? moveSpeed : airMoveFactor;
        rb.linearVelocity = new Vector2(currentMoveSpeed * direction, rb.linearVelocity.y);
    }
}
