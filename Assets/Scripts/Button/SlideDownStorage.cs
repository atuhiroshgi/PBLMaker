using UnityEngine;
using Cysharp.Threading.Tasks;

public class SlideDownStorage : Button
{
    [SerializeField, Header("BlockStorageを参照")]
    private GameObject blockStorage;
    [SerializeField, Header("スライドにかける時間")]
    private float slideDuration = 0.5f;

    private bool isSliding = false;

    protected override void OnClick()
    {
        base.OnClick();

        if (blockStorage != null && !isSliding)
        {
            // スライドを返す
            SlideToTargetPositionAsync(blockStorage, slideDuration).Forget();
        }
    }

    /// <summary>
    /// UniTaskで非同期にスライド処理を行う
    /// </summary>
    /// <param name="target">動かすオブジェクト</param>
    /// <param name="duration">スライドする秒数</param>
    /// <returns></returns>
    private async UniTaskVoid SlideToTargetPositionAsync(GameObject target, float duration)
    {
        isSliding = true; // スライド中フラグを立てる
        Vector3 startPosition = target.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 540, startPosition.z); // Y座標のみ0に設定
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Y座標のみを補間
            float newY = Mathf.Lerp(startPosition.y, targetPosition.y, elapsedTime / duration);
            target.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();  // 次のフレームまで待機
        }

        // 最後に位置を正確に設定
        target.transform.position = targetPosition;
        isSliding = false;
    }
}
