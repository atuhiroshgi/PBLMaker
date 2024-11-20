using UnityEngine;

public class Character : Actor
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;
    protected Rigidbody2D rb;

    protected bool isDead = false;  // ����ł��邩�ǂ����̃t���O

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        Init();
    }

    /// <summary>
    /// �������p�̃��\�b�h
    /// </summary>
    protected virtual void Init()
    {

    }

    /// <summary>
    /// ���񂾂Ƃ��ɌĂ΂�郁�\�b�h
    /// </summary>
    protected virtual void Death()
    {
    }
}
