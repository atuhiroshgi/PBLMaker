using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField, Header("プレハブを設定")]
    private GameObject prefab;
    [SerializeField, Header("ブロックのアイコン画像")]
    private Sprite blockSprite;
    [SerializeField, Header("ブロックに対応したblockIndex")]
    private int blockType;
    [SerializeField, Header("このデータがブロックかどうか")]
    private bool isBlock = true;

    // データにアクセスするためのプロパティ
    public GameObject Prefab => prefab;
    public Sprite BlockSprite => blockSprite;
    public int BlockType => blockType;
    public bool IsBlock => isBlock;
}
