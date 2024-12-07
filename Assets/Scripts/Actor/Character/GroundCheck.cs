using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static readonly string GROUND_TAG = "Ground";
    private static readonly string SPRING_TAG = "Spring";

    private bool isGround = false;      // �ݒu���Ă��邩�ǂ���
    private bool isGroundEnter = false; // �n�ʂɓ����Ă��邩�ǂ���
    private bool isGroundStay = false;  // �n�ʂɓ��葱���Ă��邩
    private bool isGroundExit = false;  // �n�ʂ���o����

    private bool isSpring = false;      // �o�l�ɏ���Ă��邩
    private bool isSpringEnter = false; // �o�l�ɓ�������
    private bool isSpringStay = false;  // �o�l�ɓ��葱���Ă��邩
    private bool isSpringExit = false;  // �o�l����o����

    private void Update()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }
        if (isSpringEnter || isSpringStay)
        {
            isSpring = true;
            Debug.Log("������");
        }
        else if (isSpringExit)
        {
            isSpring = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        isSpringEnter = false;
        isSpringStay = false;
        isSpringExit = false;
    }

    /// <summary>
    /// �ڒn���Ă��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns>�ڒn���Ă��邩�ǂ���</returns>
    public bool GetIsGround()
    {
        return isGround;
    }

    /// <summary>
    /// �o�l�ɏ���Ă��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns>�o�l�ɏ���Ă��邩�ǂ���</returns>
    public bool GetIsSpring()
    {
        return isSpring;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GROUND_TAG))
        {
            isGroundEnter = true;
        }
        else if (collision.CompareTag(SPRING_TAG))
        {
            isSpringEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GROUND_TAG))
        {
            isGroundStay = true;
        }
        else if (collision.CompareTag(SPRING_TAG))
        {
            isSpringStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(GROUND_TAG))
        {
            isGroundExit = true;
        }
        else if (collision.CompareTag(SPRING_TAG))
        {
            isSpringExit = true;
        }
    }
}
