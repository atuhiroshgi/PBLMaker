using UnityEngine;

public class Character : Actor
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;
    protected Rigidbody2D rb;

    protected bool isDead = false;          // ����ł��邩�ǂ����̃t���O
    protected bool isFacingLeft = true;     //���������Ă��邩�ǂ���

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

    /// <summary>
    /// �����𔽓]���郁�\�b�h
    /// </summary>
    protected void Flip()
    {
        // �����𔽓]������
        isFacingLeft = !isFacingLeft;

        // �X�v���C�g�𔽓]������
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    /// <summary>
    /// ���Ɍ���
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
