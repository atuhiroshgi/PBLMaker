using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [SerializeField, Header("壁として認識するレイヤー")]
    private LayerMask wallLayer;

    private bool isHittingWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 壁レイヤーに属するオブジェクトに衝突した場合
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            isHittingWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 壁レイヤーに属するオブジェクトから離れた場合
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
