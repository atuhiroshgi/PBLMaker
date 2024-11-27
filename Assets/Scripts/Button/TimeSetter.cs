using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class TimeSetter : MonoBehaviour
{
    [SerializeField, Header("残り秒数を表示するテキスト")]
    private TextMeshProUGUI countDownText;

    [SerializeField, Header("カウントダウン時間(秒)")]
    private int countDownTime = 10;

    private void Start()
    {
        // カウントダウン処理を開始
        StartCountDownAsync().Forget();
    }

    private async UniTaskVoid StartCountDownAsync()
    {
        // 実行可能状態になるまで待機
        await UniTask.WaitUntil(() => ExecuteManager.Instance.GetIsExecute());

        // カウントダウンを開始
        await StartCountDown();
    }

    private async UniTask StartCountDown()
    {
        int remainingTime = countDownTime;

        while (remainingTime > 0)
        {
            // 残り時間をテキストに表示
            countDownText.text = $"{remainingTime}";

            // 1秒待機
            await UniTask.Delay(1000);

            // 残り時間を減らす
            remainingTime--;
        }

        // カウントダウン終了時の処理（必要に応じて追加）
        countDownText.text = "0";
    }
}
