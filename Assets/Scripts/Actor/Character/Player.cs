using Cysharp.Threading.Tasks;
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

        //�W�����v������񓯊��ŊĎ�����
        WaitForJumpAsync().Forget();
    }

    private void Update()
    {
        Physics();
    }

    private void FixedUpdate()
    {
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
    /// Space�L�[���������܂őҋ@
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitForJumpAsync()
    {
        while (true)
        {
            // �n�ʂɒB���Ă���Space�L�[�������ꂽ��W�����v
            if(isGround && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// �W�����v�̏���
    /// </summary>
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// �ݒu���Ă��邩�ǂ���
    /// </summary>
    private void Physics()
    {
        isGround = groundCheck.GetIsGround();
    }

    /// <summary>
    /// �J�����ɑ΂������̈ʒu�ɌŒ肷�鏈��
    /// </summary>
    private void FixPositionToCamera()
    {
        if (mainCamera != null)
        {
            // �J�����̈ʒu�ɃI�t�Z�b�g���������ʒu�Ƀv���C���[�̈ʒu���Œ肷��
            Vector3 targetPosition = mainCamera.transform.position + cameraOffset;
            targetPosition.z = 0;
            transform.position = targetPosition;
        }
    }
}
