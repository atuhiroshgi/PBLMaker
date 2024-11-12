using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
public class BlockPlacer : MonoBehaviour
{
    [SerializeField, Header("どのレイヤーのGridCellをターゲットにするか")]
    private LayerMask gridCellLayer;
    [SerializeField, Header("配置するブロックタイプ")]
    private int blockType = 0;

    private void Start()
    {
        // マウスクリックを監視する非同期タスクを開始
        MonitorMouseClickAsync().Forget();
    }

    /// <summary>
    /// マウスクリックを監視し、クリックや長押し時にブロックを配置する非同期処理
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid MonitorMouseClickAsync()
    {
        while (true)
        {
            // クリックがあるまで待機
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

            // クリックや長押し中は、ブロック配置を続行
            await PlaceBlocksWhileMouseHeld();
        }
    }

    /// <summary>
    /// マウスが押されている間、ブロックを配置する
    /// </summary>
    private async UniTask PlaceBlocksWhileMouseHeld()
    {
        while (Input.GetMouseButton(0))
        {
            // マウス直下のGridCellを探して、SetBlockTypeメソッドを呼び出す
            PlaceBlockUnderMouse();

            // 入力が続く限り、一定間隔ごとに次のフレームまで待機
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// マウス直下のGridCellに対してSetBlockTypeメソッドを呼び出す
    /// </summary>
    private void PlaceBlockUnderMouse()
    {
        // マウスの位置をワールド座標に変換
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2Dレイキャストで、指定レイヤー上のコライダーを探す
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, gridCellLayer);

        // GridCellコンポーネントを取得
        if (hit.collider != null)
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();
            if (cell != null)
            {
                cell.SetBlockType(blockType);
            }
        }
    }
}

