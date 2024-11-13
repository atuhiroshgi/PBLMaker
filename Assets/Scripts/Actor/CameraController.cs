using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("Player")]
    private Player player;
    [SerializeField, Header("プレイヤーの位置")]
    private Transform playerTransform;
    [SerializeField, Header("カメラが移動を始めるx軸の範囲")]
    private float xOffsetThreshold = 2f;
    [SerializeField, Header("カメラが移動を始めるy軸の範囲")]
    private float yOffsetThreshold = 1.5f;
    [SerializeField, Header("カメラの移動速度")]
    private float cameraSpeed = 20f;
    [SerializeField, Header("カメラのx軸の最小値")]
    private float minX = 0f;
    [SerializeField, Header("カメラのx軸の最大値")]
    private float maxX = 120f;
    [SerializeField, Header("カメラのy軸の最小値")]
    private float minY = 0f;
    [SerializeField, Header("カメラのy軸の最大値")]
    private float maxY = 35f;

    private Vector3 initialCameraPosition;       // カメラの初期位置
    private Vector3 startPosition;                  // 実行開始時のカメラの位置
    private float cameraFollowSpeed;                // カメラがプレイヤーを注視するときの追尾速度

    private void Awake()
    {
        // カメラの初期位置を取得
        initialCameraPosition = transform.position;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            FollowPlayer();
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 入力があればカメラを移動
        if (horizontalInput != 0 || verticalInput != 0)
        {
            // カメラの新しい位置を計算
            Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
            Vector3 newPosition = this.transform.position + moveDirection * cameraSpeed * Time.deltaTime;

            // X軸とY軸の移動範囲を制限
            if (newPosition.x < minX) newPosition.x = minX;
            if (newPosition.x > maxX) newPosition.x = maxX;
            if (newPosition.y < minY) newPosition.y = minY;
            if (newPosition.y > maxY) newPosition.y = maxY;

            // カメラを移動させる
            this.transform.position = newPosition;
        }
    }

    /// <summary>
    /// プレイヤーを追従する
    /// </summary>
    /// <returns></returns>
    private void FollowPlayer()
    {
        if (!ExecuteManager.Instance.GetIsExecute()) return;

        // プレイヤーとカメラの相対位置
        Vector3 playerOffsetFromCamera = playerTransform.position - transform.position;

        // カメラが移動を開始するかの判定
        float newCameraPosX = transform.position.x;
        float newCameraPosY = transform.position.y;

        cameraFollowSpeed = player.GetPlayerSpeed();

        // X軸方向の処理
        if (Mathf.Abs(playerOffsetFromCamera.x) > xOffsetThreshold)
        {
            newCameraPosX = playerTransform.position.x - (Mathf.Sign(playerOffsetFromCamera.x) * xOffsetThreshold);
        }

        // Y軸方向の処理
        if (Mathf.Abs(playerOffsetFromCamera.y) > yOffsetThreshold)
        {
            newCameraPosY = playerTransform.position.y - (Mathf.Sign(playerOffsetFromCamera.y) * yOffsetThreshold);
        }

        // X軸とY軸の移動範囲を制限
        newCameraPosX = Mathf.Clamp(newCameraPosX, minX, maxX);
        newCameraPosY = Mathf.Clamp(newCameraPosY, minY, maxY);

        // カメラの新しい位置に補間して移動
        Vector3 newCameraPosition = new Vector3(newCameraPosX, newCameraPosY, initialCameraPosition.z);
        transform.position = Vector3.Lerp(transform.position, newCameraPosition, cameraFollowSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 実行が開始されたときにカメラの位置を記憶
    /// </summary>
    public void RememberStartPosition()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// 実行した時点でのカメラの位置に戻す
    /// </summary>
    public void MoveToStartPosition()
    {
        this.transform.position = startPosition;
    }
}
