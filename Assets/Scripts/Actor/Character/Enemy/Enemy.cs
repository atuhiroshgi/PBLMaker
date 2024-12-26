using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField, Header("スコア表示用のPrefab")]
    private GameObject scorePopupPrefab;
    [SerializeField, Header("体力")]
    protected int health = 1;
    [SerializeField, Header("倒したときに得られるスコア")]
    protected int score;
    [SerializeField, Header("無敵かどうか")]
    protected bool invincible = false;

    protected Player player;
    protected GroundCheck groundCheck;
    protected Vector3 initialPosition;          // 初期位置を把握するための
    protected bool isSaved = false;             // 初期位置を保存しているかどうかのフラグ
    protected bool isResetPosition = false;     // ポジションをリセットしたかどうか
    protected bool isScored = false;

    private float scorePopupDuration = 0.5f;
    private bool hasColliderAssigned = false;   // 実行時に当たり判定を加えたかどうかのフラグ

    protected override void Awake()
    {
        base.Awake();

        player = FindAnyObjectByType<Player>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    protected override void Init()
    {
        base.Init();

        // 設置された場所を記憶
        initialPosition = transform.position;
        isResetPosition = true;
    }

    protected virtual void Update()
    {
        if (player.GetIsDead())
        {
            // プレイヤーが死んでいたら全ての動作を止める
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
            return;
        }

        if (!ExecuteManager.Instance.GetIsExecute())
        {
            // 実行中でなければ重力を働かせない
            rb.gravityScale = 0;

            // 実行中でなければ貫通可能にする
            col.enabled = true;
            col.isTrigger = true;
            hasColliderAssigned = false;

            isSaved = false;
            isDead = false;
            isScored = false;
            spriteRenderer.enabled = true;

            if (!isResetPosition)
            {
                // 実行を止めたとき、まだリセットされていなければ初期位置に戻す
                transform.position = initialPosition;
                isResetPosition = true;
            }
        }
        else
        {
            if (!isSaved)
            {
                // 実行したとき、まだ初期位置を保存していなければ保存する
                isSaved = true;
                initialPosition = transform.position;
            }

            if (!hasColliderAssigned)
            {
                // 実行されたら貫通を止める
                col.isTrigger = false;
                hasColliderAssigned = true;
            }

            // 実行したら重力を働かせる
            rb.gravityScale = 3;
            isResetPosition = false;
        }

        if(groundCheck != null && groundCheck.GetIsGround())
        {

            Ground();
        }
    }

    /// <summary>
    /// 地面にいるときに実行されるメソッド
    /// </summary>
    protected virtual void Ground()
    {
    }

    protected override void Death()
    {
        if (isScored) return;

        Debug.Log("敵倒した");
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
            // スコア表示を敵の位置に生成
            GameObject popup = Instantiate(scorePopupPrefab, transform.position, Quaternion.identity);

            // スコア表示オブジェクトにスコアと持続時間を渡す
            ScorePopup popupScript = popup.GetComponent<ScorePopup>();
            if(popupScript != null)
            {
                popupScript.Initialize(score, scorePopupDuration);
            }
        }
    }

    /// <summary>
    /// 死亡状態のセッター
    /// </summary>
    public void SetIsDead(bool isDead)
    {
        if (invincible) return;     // 無敵なら死なない
        this.isDead = isDead;

        if(isDead) Death();
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
