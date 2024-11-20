using UnityEngine;

public class MoveEnemy : Enemy
{
    [SerializeField, Header("�ǂƂ��ĔF�����郌�C���[")]
    protected LayerMask wallLayer;
    [SerializeField, Header("�ǌ��m�p�̃��C�̒���")]
    protected float wallCheckDistance = 0.5f;


    protected float direction;                  //����
    protected bool isFacingLeft = true;         //���������Ă��邩�ǂ���

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
    /// �������~�߂郁�\�b�h
    /// </summary>
    protected void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// �ǂɓ������Ă��邩�ǂ��������C�L���X�g�Ŕ��f���郁�\�b�h
    /// </summary>
    /// <returns>�ǂɓ����������ǂ���</returns>
    protected bool IsHittingWall()
    {
        // ���݂̕����Ƀ��C���΂�
        Vector2 direction = isFacingLeft ? Vector2.left : Vector2.right;

        // ���C�L���X�g�̊J�n�ʒu�𒲐�
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(isFacingLeft ? -0.5f : 0.5f, 0);

        // ���C�L���X�g�ŕǂɓ����������ǂ����𔻒f����
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, wallCheckDistance, wallLayer);

        // ���C���ǂɓ��������ǂ����𔻒�
        return hit.collider != null;
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
    protected void SetFacingLeft()
    {
        if (!isFacingLeft)
        {
            Flip();
        }
    }
}
