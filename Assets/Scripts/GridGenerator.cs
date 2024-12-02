using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField, Header("配置するGridCellのプレハブ")]
    private GameObject gridCellPrefab;

    [SerializeField, Header("横方向のセルの数")]
    private int width = 5;

    [SerializeField, Header("縦方向のセルの数")]
    private int height = 5;

    [SerializeField, Header("セル同士の間隔")]
    private float cellSpacing = 1.28f;

    private GridCell[,] gridCells;  // 各セルの参照を保持する2次元配列


    /// <summary>
    /// グリッドを生成するメソッド
    /// </summary>
    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// 指定された幅と高さのグリッドを生成
    /// </summary>
    public void GenerateGrid()
    {
        // グリッドサイズに基づいて2次元配列を初期化
        gridCells = new GridCell[width, height];

        // 画面の左下のワールド座標を取得
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeft.x += cellSpacing / 2;
        bottomLeft.y += cellSpacing / 2;
        bottomLeft.z = 0;  // Z軸の位置を0に設定

        // 指定した幅と高さに応じてセルを生成し、並べる
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // セルの位置を計算
                Vector3 cellPosition = bottomLeft + new Vector3(x * cellSpacing, y * cellSpacing, 0);

                // プレハブをインスタンス化
                GameObject cellObj = Instantiate(gridCellPrefab, cellPosition, Quaternion.identity, transform);

                // GridCellコンポーネントを取得
                GridCell cell = cellObj.GetComponent<GridCell>();

                cell.SetX(x);
                cell.SetY(y);

                // 位置情報を設定（セルのインスペクター上で確認できる）
                cellObj.name = $"GridCell ({x}, {y})";

                // Gridに色を付ける
                if(x <= 3 && y == 2)
                {
                    cell.SetBlockType(0);
                }
                else if(x <= 3 && y < 2)
                {
                    cell.SetBlockType(1);
                }
                else
                {
                    cell.SetBlockType(-1);
                }

                // グリッド配列に格納
                gridCells[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// 任意のセルにブロックタイプを設定
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="blockType">設定するブロックタイプ</param>
    public void SetBlockTypeAt(int x, int y, int blockType)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridCells[x, y].SetBlockType(blockType);
        }
        else
        {
            Debug.LogWarning("指定されたセルの位置が範囲外です");
        }
    }

    /// <summary>
    /// 全てのGridCellクラスがアタッチされたオブジェクトを削除
    /// </summary>
    public void ClearAllGridCells()
    {
        // 配列を順に確認して全てのGridCellを削除
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (gridCells[x, y] != null)
                {
                    Destroy(gridCells[x, y].gameObject);
                    gridCells[x, y] = null;
                }
            }
        }
    }

    public GridCell[,] GetGridCells()
    {
        return gridCells;
    }
}