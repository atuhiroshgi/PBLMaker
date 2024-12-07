using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static readonly string GROUND_TAG = "Ground";
    private static readonly string SPRING_TAG = "Spring";

    private bool isGround = false;      // 設置しているかどうか
    private bool isGroundEnter = false; // 地面に入っているかどうか
    private bool isGroundStay = false;  // 地面に入り続けているか
    private bool isGroundExit = false;  // 地面から出たか

    private bool isSpring = false;      // バネに乗っているか
    private bool isSpringEnter = false; // バネに入ったか
    private bool isSpringStay = false;  // バネに入り続けているか
    private bool isSpringExit = false;  // バネから出たか

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
            Debug.Log("きたあ");
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
    /// 接地しているかどうかのゲッター
    /// </summary>
    /// <returns>接地しているかどうか</returns>
    public bool GetIsGround()
    {
        return isGround;
    }

    /// <summary>
    /// バネに乗っているかどうかのゲッター
    /// </summary>
    /// <returns>バネに乗っているかどうか</returns>
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
