using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField, Header("�u���b�N�̃A�C�R���摜")]
    private Sprite blockSprite;
    [SerializeField, Header("�u���b�N�ɑΉ�����blockIndex")]
    private int blockType;

    // �f�[�^�ɃA�N�Z�X���邽�߂̃v���p�e�B
    public Sprite BlockSprite => blockSprite;
    public int BlockType => blockType;
}
