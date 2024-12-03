using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject();
                go.name = "GridManager";
                instance = go.AddComponent<GridManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (!ReferenceEquals(instance, this)) // 参照比較を使用
        {
            Destroy(gameObject);
        }
    }

    private static readonly string INITIALSTAGE_FILE_PATH = "initialGridData.json";

    [HideInInspector]
    public GridCell[,] gridCells;    // シーン内に配置されているセルを取得してセット

    private GridGenerator gridGenerator;
    private float maxTimeDifference = 0.2f; // 同時押しとみなす最大時間差
    private bool saveLogTriggered = false;  // 保存用フラグ
    private bool loadLogTriggered = false;  // 読み込み用フラグ

    // キーとタイムスタンプを保持
    private Dictionary<KeyCode, float> saveKeyPressTimes = new Dictionary<KeyCode, float>
    {
        { KeyCode.I, -1 },
        { KeyCode.P, -1 }
    };
    private Dictionary<KeyCode, float> loadKeyPressTimes = new Dictionary<KeyCode, float>
    {
        { KeyCode.O, -1 },
        { KeyCode.P, -1 }
    };


    private void Start()
    {
        gridGenerator = GetComponent<GridGenerator>();

        if (gridGenerator != null)
        {
            gridCells = gridGenerator.GetGridCells();
            Debug.Log($"gridCells が初期化されました: {gridCells != null}");
        }
        else
        {
            Debug.LogError("GridGeneratorが見つかりません！");
        }
    }


    void Update()
    {
        // 保存操作用のキーのチェック
        UpdateKeyPressTimes(saveKeyPressTimes);

        // 読み込み操作用のキーのチェック
        UpdateKeyPressTimes(loadKeyPressTimes);

        // I + P の同時押しで保存
        if (AreKeysPressedSimultaneously(saveKeyPressTimes) && !saveLogTriggered)
        {
            SaveGridData(INITIALSTAGE_FILE_PATH);
            saveLogTriggered = true;
        }
        else if (!AreKeysPressedSimultaneously(saveKeyPressTimes))
        {
            saveLogTriggered = false; // 条件が解除されたらフラグをリセット
        }

        // O + P の同時押しで読み込み
        if (AreKeysPressedSimultaneously(loadKeyPressTimes) && !loadLogTriggered)
        {
            LoadGridData(INITIALSTAGE_FILE_PATH);
            loadLogTriggered = true;
        }
        else if (!AreKeysPressedSimultaneously(loadKeyPressTimes))
        {
            loadLogTriggered = false; // 条件が解除されたらフラグをリセット
        }
    }

    private void UpdateKeyPressTimes(Dictionary<KeyCode, float> keyPressTimes)
    {
        var keys = keyPressTimes.Keys.ToList();

        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                keyPressTimes[key] = Time.time;
            }
        }
    }

    private bool AreKeysPressedSimultaneously(Dictionary<KeyCode, float> keyPressTimes)
    {
        float firstPressTime = -1;
        foreach (var keyTime in keyPressTimes.Values)
        {
            if (keyTime < 0) return false; // 押されていないキーがある場合は失敗

            if (firstPressTime < 0)
            {
                firstPressTime = keyTime;
            }
            else if (Mathf.Abs(firstPressTime - keyTime) > maxTimeDifference)
            {
                return false; // 時間差が大きすぎる場合は失敗
            }
        }
        return true;
    }

    public void ReloadGridCells()
    {
        gridCells = gridGenerator.GetGridCells();
    }

    public void SaveGridData(string filePath)
    {
        GridData gridData = new GridData();

        // gridCells を二重ループで回す
        for (int x = 0; x < gridCells.GetLength(0); x++)
        {
            for (int y = 0; y < gridCells.GetLength(1); y++)
            {
                GridCell cell = gridCells[x, y];
                if (cell.GetBlockType() != -1 || cell.GetIsGoal() || cell.GetPlacedObject() != null)
                {
                    string placedObjectName = null;

                    if (cell.GetPlacedObject() != null)
                    {
#if UNITY_EDITOR
                        var prefab = PrefabUtility.GetCorrespondingObjectFromSource(cell.GetPlacedObject());
                        placedObjectName = prefab != null ? prefab.name : cell.GetPlacedObject().name;
#else
                    placedObjectName = cell.GetPlacedObject().name.Replace("(Clone)", "").Trim();
#endif
                    }

                    CellData cellData = new CellData
                    {
                        xGrid = cell.GetX(),
                        yGrid = cell.GetY(),
                        blockType = cell.GetBlockType(),
                        isGoal = cell.GetIsGoal(),
                        placedObjectName = placedObjectName
                    };
                    gridData.cells.Add(cellData);
                }
            }
        }

        string json = JsonUtility.ToJson(gridData, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadGridData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("ファイルが見つかりません!");
            return;
        }

        string json = File.ReadAllText(filePath);
        GridData gridData = JsonUtility.FromJson<GridData>(json);

        foreach (CellData cellData in gridData.cells)
        {
            GridCell cell = GetCellAtPosition(cellData.xGrid, cellData.yGrid);
            if (cell != null)
            {
                cell.SetBlockType(cellData.blockType);
                cell.SetIsGoal(cellData.isGoal);

                if (!string.IsNullOrEmpty(cellData.placedObjectName))
                {
                    GameObject prefab = Resources.Load<GameObject>(cellData.placedObjectName);

                    if (prefab == null)
                    {
                        // セーブデータに (Clone) が含まれている場合の対応
                        string adjustedName = cellData.placedObjectName.Replace("(Clone)", "").Trim();
                        prefab = Resources.Load<GameObject>(adjustedName);
                    }

                    if (prefab != null)
                    {
                        cell.SetPlacedObject(prefab);
                    }
                    else
                    {
                        Debug.LogError($"プレハブ '{cellData.placedObjectName}' が見つかりません");
                    }
                }
            }
        }
    }

    public GridCell GetCellAtPosition(int x, int y)
    {
        if (gridCells == null)
        {
            Debug.LogError("gridCells が初期化されていません。");
            return null;
        }

        foreach (var cell in gridCells)
        {
            if (cell != null && cell.GetX() == x && cell.GetY() == y)
                return cell;
        }
        return null; // 見つからない場合はnullを返す
    }
}

[System.Serializable]
public class CellData
{
    public int xGrid;
    public int yGrid;
    public int blockType;
    public bool isGoal;
    public string placedObjectName;
}

[System.Serializable]
public class GridData
{
    public List<CellData> cells = new List<CellData>();
}
