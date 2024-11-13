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
    [SerializeField, Header("重なっている間ブロックを置いてほしくないUI")]
    private GameObject[] uiObjects;

    private async void Update()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
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
            //Buttonクラスに重なっているときはブロックを置かない
            Player player = hit.collider.gameObject.GetComponent<Player>();
            Button button = hit.collider.GetComponent<Button>();
            if(button != null)
            {
                return;
            }

            GridCell cell = hit.collider.GetComponent<GridCell>();
            if (cell != null)
            {
                cell.SetBlockType(blockType);
            }
        }
    }

    /// <summary>
    /// マウスポインタがUI要素に重なっているかどうかを確認するメソッド
    /// </summary>
    /// <returns>重なっているか</returns>
    private bool IsPointerOverUIObject()
    {
        // Raycastを使ってUIを検出
        UnityEngine.EventSystems.PointerEventData eventDataCurrentPosition = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // UIオブジェクト配列に含まれているかをチェック
        foreach (var result in results)
        {
            foreach (var uiObject in uiObjects)
            {
                if (result.gameObject == uiObject)
                {
                    //重なっているオブジェクトが見つかった
                    return true;
                }
            }
        }
        //重なっているオブジェクトが見つからなかった
        return false;
    }
}

