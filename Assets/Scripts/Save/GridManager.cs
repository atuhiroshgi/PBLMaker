using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class GridManager : MonoBehaviour
{
    [HideInInspector]
    public GridCell[,] gridCells;    // シーン内に配置されているセルを取得してセット

    private GridGenerator gridGenerator;

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

    private GridCell GetCellAtPosition(int x, int y)
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
