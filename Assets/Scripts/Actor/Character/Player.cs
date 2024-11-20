using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private static readonly KeyCode DASH_KEY = KeyCode.LeftShift;       // �_�b�V������Ƃ��Ɏg���L�[
    private static readonly string ENEMY_TAG = "Enemy";                 // �G�I�u�W�F�N�g�̃^�O
    private static readonly string DEATH_OBJECT_TAG = "DeathObject";    // �G�ꂽ�玀�ʃI�u�W�F�N�g�̃^�O
    private static readonly float CLEAR_COOLTIME = 2f;                  // �N���A������̃N�[���^�C��
    private static readonly float DEAD_LINE = -8f;                      // ���ʃ��C��

    [SerializeField, Header("GroundCheck���Q��")]
    private GroundCheck groundCheck = null;
    [SerializeField, Header("�ꎞ��~���ɃJ�����ɑ΂��Ăǂ̈ʒu�ɔz�u���邩")]
    private Vector3 cameraOffset = new Vector3(-11, -3, 0);
    [SerializeField, Header("�v���C���[�̈ړ����x")]
    private float moveSpeed = 5f;
    [SerializeField, Header("�_�b�V�����̃v���C���[�̈ړ����x")]
    private float dashMoveSpeed = 10f;
    [SerializeField, Header("�W�����v�̋���")]
    private float jumpForce = 7f;
    [SerializeField, Header("�ʏ펞�̏d�̓X�P�[��")]
    private float defaultGravityScale = 1f;
    [SerializeField, Header("�W�����v���̏d�̓X�P�[��")]
    private float jumpGravityScale = 0.5f;
    [SerializeField, Header("�������̏d�̓X�P�[��")]
    private float fallGravityScale = 2f;

    private Camera mainCamera;
    private float lastDirection = 1;        // �Ō�̈ړ�������ێ�(1�͉E����, -1�͍�����)
    private float playerSpeed;              // �v���C���[�̃X�s�[�h���i�[���邽��
    private bool isGround = false;          // �n�ʂɐG��Ă��邩�ǂ���
    private bool isClear = false;           // �N���A���Ă��邩�ǂ���
    private bool isJump = false;            // �W�����v���Ă��邩�ǂ���

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            if (isClear || isDead) return;      //�N���A�ς�or����ł����瑀��𖳌���
            // ���s����������\
            Jump();
            CheckPlayerY();
        }
        else
        {
            DisableOperation();
        }

        ForDebug();
    }

    private void FixedUpdate()
    {
        if (!ExecuteManager.Instance.GetIsExecute() || isClear || isDead) return;
        Physics();
        AdjustGravity();
        Move();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // �_�b�V�������ǂ����ňړ����x��ς���
        playerSpeed = Input.GetKey(DASH_KEY) ? dashMoveSpeed : moveSpeed;
        
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
            if (isGround)
            {
                // �n��ł̃W�����v
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isJump = true;
            }
        }

        if (isGround)
        {
            isJump = false;
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

    private void CheckPlayerY()
    {
        if(transform.position.y < DEAD_LINE)
        {
            isDead = true;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            // �v���C���[�̑������G�����ɂ�����|���A����ȊO�Ȃ�v���C���[������
            if(transform.position.y - 1 > collision.gameObject.transform.position.y)
            {
                // �G�����S��Ԃɂ���
                enemy.SetIsDead(true);
            }
            else
            {
                if (collision.gameObject.GetComponent<Enemy>().GetIsDead()) return;
                isDead = true;
            }
        }

        if (collision.gameObject.CompareTag(DEATH_OBJECT_TAG))
        {
            if (isDead) return;
            isDead = true;
        }
    }

    /// <summary>
    /// �S�[�����B���ɌĂԏ���
    /// </summary>
    public void GoalReached()
    {
        if (isClear || isDead) return;      // �S�[�����Ă����莀��ł���ꍇ�͏������Ȃ�
        isClear = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Debug.Log("�S�[���ɓ��B���܂���");
    }


    /// <summary>
    /// �v���C���[�̑��x���擾���邽�߂̃Q�b�^�[
    /// </summary>
    /// <returns>�v���C���[�̑��x</returns>
    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    /// <summary>
    /// ����ł��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns></returns>
    public bool GetIsDead()
    {
        return isDead;
    }

    private void ForDebug()
    {

    }
}
