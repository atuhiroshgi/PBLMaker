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

    private Color selectedColor = Color.yellow;
    private Color defaultColor = Color.gray;
    private int selectedBlockType = 0;
    private int targetLineIndex = 0;

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
            }
            else
            {
                // 範囲外なら非表示
                paletteIconBgs[i].enabled = false;
                paletteIcons[i].enabled = false;
            }
        }

        // ブロックタイプをblockPlacerに通知
        blockPlacer.SetBlockType(selectedBlockType);
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
}
