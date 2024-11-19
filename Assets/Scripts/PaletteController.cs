using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    [SerializeField, Header("BlockPlacerを参照")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("BlockButtonを参照")]
    private BlockButton[] blockButtons; 
    [SerializeField, Header("BlockStorageDataを参照")]
    private BlockStorageData blockStorageData;
    [SerializeField, Header("パレットのアイコン画像")]
    private Image[] paletteIcons;
    [SerializeField, Header("アイコンそれぞれの背景")]
    private Image[] paletteIconBgs;

    private GameObject currentObject;           // 現在選択されているオブジェクト
    private Color selectedColor = Color.red;    // 選ばれている時の色
    private Color defaultColor = Color.gray;    // 選ばれていない時の色
    private int selectedBlockType = 0;          // 選ばれているブロックタイプ
    private int targetLineIndex = 0;            // 何行目を選択しているかのインデックス
    private bool selectedIsBlock = true;        // 選ばれているのがブロックかどうか

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// パレットのアイコンを初期化するメソッド
    /// </summary>
    private void Init()
    {
        // LineDataの取得
        LineData[] lineDatas = blockStorageData.LineDatas;

        if (lineDatas == null || lineDatas.Length <= targetLineIndex)
        {
            Debug.LogWarning("指定されたインデックスのLineDataは存在せえへんで");
            return;
        }

        // 対象のLineDataを取得
        LineData targetLineData = lineDatas[targetLineIndex];
        BlockData[] blockDataArray = targetLineData.BlockData;

        for (int i = 0; i < paletteIcons.Length; i++)
        {
            // ブロックデータの配列の範囲内なら表示して、画像を設定
            if (i < blockDataArray.Length && blockDataArray[i] != null)
            {
                BlockData blockData = blockDataArray[i];

                // アイコン画像を設定
                paletteIcons[i].sprite = blockData.BlockSprite;
                paletteIcons[i].enabled = true;
                paletteIconBgs[i].color = i == selectedBlockType ? selectedColor : defaultColor;
                blockButtons[i].SetBlockType(blockData.BlockType);

                // 選ばれているオブジェクトがブロックかどうかを判断して変数に格納
                if(i == selectedBlockType)
                {
                    selectedIsBlock = blockData.IsBlock;
                    if (!selectedIsBlock)
                    {
                        currentObject = blockData.Prefab;
                    }
                }
            }
            else
            {
                // 範囲外なら非表示
                paletteIconBgs[i].enabled = false;
                paletteIcons[i].enabled = false;
            }
        }
        
        // 選ばれているのがブロックかどうかで処理を変更
        if (selectedIsBlock)
        {
            // ブロックタイプをblockPlacerに通知
            blockPlacer.SetBlockType(selectedBlockType);
        }
        else
        {
            Debug.Log("あああ");
            // 設置するオブジェクトをBlockPlacerに渡す
            blockPlacer.SetObjectToPlace(currentObject);
        }
    }

    /// <summary>
    /// 選択されているブロックのタイプのセッター
    /// </summary>
    /// <param name="blockType">ブロックタイプ</param>
    public void SetSelectedBlockType(int blockType)
    {
        this.selectedBlockType = blockType;
        Init();
    }

    /// <summary>
    /// 選択されているオブジェクトがブロックかどうかのゲッター
    /// </summary>
    /// <returns>選択されているオブジェクトがブロックかどうか</returns>
    public bool GetSelectedIsBlock()
    {
        return selectedIsBlock;
    }
}
