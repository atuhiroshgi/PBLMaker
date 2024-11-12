using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
public class BlockPlacer : MonoBehaviour
{
    [SerializeField, Header("どのレイヤーのGridCellをターゲットにするか")]
    private LayerMask gridCellLayer;
    [SerializeField, Header("配置するブロックタイプ")]
    private int blockType = 0;

    private void Start()
    {
        //マウスクリックを監視する非同期タスクを開始
        WaitForMouseClickAsync().Forget();
    }

    /// <summary>
    /// マウスがクリックされるまで待機し、クリックされたら直下のGridCellにブロックを配置する
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitForMouseClickAsync()
    {
        while (true)
        {
            //左クリックが押されるまで待機
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

            while (Input.GetMouseButton(0))
            {
                //マウス直下のGridCellを探して、SetBlockTypeメソッドを呼び出す
                PlaceBlockUnderMouse();

                //次のフレームまで待機
                await UniTask.Yield();
            }
        }
    }

    /// <summary>
    /// マウス直下のGridCellに対してSetBlockTypeメソッドを呼び出す
    /// </summary>
    private void PlaceBlockUnderMouse()
    {
        //マウスの位置をワールド座標に変換
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //2Dレイキャストで、指定レイヤー上のコライダーを探す
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, gridCellLayer);

        //GridCellコンポーネントを取得
        if(hit.collider != null)
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();

            if (cell == null) return;
            cell.SetBlockType(blockType);
        }
    }
}
