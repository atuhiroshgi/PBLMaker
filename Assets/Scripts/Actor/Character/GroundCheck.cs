using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static string GROUND_TAG = "Ground";

    private bool isGround = false;      //�ݒu���Ă��邩�ǂ���
    private bool isGroundEnter = false; //�n�ʂɓ����Ă��邩�ǂ���
    private bool isGroundStay = false;  //�n�ʂɓ��葱���Ă��邩
    private bool isGroundExit = false;  //�n�ʂ���o���Ƃ��̃t���O

    /// <summary>
    /// �ڒn���Ă��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns>�ڒn���Ă��邩�ǂ���</returns>
    public bool GetIsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GROUND_TAG == collision.tag)
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(GROUND_TAG == collision.tag)
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(GROUND_TAG == collision.tag)
        {
            isGroundExit = true;
        }
    }
}
