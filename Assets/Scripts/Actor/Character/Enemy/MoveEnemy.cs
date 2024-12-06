using UnityEngine;

public class MoveEnemy : Enemy
{
    [SerializeField, Header("WallCheckの参照")]
    private WallCheck wallCheck;

    

    protected override void Awake()
    {
        base.Awake();
        SetFacingLeft();
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Ground()
    {
        base.Ground();
    }

    /// <summary>
    /// 動きを止めるメソッド
    /// </summary>
    protected void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsHittingWall()
    {
        if(wallCheck != null)
        {
            return wallCheck.GetIsHittingWall();
        }
        else
        {
            Debug.LogError("WallCheckコンポーネントがアタッチされていません");
            return false;
        }
    }
}
