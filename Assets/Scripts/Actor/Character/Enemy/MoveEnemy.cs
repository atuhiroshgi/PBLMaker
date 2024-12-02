using UnityEngine;

public class MoveEnemy : Enemy
{
    [SerializeField, Header("WallCheckの参照")]
    private WallCheck wallCheck;

    protected bool isFacingLeft = true;         //左を向いているかどうか

    protected override void Awake()
    {
        base.Awake();
        SetFacingLeft();
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

    public bool IsHittingWall()
    {
        if(wallCheck != null)
        {
            return wallCheck.GetIsHittingWall();
        }
        else
        {
            Debug.LogError("WallCheckコンポーネントがアタッチされていません");
            return false;
        }
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
    public void SetFacingLeft()
    {
        if (!isFacingLeft)
        {
            Flip();
        }
    }

    public void SetFacingRight()
    {
        if (isFacingLeft)
        {
            Flip();
        }
    }
}
