using UnityEngine;

public class Character : Actor
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;
    protected Rigidbody2D rb;

    protected bool isDead = false;  // 死んでいるかどうかのフラグ

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
}
