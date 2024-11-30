using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NUnit.Framework;
using System.Collections.Generic;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField, Header("PaletteControllerの参照")]
    private PaletteController paletteController;
    [SerializeField, Header("どのレイヤーのGridCellをターゲットにするか")]
    private LayerMask gridCellLayer;
    [SerializeField, Header("配置するブロックタイプ")]
    private int blockType = 0;
    [SerializeField, Header("重なっている間ブロックを置いてほしくないUI")]
    private GameObject[] uiObjects;
    [SerializeField, Header("重なっている間ブロックを置いてほしくないゲームオブジェクト")]
    private List<GameObject> blockerGameObjects;

    [Header("効果音の設定")]
    [SerializeField] private AudioClip placeBlockSound;     // ブロック配置時の効果音
    [SerializeField] private AudioClip placeObjectSound;    // オブジェクト配置時の効果音
    [SerializeField] private AudioClip deleteBlockSound;    // ブロック削除時の効果音

    private GameObject objectToPlace;
    private AudioSource audioSource;
    private bool isEraserMode = false;
    private bool isGoalSettingMode = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private async void Update()
    {
        if (IsPointerOverUIObject() || IsPointerOverBlockerObject() || ExecuteManager.Instance.GetIsExecute()) return;

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
            PlaceObjectUnderMouse();

            // 入力が続く限り、一定間隔ごとに次のフレームまで待機
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// マウス直下のGridCellに対してSetBlockTypeメソッドを呼び出す
    /// </summary>
    private void PlaceObjectUnderMouse()
    {
        // マウスの位置をワールド座標に変換
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2Dレイキャストで、指定レイヤー上のコライダーを探す
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, gridCellLayer);

        // GridCellコンポーネントを取得
        if (hit.collider != null)
        {
            if(isEraserMode && isGoalSettingMode)
            {
                RemoveGoalComponent(hit);
            }
            else if (isEraserMode)
            {
                DeleteObject(hit);
            }
            else if (isGoalSettingMode)
            {
                AddGoalScript(hit);
            }
            else
            {
                if(paletteController.GetSelectedIsBlock()) PlaceBlock(hit);
                if(!paletteController.GetSelectedIsBlock()) PlaceObject(hit);
            }
        }
    }

    /// <summary>
    /// ブロックを設置する用のメソッド
    /// </summary>
    /// <param name="hit">指定レイヤー上のコライダーの位置</param>
    private void PlaceBlock(RaycastHit2D hit)
    {
        // Buttonクラスに重なっているときはブロックを置かない
        Player player = hit.collider.gameObject.GetComponent<Player>();
        Button button = hit.collider.GetComponent<Button>();
        if (button != null)
        {
            return;
        }

        GridCell cell = hit.collider.GetComponent<GridCell>();
        if (cell != null)
        {
            // ブロック配置時の効果音を再生
            if(cell.canPlaySEBlock(blockType) && placeBlockSound != null)
            {
                audioSource.PlayOneShot(placeBlockSound);
            }

            // ブロックタイプを設定
            cell.SetBlockType(blockType);

            // GridCellのレイヤーを"Wall"に変更
            cell.gameObject.layer = blockType == -1 ? LayerMask.NameToLayer("Grid") : LayerMask.NameToLayer("Wall");

            if(blockType != -1)
            {
                cell.gameObject.tag = "Ground";
            }

        }
    }

    /// <summary>
    /// オブジェクトを設置する用のメソッド
    /// </summary>
    /// <param name="hit">指定レイヤー上のコライダーの位置</param>
    private void PlaceObject(RaycastHit2D hit)
    {
        GridCell cell = hit.collider.GetComponent<GridCell>();
        if(cell != null)
        {
            // オブジェクト配置時の効果音を再生
            if (cell.canPlaySEObject(objectToPlace) && placeObjectSound != null)
            {
                audioSource.PlayOneShot(placeObjectSound);
            }

            cell.SetPlacedObject(null);
            cell.SetPlacedObject(objectToPlace);

        }
    }

    /// <summary>
    /// オブジェクトを消す
    /// </summary>
    /// <param name="hit">指定レイヤー上のコライダーの位置</param>
    private void DeleteObject(RaycastHit2D hit)
    {
        // Buttonクラスに重なっているときはブロックを置かない
        Player player = hit.collider.gameObject.GetComponent<Player>();
        Button button = hit.collider.GetComponent<Button>();
        GridCell cell = hit.collider.GetComponent<GridCell>();

        if (button != null)
        {
            return;
        }

        if (cell != null)
        {
            // ブロック配置時の効果音を再生
            if (cell.canPlaySEBlock(-1) && placeBlockSound != null)
            {
                audioSource.PlayOneShot(deleteBlockSound);
            }

            // ブロックタイプを設定
            cell.SetBlockType(-1);
            cell.gameObject.layer = LayerMask.NameToLayer("Grid");
            cell.gameObject.tag = "Grid";

            RemoveGoalComponent(hit);

            // nullを置く(消す)
            cell.SetPlacedObject(null);
        }
    }

    /// <summary>
    /// Goalスクリプトを削除するメソッド
    /// </summary>
    /// <param name="hit">指定レイヤー上のコライダーの位置</param>
    private void RemoveGoalComponent(RaycastHit2D hit)
    {
        var targetObject = hit.collider.gameObject;

        if (targetObject != null)
        {
            Goal goalComponent = targetObject.GetComponent<Goal>();
            if (goalComponent != null)
            {
                Destroy(goalComponent);
                hit.collider.gameObject.GetComponent<GridCell>().SetIsGoal(false);
                Debug.Log($"Goal script removed from {targetObject.name}");
            }
        }
    }

    /// <summary>
    /// Goalスクリプトを追加するメソッド
    /// </summary>
    /// <param name="hit"></param>
    private void AddGoalScript(RaycastHit2D hit)
    {
        // Goalスクリプトを追加
        var targetObject = hit.collider.gameObject;
        if(targetObject != null && targetObject.GetComponent<Goal>() == null)
        {
            targetObject.AddComponent<Goal>();
            targetObject.GetComponent<GridCell>().SetIsGoal(true);
            Debug.Log($"Goal script added to {targetObject.name}");
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

    /// <summary>
    /// マウスポインタがBlockerオブジェクトに重なっているかを確認するメソッド
    /// </summary>
    /// <returns>重なっているか</returns>
    private bool IsPointerOverBlockerObject()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);

        if(hit != null)
        {
            foreach(var blocker in blockerGameObjects)
            {
                if(hit.gameObject == blocker)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// ブロックタイプのセッター
    /// </summary>
    /// <param name="blockType">ブロックタイプ</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }

    /// <summary>
    /// 設置するオブジェクトのセッター
    /// </summary>
    /// <param name="objectToPlace">設置するオブジェクト</param>
    public void SetObjectToPlace(GameObject objectToPlace)
    {
        this.objectToPlace = objectToPlace;
    }

    /// <summary>
    /// 消しゴムモードかどうかのフラグのセッター
    /// </summary>
    /// <param name="isEraserMode">消しゴムモードかどうか</param>
    public void SetIsEraserMode(bool isEraserMode)
    {
        this.isEraserMode = isEraserMode;
    }

    /// <summary>
    /// ゴール設定モードかどうかのフラグのセッター
    /// </summary>
    /// <param name="isGoalSettingMode">ゴール設定モードかどうか</param>
    public void SetIsGoalSettingMode(bool isGoalSettingMode)
    {
        this.isGoalSettingMode = isGoalSettingMode;
    }
}

