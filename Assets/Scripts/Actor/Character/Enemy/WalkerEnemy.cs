using UnityEngine;

public class WalkerEnemy : MoveEnemy
{
    [SerializeField, Header("�ړ����x")]
    protected float moveSpeed = 2f;
    [SerializeField, Header("�󒆂ł̈ړ����x�̊���")]
    protected float airMoveFactor = 0.5f;

    protected float currentMoveSpeed;       // ���݂̈ړ����x

    protected override void Update()
    {
        base.Update();

        if (ExecuteManager.Instance.GetIsExecute())
        {
            Patrol();
        }
        else
        {
            StopMovement();
            SetFacingLeft();
        }
    }

    /// <summary>
    /// �ǂɓ��������������ς��Ȃ��珄�񂷂邽�߂̏���
    /// </summary>
    protected void Patrol()
    {
        if (player.GetIsDead()) return;     //�v���C���[������ł�����p�g���[�����~

        // �ǂɓ����������ǂ��������C�L���X�g�Ŕ��f���Ĕ��]
        if (IsHittingWall())
        {
            Flip();
        }

        // ���݂̕����ɏ]���Ĉړ�����
        direction = isFacingLeft ? -1f : 1f;
        currentMoveSpeed = groundCheck.GetIsGround() ? moveSpeed : airMoveFactor;
        rb.linearVelocity = new Vector2(currentMoveSpeed * direction, rb.linearVelocity.y);
    }
}
