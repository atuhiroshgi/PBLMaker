using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static string GROUND_TAG = "Ground";

    private bool isGround = false;      //設置しているかどうか
    private bool isGroundEnter = false; //地面に入っているかどうか
    private bool isGroundStay = false;  //地面に入り続けているか
    private bool isGroundExit = false;  //地面から出たときのフラグ

    /// <summary>
    /// 接地しているかどうかのゲッター
    /// </summary>
    /// <returns>接地しているかどうか</returns>
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
