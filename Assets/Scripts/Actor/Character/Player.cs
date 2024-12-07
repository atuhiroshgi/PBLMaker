using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private static readonly KeyCode DASH_KEY = KeyCode.LeftShift;       // ダッシュするときに使うキー
    private static readonly string ENEMY_TAG = "Enemy";                 // 敵オブジェクトのタグ
    private static readonly string DEATH_OBJECT_TAG = "DeathObject";    // 触れたら死ぬオブジェクトのタグ
    private static readonly string CLEAR_MESSAGE = "CLEAR!!";           // ミスしたときに表示するメッセージ
    private static readonly string FAILED_MESSAGE = "FAILED!!";         // ミスしたときに表示するメッセージ
    private static readonly float CLEAR_COOLTIME = 2f;                  // クリアした後のクールタイム
    

    [SerializeField, Header("CameraControllerを参照")]
    private CameraController cameraController;
    [SerializeField, Header("GroundCheckを参照")]
    private GroundCheck groundCheck = null;
    [SerializeField, Header("一時停止中にカメラに対してどの位置に配置するか")]
    private Vector3 cameraOffset = new Vector3(-11, -3, 0);
    [SerializeField, Header("メッセージUI")]
    private TMPro.TMP_Text messageText;
    [SerializeField, Header("プレイヤーの移動速度")]
    private float moveSpeed = 5f;
    [SerializeField, Header("ダッシュ中のプレイヤーの移動速度")]
    private float dashMoveSpeed = 10f;
    [SerializeField, Header("ジャンプの強さ")]
    private float jumpForce = 7f;
    [SerializeField, Header("通常時の重力スケール")]
    private float defaultGravityScale = 1f;
    [SerializeField, Header("ジャンプ中の重力スケール")]
    private float jumpGravityScale = 0.5f;
    [SerializeField, Header("落下中の重力スケール")]
    private float fallGravityScale = 2f;
    [SerializeField, Header("x座標の最小値")]
    private float xMin = -14f;
    [SerializeField, Header("x座標の最大値")]
    private float xMax;
    [SerializeField, Header("y座標の最小値")]
    private float yMin = -12f;

    private Camera mainCamera;
    private Animator animator;
    private float lastDirection = 1;        // 最後の移動方向を保持(1は右向き, -1は左向き)
    private float playerSpeed;              // プレイヤーのスピードを格納するため
    private bool isGround = false;          // 地面に触れているかどうか
    private bool isClear = false;           // クリアしているかどうか
    private bool isJump = false;            // ジャンプしているかどうか
    private bool canMove = false;           // プレイヤーが動けるかどうか
    private bool canJump = false;           // プレイヤーがジャンプできるかどうか

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
            if (isClear || isDead) return;      //クリア済みor死んでいたら操作を無効化
            // 実行中だけ操作可能
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
        if (!canMove) return;   // canMoveがfalseなら動かせない
        float moveInput = Input.GetAxis("Horizontal");

        // ダッシュ中かどうかで移動速度を変える
        playerSpeed = Input.GetKey(DASH_KEY) ? dashMoveSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * playerSpeed, rb.linearVelocity.y);

        // プレイヤーが移動している場合、方向を更新
        if(moveInput != 0)
        {
            lastDirection = Mathf.Sign(moveInput);

            //最後の移動方向に基づいてキャラクターを左右反転
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * lastDirection;
            transform.localScale = scale;
        }
    }

    /// <summary>
    /// ジャンプの処理
    /// </summary>
    private void Jump()
    {
        if (!ExecuteManager.Instance.GetIsExecute() || !canJump) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                // 地上でのジャンプ
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
        // Y方向の速度をリセットして上向きの力を加える
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }

    /// <summary>
    /// 設置しているかどうか
    /// </summary>
    private void Physics()
    {
        isGround = groundCheck.GetIsGround();
    }

    /// <summary>
    /// 状態に応じて重力を変える
    /// </summary>
    private void AdjustGravity()
    {
        //ジャンプ中、落下中、通常時で重力を変える
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
        // プレイヤーの移動速度を反映させる
        float speed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", Mathf.Max(speed, 0f));

        // ジャンプ状態を反映させる
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
    /// カメラに対する特定の位置に固定する処理
    /// </summary>
    private void FixPositionToCamera()
    {
        if (mainCamera != null && !ExecuteManager.Instance.GetIsExecute())
        {
            // カメラの位置にオフセットを加えた位置にプレイヤーの位置を固定する
            Vector3 targetPosition = mainCamera.transform.position + cameraOffset;
            targetPosition.z = 0;
            transform.position = targetPosition;
        }
    }

    /// <summary>
    /// 操作を無効化するときに呼ぶ処理
    /// </summary>
    private void DisableOperation()
    {
        // 動きを止める
        rb.linearVelocity = Vector2.zero;
        // 重力を無効化
        rb.gravityScale = 0;

        // カメラに対して特定の位置にPlayerを固定
        FixPositionToCamera();

        // メッセージを即座に非表示する
        messageText.gameObject.SetActive(false);
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        // 現在位置から目標地点への移動
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // 目標地点に到達したら停止
        if(Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// メッセージを表示するメソッド
    /// </summary>
    /// <param name="message">メッセージ</param>
    private async void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        await HideMessageAfterDelay(CLEAR_COOLTIME);
    }

    /// <summary>
    /// 一定時間後にメッセージを非表示にするコルーチン
    /// </summary>
    /// <param name="delay">クリアしてから実行停止までの時間</param>
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

            // プレイヤーの足元より敵が下にいたら倒す、それ以外ならプレイヤーが死ぬ
            if(transform.position.y - 1 > collision.gameObject.transform.position.y)
            {
                // 敵を死亡状態にする
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
    /// ゴール到達時に呼ぶ処理
    /// </summary>
    public void ReachGoal()
    {
        if (isClear || isDead) return;      // ゴールしていたり死んでいる場合は処理しない
        isClear = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        ShowMessage(CLEAR_MESSAGE);
    }

    /// <summary>
    /// スキルのオンオフのセッター
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isChecked"></param>
    public void SetSkill(int index, bool isChecked)
    {
        switch (index)
        {
            case 0:
                canMove = isChecked;
                Debug.Log($"canMoveが{isChecked}");
                break;

            case 1:
                canJump = isChecked;
                Debug.Log($"canJumpが{isChecked}");
                break;

            default:
                Debug.LogError("予測されないインデックスが指定されています");
                break;
        }
    }

    /// <summary>
    /// プレイヤーの速度を取得するためのゲッター
    /// </summary>
    /// <returns>プレイヤーの速度</returns>
    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    /// <summary>
    /// 死亡のセッター
    /// </summary>
    public void SetIsDead()
    {
        isDead = true;
        ShowMessage(FAILED_MESSAGE);
    }

    /// <summary>
    /// 死んでいるかどうかのゲッター
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