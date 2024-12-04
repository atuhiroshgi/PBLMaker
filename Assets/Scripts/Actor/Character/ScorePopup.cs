using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField, Header("上昇速度")]
    private float riseSpeed = 1.0f;
    [SerializeField, Header("フェードアウト時間")]
    private float fadeOutTime = 0.5f;

    private float duration;
    private TextMeshPro text;

    private void Awake()
    {
    }

    public void Initialize(int score, float displayDuration)
    {
        duration = displayDuration;
        text = GetComponent<TextMeshPro>();

        // スコアを表示
        if(text != null)
        {
            text.text = score.ToString();
        }

        // 始点を上にオフセット
        transform.position += Vector3.up;

        // UniTaskで非同期処理を開始
        HandlePopupLifecycleAsync().Forget();
    }

    private async UniTaskVoid HandlePopupLifecycleAsync()
    {
        float elapsedTime = 0f;

        // 表示の持続時間中ループ
        while (elapsedTime < duration)
        {
            // 上昇処理
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        //オブジェクトを破棄
        Destroy(gameObject);
    }
}
