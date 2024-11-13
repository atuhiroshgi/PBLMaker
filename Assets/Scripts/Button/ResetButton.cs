using UnityEngine;

public class ResetButton : Button
{
    [SerializeField, Header("GridGeneratorを参照")]
    private GridGenerator gridGenerator = null;

    private bool isPressing = false;            // ボタンが押されているかどうか
    private float pressTime = 0f;               // ボタンが押されている時間
    private const float requiredHoldTime = 3f;  // 3秒間押下が必要

    protected override void OnPointerDown()
    {
        base.OnPointerDown();
        isPressing = true;
        pressTime = 0f;     // タイマーをリセット
    }

    protected override void OnPointerUp()
    {
        base.OnPointerUp();
        isPressing = false;
        pressTime = 0f;     // タイマーをリセット
    }

    private void Update()
    {
        // ボタンが押されている間、時間を計測
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            // 押下時間が指定された時間に達したらOnClick処理を実行
            if (pressTime >= requiredHoldTime)
            {
                ResetGridCell();
                isPressing = false;     // 処理を一度だけ行うためフラグをオフに
            }
        }
    }

    /// <summary>
    /// 作られたマップを全て消して再生成する
    /// </summary>
    private void ResetGridCell()
    {
        gridGenerator.ClearAllGridCells();
        gridGenerator.GenerateGrid();
    }
}
