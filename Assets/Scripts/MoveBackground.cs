using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField, Header("背景画像の参照")]
    private GameObject[] bgObject;

    [SerializeField, Header("カメラの参照")]
    private Camera mainCamera;

    private float bgSkySpeed = 23f;          // 背景の空のスピード
    private float bgCloudSpeed = 25f;        // 背景の雲のスピード
    private float bgBackMountain = 30f;     // 背景の裏の山のスピード
    private float bgFrontMountain = 35f;    // 背景の表の山のスピード

    private float loopWidth = 34f;          // ループさせる幅

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

            for (int i = 0; i < bgObject.Length; i++)
            {
                float speed = GetSpeed(i);
                bgObject[i].transform.position = CalcPosition(moveDirection, bgObject[i], speed);
            }
        }
    }

    private float GetSpeed(int index)
    {
        return index switch
        {
            0 => bgSkySpeed,
            1 => bgCloudSpeed,
            2 => bgBackMountain,
            3 => bgFrontMountain,
            _ => 0f,
        };
    }

    private Vector3 CalcPosition(Vector3 moveDirection, GameObject gameObject, float speed)
    {
        Vector3 newPosition = gameObject.transform.position + moveDirection * speed * Time.deltaTime;

        // カメラのX座標を基準にオフセットを計算
        float cameraX = mainCamera.transform.position.x;
        float objectX = newPosition.x;

        if (objectX < cameraX - loopWidth / 2)
        {
            newPosition.x += loopWidth;
        }
        else if (objectX > cameraX + loopWidth / 2)
        {
            newPosition.x -= loopWidth;
        }

        return newPosition;
    }
}
