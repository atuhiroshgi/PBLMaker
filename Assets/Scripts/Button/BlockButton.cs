using UnityEngine;

public class BlockButton : Button
{
    [SerializeField, Header("アイコンが押されたときにBlockPlacerに設定するBlockType")]
    private int blockType;

    /// <summary>
    /// blockTypeのセッター
    /// </summary>
    /// <param name="blockType">アイコンにどのブロックタイプを設定するか</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }
}
