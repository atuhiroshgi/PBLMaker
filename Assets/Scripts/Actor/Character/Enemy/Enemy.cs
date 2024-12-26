using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField, Header("�X�R�A�\���p��Prefab")]
    private GameObject scorePopupPrefab;
    [SerializeField, Header("�̗�")]
    protected int health = 1;
    [SerializeField, Header("�|�����Ƃ��ɓ�����X�R�A")]
    protected int score;
    [SerializeField, Header("���G���ǂ���")]
    protected bool invincible = false;

    protected Player player;
    protected GroundCheck groundCheck;
    protected Vector3 initialPosition;          // �����ʒu��c�����邽�߂�
    protected bool isSaved = false;             // �����ʒu��ۑ����Ă��邩�ǂ����̃t���O
    protected bool isResetPosition = false;     // �|�W�V���������Z�b�g�������ǂ���
    protected bool isScored = false;

    private float scorePopupDuration = 0.5f;
    private bool hasColliderAssigned = false;   // ���s���ɓ����蔻������������ǂ����̃t���O

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
            hasColliderAssigned = false;

            isSaved = false;
            isDead = false;
            isScored = false;
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

            if (!hasColliderAssigned)
            {
                // ���s���ꂽ��ђʂ��~�߂�
                col.isTrigger = false;
                hasColliderAssigned = true;
            }

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
        if (isScored) return;

        Debug.Log("�G�|����");
        spriteRenderer.enabled = false;
        col.isTrigger = true;
        isScored = true;
        ScoreCounter.Instance.AddScore(score);
        ShowScorePopup();
    }

    private void ShowScorePopup()
    {
        if(scorePopupPrefab != null)
        {
            // �X�R�A�\����G�̈ʒu�ɐ���
            GameObject popup = Instantiate(scorePopupPrefab, transform.position, Quaternion.identity);

            // �X�R�A�\���I�u�W�F�N�g�ɃX�R�A�Ǝ������Ԃ�n��
            ScorePopup popupScript = popup.GetComponent<ScorePopup>();
            if(popupScript != null)
            {
                popupScript.Initialize(score, scorePopupDuration);
            }
        }
    }

    /// <summary>
    /// ���S��Ԃ̃Z�b�^�[
    /// </summary>
    public void SetIsDead(bool isDead)
    {
        if (invincible) return;     // ���G�Ȃ玀�ȂȂ�
        this.isDead = isDead;

        if(isDead) Death();
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
