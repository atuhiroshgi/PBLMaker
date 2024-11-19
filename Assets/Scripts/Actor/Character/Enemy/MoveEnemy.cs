using UnityEngine;

public class MoveEnemy : Enemy
{
    [SerializeField, Header("壁として認識するレイヤー")]
    protected LayerMask wallLayer;
    [SerializeField, Header("壁検知用のレイの長さ")]
    protected float wallCheckDistance = 0.5f;


    protected float direction;                  //向き
    protected bool isFacingLeft = true;         //左を向いているかどうか

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void Ground()
    {
        base.Ground();
    }

    /// <summary>
    /// 動きを止めるメソッド
    /// </summary>
    protected void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// 壁に当たっているかどうかをレイキャストで判断するメソッド
    /// </summary>
    /// <returns>壁に当たったかどうか</returns>
    protected bool IsHittingWall()
    {
        // 現在の方向にレイを飛ばす
        Vector2 direction = isFacingLeft ? Vector2.left : Vector2.right;

        // レイキャストの開始位置を調整
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(isFacingLeft ? -0.5f : 0.5f, 0);

        // レイキャストで壁に当たったかどうかを判断する
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, wallCheckDistance, wallLayer);

        // レイが壁に当たったどうかを判定
        return hit.collider != null;
    }

    /// <summary>
    /// 向きを反転するメソッド
    /// </summary>
    protected void Flip()
    {
        // 向きを反転させる
        isFacingLeft = !isFacingLeft;

        // スプライトを反転させる
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /// <summary>
    /// 左に向く
    /// </summary>
    protected void SetFacingLeft()
    {
        if (!isFacingLeft)
        {
            Flip();
        }
    }
}
