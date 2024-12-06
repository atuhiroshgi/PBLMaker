using UnityEngine;

public class Character : Actor
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;
    protected Rigidbody2D rb;

    protected bool isDead = false;          // 死んでいるかどうかのフラグ
    protected bool isFacingLeft = true;     //左を向いているかどうか

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        Init();
    }

    /// <summary>
    /// 初期化用のメソッド
    /// </summary>
    protected virtual void Init()
    {

    }

    /// <summary>
    /// 死んだときに呼ばれるメソッド
    /// </summary>
    protected virtual void Death()
    {
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
