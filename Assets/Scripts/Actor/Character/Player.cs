using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private static float CLEAR_COOLTIME = 2f;    // クリアした後のクールタイム

    [SerializeField, Header("GroundCheckを参照")]
    private GroundCheck groundCheck = null;
    [SerializeField, Header("一時停止中にカメラに対してどの位置に配置するか")]
    private Vector3 cameraOffset = new Vector3(-11, -3, 0);
    [SerializeField, Header("プレイヤーの移動速度")]
    private float moveSpeed = 5f;
    [SerializeField, Header("空中でのプレイヤーの移動速度")]
    private float airMoveSpeed = 3f;
    [SerializeField, Header("ジャンプの強さ")]
    private float jumpForce = 7f;
    [SerializeField, Header("通常時の重力スケール")]
    private float defaultGravityScale = 1f;
    [SerializeField, Header("ジャンプ中の重力スケール")]
    private float jumpGravityScale = 0.5f;
    [SerializeField, Header("落下中の重力スケール")]
    private float fallGravityScale = 2f;

    private Camera mainCamera;
    private float lastDirection = 1;    // 最後の移動方向を保持(1は右向き, -1は左向き)
    private float playerSpeed;          // プレイヤーのスピードを格納するため
    private bool isGround = false;      // 地面に触れているかどうか
    private bool isClear = false;       // クリアしているかどうか
    private bool isDead = false;        // 死んでいるかどうか

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            //実行中だけ操作可能
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

        // 地上と空中で横移動できる速度を変える
        playerSpeed = isGround ? moveSpeed : airMoveSpeed;
        rb.linearVelocity = new Vector2(moveInput * playerSpeed, rb.linearVelocity.y);

        // プレイヤーが移動している場合、方向を更新
        if(moveInput != 0)
        {
            lastDirection = Mathf.Sign(lastDirection);

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
        if (!ExecuteManager.Instance.GetIsExecute()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
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
        //動きを止める
        rb.linearVelocity = Vector2.zero;
        //重力を無効化
        rb.gravityScale = 0;

        //カメラに対して特定の位置にPlayerを固定
        FixPositionToCamera();
    }

    /// <summary>
    /// プレイヤーの速度を取得するためのゲッター
    /// </summary>
    /// <returns>プレイヤーの速度</returns>
    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }
}
