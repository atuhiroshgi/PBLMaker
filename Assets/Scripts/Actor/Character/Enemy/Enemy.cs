using UnityEngine;

public class Enemy : Character
{
    [SerializeField, Header("�̗�")]
    protected int health = 1;
    [SerializeField, Header("���G���ǂ���")]
    protected bool invincible = false;

    protected Player player;
    protected GroundCheck groundCheck;

    protected Vector3 initialPosition;          // �����ʒu��c�����邽�߂�
    protected bool isSaved = false;             // �����ʒu��ۑ����Ă��邩�ǂ����̃t���O
    protected bool isResetPosition = false;     // �|�W�V���������Z�b�g�������ǂ���

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<Player>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    protected override void Init()
    {
        base.Init();

        // �ݒu���ꂽ�ꏊ���L��
        initialPosition = transform.position;
        isResetPosition = true;
    }

    protected virtual void Update()
    {
        if (player.GetIsDead())
        {
            // �v���C���[������ł�����S�Ă̓�����~�߂�
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            return;
        }

        if (!ExecuteManager.Instance.GetIsExecute())
        {
            // ���s���łȂ���Ώd�͂𓭂����Ȃ�
            rb.gravityScale = 0;

            // ���s���łȂ���Ίђʉ\�ɂ���
            col.enabled = true;
            col.isTrigger = true;

            isSaved = false;
            isDead = false;
            spriteRenderer.enabled = true;

            if (!isResetPosition)
            {
                // ���s���~�߂��Ƃ��A�܂����Z�b�g����Ă��Ȃ���Ώ����ʒu�ɖ߂�
                transform.position = initialPosition;
                isResetPosition = true;
            }
        }
        else
        {
            if (!isSaved)
            {
                // ���s�����Ƃ��A�܂������ʒu��ۑ����Ă��Ȃ���Εۑ�����
                isSaved = true;
                initialPosition = transform.position;
            }

            // ���s���ꂽ��ђʂ��~�߂�
            col.isTrigger = false;

            // ���s������d�͂𓭂�����
            rb.gravityScale = 3;
            isResetPosition = false;
        }

        if(groundCheck != null && groundCheck.GetIsGround())
        {
            Ground();
        }
    }

    /// <summary>
    /// �n�ʂɂ���Ƃ��Ɏ��s����郁�\�b�h
    /// </summary>
    protected virtual void Ground()
    {
    }

    protected override void Death()
    {
        spriteRenderer.enabled = false;
        col.isTrigger = true;
    }

    /// <summary>
    /// ���S��Ԃ̃Z�b�^�[
    /// </summary>
    public void SetIsDead(bool isDead)
    {
        if (invincible) return;     // ���G�Ȃ玀�ȂȂ�
        this.isDead = isDead;

        Death();
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
