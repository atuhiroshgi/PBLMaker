using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

public class BlockButton : Button
{
    [SerializeField, Header("PaletteControllerを参照")]
    private PaletteController paletteController;
    [SerializeField, Header("EraserModeを参照")]
    private EraserButton eraserButton;
    [SerializeField, Header("斜線のオブジェクト")]
    private GameObject diagonalLine;
    [SerializeField, Header("アイコンが押されたときにBlockPlacerに設定するBlockType")]
    private int blockType;

    private Image thisObjectImage;

    private void Awake()
    {
        thisObjectImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(eraserButton != null)
        {
            // EraserButtonの状態を確認し、diagonalLineを表示/非表示に切り替える
            if (!eraserButton.GetIsEraserMode() || !thisObjectImage.IsActive())
            {
                diagonalLine.SetActive(false);  // EraserModeが有効なら表示
            }
            else
            {
                diagonalLine.SetActive(true); // EraserModeが無効なら非表示
            }
        }

    }

    protected override void OnClick()
    {
        // クリックされたら選択されたことをPaletteControllerに通知する
        paletteController.SetSelectedBlockType(this.blockType);
    }

    /// <summary>
    /// blockTypeのセッター
    /// </summary>
    /// <param name="blockType">アイコンにどのブロックタイプを設定するか</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }
}
