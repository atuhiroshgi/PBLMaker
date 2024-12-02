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

        // 実行時のみ巡回
        if (ExecuteManager.Instance.GetIsExecute())
        {
            Patrol();
        }
        else
        {
            StopMovement();     // 動きを止める
            SetFacingLeft();    // 初期状態で左向きに設定
        }
    }

    /// <summary>
    /// 壁に当たったら向きを変えながら巡回するための処理
    /// </summary>
    protected void Patrol()
    {
        // プレイヤーが死亡していたら処理を終了
        if (player.GetIsDead())
        {
            StopMovement();
            return;
        }

        // 壁に当たったかどうかをレイキャストで判断して反転
        if (IsHittingWall())
        {
            Flip();
        }

        // 地上か空中かで移動速度を設定
        currentMoveSpeed = groundCheck.GetIsGround() ? moveSpeed : airMoveFactor;

        // 現在の方向に基づいて移動
        float direction = isFacingLeft ? -1f : 1f;
        rb.linearVelocity = new Vector2(currentMoveSpeed * direction, rb.linearVelocity.y);
    }
}
