using Cysharp.Threading.Tasks;
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

        //ジャンプ処理を非同期で監視する
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
    /// Spaceキーが押されるまで待機
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitForJumpAsync()
    {
        while (true)
        {
            // 地面に達していてSpaceキーが押されたらジャンプ
            if(isGround && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            await UniTask.Yield();
        }
    }

    /// <summary>
    /// ジャンプの処理
    /// </summary>
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// 設置しているかどうか
    /// </summary>
    private void Physics()
    {
        isGround = groundCheck.GetIsGround();
    }

    /// <summary>
    /// カメラに対する特定の位置に固定する処理
    /// </summary>
    private void FixPositionToCamera()
    {
        if (mainCamera != null)
        {
            // カメラの位置にオフセットを加えた位置にプレイヤーの位置を固定する
            Vector3 targetPosition = mainCamera.transform.position + cameraOffset;
            targetPosition.z = 0;
            transform.position = targetPosition;
        }
    }
}
