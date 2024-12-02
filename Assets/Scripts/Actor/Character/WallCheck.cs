using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [SerializeField, Header("�ǂƂ��ĔF�����郌�C���[")]
    private LayerMask wallLayer;

    private bool isHittingWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ǃ��C���[�ɑ�����I�u�W�F�N�g�ɏՓ˂����ꍇ
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isHittingWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �ǃ��C���[�ɑ�����I�u�W�F�N�g���痣�ꂽ�ꍇ
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isHittingWall = false;
        }
    }

    public bool GetIsHittingWall()
    {
        return isHittingWall;
    }
}
