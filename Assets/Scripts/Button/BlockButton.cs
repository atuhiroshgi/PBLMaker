using UnityEngine;

public class BlockButton : Button
{
    [SerializeField, Header("PaletteControllerを参照")]
    private PaletteController paletteController;
    [SerializeField, Header("アイコンが押されたときにBlockPlacerに設定するBlockType")]
    private int blockType;

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
