using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField, Header("ブロックのアイコン画像")]
    private Sprite blockSprite;
    [SerializeField, Header("ブロックに対応したblockIndex")]
    private int blockType;

    // データにアクセスするためのプロパティ
    public Sprite BlockSprite => blockSprite;
    public int BlockType => blockType;
}
