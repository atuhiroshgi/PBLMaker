using Cysharp.Threading.Tasks;
using UnityEngine;

public class GridCell : Actor
{
    [SerializeField, Header("このセルが左から何マス目か")]
    private int xGrid;
    [SerializeField, Header("このセルが下から何マス目か")]
    private int yGrid;
    [SerializeField, Header("このセルにどのブロックが置かれているか")]
    private int blockType = -1;

    [SerializeField, Header("ゴールマーカーオブジェクト")]
    private GameObject goalMarker;
    [SerializeField, Header("空の時のセルの見た目")]
    private Sprite emptySprite;
    [SerializeField, Header("空で実行された時の見た目")]
    private Sprite executeEmptySprite;
    [SerializeField, Header("ブロックの見た目を変更するための画像の配列")]
    private Sprite[] cellSprites;

    private Collider2D col;
    private GameObject placedObject;        // このセルの上に存在するオブジェクトを格納するためのメンバ
    private SpriteRenderer spriteRenderer;  // このセルの見た目を変更するため
    private bool isGoal = false;

    private async void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(goalMarker != null) goalMarker.SetActive(false);

        while (true)
        {
            await UniTask.WaitUntil(() => !ExecuteManager.Instance.GetIsExecute());
            if (blockType == -1) spriteRenderer.sprite = emptySprite;
            await UniTask.WaitUntil(() => ExecuteManager.Instance.GetIsExecute());
            if (blockType == -1) spriteRenderer.sprite = null;
        }
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            goalMarker.SetActive(false);
            return;
        }
        if (goalMarker != null)
        {
            goalMarker.SetActive(isGoal);
        }
    }

    /// <summary>
    /// 他のクラスからのセルの書き換えがあった時に呼ばれるメソッド
    /// </summary>
    /// <param name="newBlockType"></param>
    public void SetBlockType(int newBlockType)
    {
        blockType = newBlockType;
        UpdateAppearance();
    }

    /// <summary>
    /// 設置したブロックタイプが前のと違うかどうか
    /// </summary>
    /// <param name="blockType">設置するブロックタイプ</param>
    /// <returns>違うならtrue, 同じならfalse</returns>
    public bool canPlaySEBlock(int blockType)
    {
        return this.blockType != blockType;
    }

    /// <summary>
    /// ブロックタイプに応じた見た目の設定
    /// </summary>
    private void UpdateAppearance()
    {
        if (spriteRenderer == null) return;

        // ブロックに応じたSpriteを設定
        spriteRenderer.sprite = blockType == -1 ? emptySprite : cellSprites[blockType];
        // 色と透明度を設定
        spriteRenderer.color = blockType == -1 ? new Color(0, 0, 0, 120f / 255f) : Color.white;
        // コライダーのトリガー設定
        col.isTrigger = blockType == -1;

        this.gameObject.tag = blockType == -1 ? "Untagged" : "Ground";
    }

    /// <summary>
    /// セル上に配置するオブジェクトを設定するメソッド
    /// </summary>
    /// <param name="newObjectPrefab"></param>
    public void SetPlacedObject(GameObject newObjectPrefab)
    {
        // 既に配置済みのオブジェクトがあれば削除
        if(placedObject != null)
        {
            Destroy(placedObject);
        }

        // nullじゃなければ設置する
        if(newObjectPrefab != null)
        {
            placedObject = Instantiate(newObjectPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {

        }
    }

    /// <summary>
    /// 設置したオブジェクトが前のと違うかどうか
    /// </summary>
    /// <param name="placeObject">設置するオブジェクト</param>
    /// <returns>違うならtrue, 同じならfalse</returns>
    public bool canPlaySEObject(GameObject placeObject)
    {
        return placedObject != placeObject;
    }

    /// <summary>
    /// 現在のセルの上にあるセルを検索し、条件に基づいて更新する
    /// </summary>
    public void UpdateUpperCells()
    {
        // GridManagerを使用して現在のセルの上（Y座標が1つ大きいセル）を取得
        GridCell upperCell = GridManager.Instance.GetCellAtPosition(this.xGrid, this.yGrid + 1);

        // 上のセルが存在し、BlockTypeが0である場合
        if (upperCell != null && (upperCell.GetBlockType() == 0 || upperCell.GetBlockType() == 1))
        {
            // 上のセルのBlockTypeが0なら、現在のセルのBlockTypeを1に設定
            SetBlockType(1);
        }

        // GridManagerを使用して現在のセルの下（Y座標が1つ小さいセル）を取得
        GridCell lowerCell = GridManager.Instance.GetCellAtPosition(this.xGrid, this.yGrid - 1);

        // 下のセルが存在し、BlockTypeが0である場合
        if (lowerCell != null && lowerCell.GetBlockType() == 0)
        {
            // 下のセルのBlockTypeを1に設定
            lowerCell.SetBlockType(1);
        }
    }


    /// <summary>
    /// ゴールに設定されているかのセッター
    /// </summary>
    /// <param name="isGoal">ゴールに設定されているか</param>
    public void SetIsGoal(bool isGoal)
    {
        this.isGoal = isGoal;
    }

    /// <summary>
    /// x座標のセッター
    /// </summary>
    /// <param name="x"></param>
    public void SetX(int x)
    {
        this.xGrid = x;
    }

    /// <summary>
    /// y座標のセッター
    /// </summary>
    /// <param name="y"></param>
    public void SetY(int y)
    {
        this.yGrid = y;
    }

    /// <summary>
    /// x座標のゲッター
    /// </summary>
    /// <returns>セルのx座標(セル数カウント)</returns>
    public int GetX()
    {
        return xGrid;
    }

    /// <summary>
    /// y座標のゲッター
    /// </summary>
    /// <returns>セルのy座標(セル数カウント)</returns>
    public int GetY()
    {
        return yGrid;
    }

    public int GetBlockType()
    {
        return blockType;
    }

    public bool GetIsGoal()
    {
        return isGoal;
    }

    public GameObject GetPlacedObject()
    {
        return placedObject;
    }
}
