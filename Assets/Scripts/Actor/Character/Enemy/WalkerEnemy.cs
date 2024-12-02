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

        // ���s���̂ݏ���
        if (ExecuteManager.Instance.GetIsExecute())
        {
            Patrol();
        }
        else
        {
            StopMovement();     // �������~�߂�
            SetFacingLeft();    // ������Ԃō������ɐݒ�
        }
    }

    /// <summary>
    /// �ǂɓ��������������ς��Ȃ��珄�񂷂邽�߂̏���
    /// </summary>
    protected void Patrol()
    {
        // �v���C���[�����S���Ă����珈�����I��
        if (player.GetIsDead())
        {
            StopMovement();
            return;
        }

        // �ǂɓ����������ǂ��������C�L���X�g�Ŕ��f���Ĕ��]
        if (IsHittingWall())
        {
            Flip();
        }

        // �n�ォ�󒆂��ňړ����x��ݒ�
        currentMoveSpeed = groundCheck.GetIsGround() ? moveSpeed : airMoveFactor;

        // ���݂̕����Ɋ�Â��Ĉړ�
        float direction = isFacingLeft ? -1f : 1f;
        rb.linearVelocity = new Vector2(currentMoveSpeed * direction, rb.linearVelocity.y);
    }
}
