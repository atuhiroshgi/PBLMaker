using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private static float CLEAR_COOLTIME = 2f;    // �N���A������̃N�[���^�C��

    [SerializeField, Header("GroundCheck���Q��")]
    private GroundCheck groundCheck = null;
    [SerializeField, Header("�ꎞ��~���ɃJ�����ɑ΂��Ăǂ̈ʒu�ɔz�u���邩")]
    private Vector3 cameraOffset = new Vector3(-11, -3, 0);
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float moveSpeed = 5f;
    [SerializeField, Header("�󒆂ł̃v���C���[�̈ړ����x")]
    private float airMoveSpeed = 3f;
    [SerializeField, Header("�W�����v�̋���")]
    private float jumpForce = 7f;
    [SerializeField, Header("�ʏ펞�̏d�̓X�P�[��")]
    private float defaultGravityScale = 1f;
    [SerializeField, Header("�W�����v���̏d�̓X�P�[��")]
    private float jumpGravityScale = 0.5f;
    [SerializeField, Header("�������̏d�̓X�P�[��")]
    private float fallGravityScale = 2f;

    private Camera mainCamera;
    private float lastDirection = 1;    // �Ō�̈ړ�������ێ�(1�͉E����, -1�͍�����)
    private float playerSpeed;          // �v���C���[�̃X�s�[�h���i�[���邽��
    private bool isGround = false;      // �n�ʂɐG��Ă��邩�ǂ���
    private bool isClear = false;       // �N���A���Ă��邩�ǂ���
    private bool isDead = false;        // ����ł��邩�ǂ���

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            //���s����������\
            Jump();
        }
        else
        {
            DisableOperation();
        }
    }

    private void FixedUpdate()
    {
        if (!ExecuteManager.Instance.GetIsExecute()) return;
        Physics();
        AdjustGravity();
        Move();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // �n��Ƌ󒆂ŉ��ړ��ł��鑬�x��ς���
        playerSpeed = isGround ? moveSpeed : airMoveSpeed;
        rb.linearVelocity = new Vector2(moveInput * playerSpeed, rb.linearVelocity.y);

        // �v���C���[���ړ����Ă���ꍇ�A�������X�V
        if(moveInput != 0)
        {
            lastDirection = Mathf.Sign(lastDirection);

            //�Ō�̈ړ������Ɋ�Â��ăL�����N�^�[�����E���]
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * lastDirection;
            transform.localScale = scale;
        }
    }

    /// <summary>
    /// �W�����v�̏���
    /// </summary>
    private void Jump()
    {
        if (!ExecuteManager.Instance.GetIsExecute()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    /// <summary>
    /// �ݒu���Ă��邩�ǂ���
    /// </summary>
    private void Physics()
    {
        isGround = groundCheck.GetIsGround();
    }

    /// <summary>
    /// ��Ԃɉ����ďd�͂�ς���
    /// </summary>
    private void AdjustGravity()
    {
        //�W�����v���A�������A�ʏ펞�ŏd�͂�ς���
        rb.gravityScale =
            rb.linearVelocity.y > 0 ? jumpGravityScale :
            rb.linearVelocity.y < 0 ? fallGravityScale :
            defaultGravityScale;
    }

    /// <summary>
    /// �J�����ɑ΂������̈ʒu�ɌŒ肷�鏈��
    /// </summary>
    private void FixPositionToCamera()
    {
        if (mainCamera != null && !ExecuteManager.Instance.GetIsExecute())
        {
            // �J�����̈ʒu�ɃI�t�Z�b�g���������ʒu�Ƀv���C���[�̈ʒu���Œ肷��
            Vector3 targetPosition = mainCamera.transform.position + cameraOffset;
            targetPosition.z = 0;
            transform.position = targetPosition;
        }
    }

    /// <summary>
    /// ����𖳌�������Ƃ��ɌĂԏ���
    /// </summary>
    private void DisableOperation()
    {
        //�������~�߂�
        rb.linearVelocity = Vector2.zero;
        //�d�͂𖳌���
        rb.gravityScale = 0;

        //�J�����ɑ΂��ē���̈ʒu��Player���Œ�
        FixPositionToCamera();
    }

    /// <summary>
    /// �v���C���[�̑��x���擾���邽�߂̃Q�b�^�[
    /// </summary>
    /// <returns>�v���C���[�̑��x</returns>
    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }
}
