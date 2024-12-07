using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private static readonly KeyCode DASH_KEY = KeyCode.LeftShift;       // �_�b�V������Ƃ��Ɏg���L�[
    private static readonly string ENEMY_TAG = "Enemy";                 // �G�I�u�W�F�N�g�̃^�O
    private static readonly string DEATH_OBJECT_TAG = "DeathObject";    // �G�ꂽ�玀�ʃI�u�W�F�N�g�̃^�O
    private static readonly string CLEAR_MESSAGE = "CLEAR!!";           // �~�X�����Ƃ��ɕ\�����郁�b�Z�[�W
    private static readonly string FAILED_MESSAGE = "FAILED!!";         // �~�X�����Ƃ��ɕ\�����郁�b�Z�[�W
    private static readonly float CLEAR_COOLTIME = 2f;                  // �N���A������̃N�[���^�C��
    

    [SerializeField, Header("CameraController���Q��")]
    private CameraController cameraController;
    [SerializeField, Header("GroundCheck���Q��")]
    private GroundCheck groundCheck = null;
    [SerializeField, Header("�ꎞ��~���ɃJ�����ɑ΂��Ăǂ̈ʒu�ɔz�u���邩")]
    private Vector3 cameraOffset = new Vector3(-11, -3, 0);
    [SerializeField, Header("���b�Z�[�WUI")]
    private TMPro.TMP_Text messageText;
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
    [SerializeField, Header("x���W�̍ŏ��l")]
    private float xMin = -14f;
    [SerializeField, Header("x���W�̍ő�l")]
    private float xMax;
    [SerializeField, Header("y���W�̍ŏ��l")]
    private float yMin = -12f;

    private Camera mainCamera;
    private Animator animator;
    private float lastDirection = 1;        // �Ō�̈ړ�������ێ�(1�͉E����, -1�͍�����)
    private float playerSpeed;              // �v���C���[�̃X�s�[�h���i�[���邽��
    private bool isGround = false;          // �n�ʂɐG��Ă��邩�ǂ���
    private bool isClear = false;           // �N���A���Ă��邩�ǂ���
    private bool isJump = false;            // �W�����v���Ă��邩�ǂ���
    private bool canMove = false;           // �v���C���[�������邩�ǂ���
    private bool canJump = false;           // �v���C���[���W�����v�ł��邩�ǂ���

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        isFacingLeft = false;
        animator = GetComponent<Animator>();
        messageText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            if (isClear || isDead) return;      //�N���A�ς�or����ł����瑀��𖳌���
            // ���s����������\
            Jump();
            CheckPlayerX();
            CheckPlayerY();
            UpdateAnimationParameters();
        }
        else
        {
            SetFacingRight();
            DisableOperation();
            ResetAnimationParameters();
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
        if (!canMove) return;   // canMove��false�Ȃ瓮�����Ȃ�
        float moveInput = Input.GetAxis("Horizontal");

        // �_�b�V�������ǂ����ňړ����x��ς���
        playerSpeed = Input.GetKey(DASH_KEY) ? dashMoveSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * playerSpeed, rb.linearVelocity.y);

        // �v���C���[���ړ����Ă���ꍇ�A�������X�V
        if(moveInput != 0)
        {
            lastDirection = Mathf.Sign(moveInput);

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
        if (!ExecuteManager.Instance.GetIsExecute() || !canJump) return;

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

    public void Bounce(float bounceForce)
    {
        // Y�����̑��x�����Z�b�g���ď�����̗͂�������
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
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

    private void CheckPlayerX()
    {
        Vector3 correctedPosition = transform.position;

        if(transform.position.x < xMin)
        {
            correctedPosition.x = xMin;
            transform.position = correctedPosition;
        }
        if(transform.position.x > xMax)
        {
            correctedPosition.x = xMax;
            transform.position = correctedPosition;
        }
    }

    private void CheckPlayerY()
    {
        if(transform.position.y < yMin)
        {
            isDead = true;
            ShowMessage(FAILED_MESSAGE);
        }
    }

    private void UpdateAnimationParameters()
    {
        // �v���C���[�̈ړ����x�𔽉f������
        float speed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", Mathf.Max(speed, 0f));

        // �W�����v��Ԃ𔽉f������
        bool isJumping = !isGround;
        animator.SetBool("IsJumping", isJumping);

        bool isFalling = rb.linearVelocity.y < 0 && !isGround;
        animator.SetBool("IsFalling", isFalling);
    }

    private void ResetAnimationParameters()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
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
        // �������~�߂�
        rb.linearVelocity = Vector2.zero;
        // �d�͂𖳌���
        rb.gravityScale = 0;

        // �J�����ɑ΂��ē���̈ʒu��Player���Œ�
        FixPositionToCamera();

        // ���b�Z�[�W�𑦍��ɔ�\������
        messageText.gameObject.SetActive(false);
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        // ���݈ʒu����ڕW�n�_�ւ̈ړ�
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // �ڕW�n�_�ɓ��B�������~
        if(Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// ���b�Z�[�W��\�����郁�\�b�h
    /// </summary>
    /// <param name="message">���b�Z�[�W</param>
    private async void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        await HideMessageAfterDelay(CLEAR_COOLTIME);
    }

    /// <summary>
    /// ��莞�Ԍ�Ƀ��b�Z�[�W���\���ɂ���R���[�`��
    /// </summary>
    /// <param name="delay">�N���A���Ă�����s��~�܂ł̎���</param>
    /// <returns></returns>
    private async UniTask HideMessageAfterDelay(float delay)
    {
        await UniTask.Delay((int)(delay * 1000));
        messageText.gameObject.SetActive(false);
        cameraController.MoveToStartPosition();
        ExecuteManager.Instance.SetIsExecute(false);
        isClear = false;
        isDead = false;
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
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else
            {
                if (collision.gameObject.GetComponent<Enemy>().GetIsDead()) return;
                isDead = true;
                ShowMessage(FAILED_MESSAGE);
            }
        }

        if (collision.gameObject.CompareTag(DEATH_OBJECT_TAG))
        {
            if (isDead) return;
            isDead = true;
            ShowMessage(FAILED_MESSAGE);
        }
    }

    /// <summary>
    /// �S�[�����B���ɌĂԏ���
    /// </summary>
    public void ReachGoal()
    {
        if (isClear || isDead) return;      // �S�[�����Ă����莀��ł���ꍇ�͏������Ȃ�
        isClear = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        ShowMessage(CLEAR_MESSAGE);
    }

    /// <summary>
    /// �X�L���̃I���I�t�̃Z�b�^�[
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isChecked"></param>
    public void SetSkill(int index, bool isChecked)
    {
        switch (index)
        {
            case 0:
                canMove = isChecked;
                Debug.Log($"canMove��{isChecked}");
                break;

            case 1:
                canJump = isChecked;
                Debug.Log($"canJump��{isChecked}");
                break;

            default:
                Debug.LogError("�\������Ȃ��C���f�b�N�X���w�肳��Ă��܂�");
                break;
        }
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
    /// ���S�̃Z�b�^�[
    /// </summary>
    public void SetIsDead()
    {
        isDead = true;
        ShowMessage(FAILED_MESSAGE);
    }

    /// <summary>
    /// ����ł��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns></returns>
    public bool GetIsDead()
    {
        return isDead;
    }

    public bool[] GetPlayerSkills()
    {
        bool[] currentSkill = { canMove, canJump };
        return currentSkill;
    }

    private void ForDebug()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            canMove = true;
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            canMove = false;
            canJump = false;
        }
    }
}