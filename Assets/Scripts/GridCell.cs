using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField, Header("このセルが左から何マス目か")]
    private int xGrid;
    [SerializeField, Header("このセルが下から何マス目か")]
    private int yGrid;
    [SerializeField, Header("このセルにどのブロックが置かれているか")]
    private int blockType = -1;
    
    [SerializeField, Header("空の時のセルの見た目")]
    private Sprite emptySprite;
    [SerializeField, Header("ブロックの見た目を変更するための画像の配列")]
    private Sprite[] cellSprites;

    private Collider2D col;
    private GameObject placedObject;        // このセルの上に存在するオブジェクトを格納するためのメンバ
    private SpriteRenderer spriteRenderer;  // このセルの見た目を変更するため

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
}
