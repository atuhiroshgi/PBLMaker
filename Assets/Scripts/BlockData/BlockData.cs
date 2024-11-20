using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField, Header("�v���n�u��ݒ�")]
    private GameObject prefab;
    [SerializeField, Header("�u���b�N�̃A�C�R���摜")]
    private Sprite blockSprite;
    [SerializeField, Header("�u���b�N�ɑΉ�����blockIndex")]
    private int blockType;
    [SerializeField, Header("���̃f�[�^���u���b�N���ǂ���")]
    private bool isBlock = true;

    // �f�[�^�ɃA�N�Z�X���邽�߂̃v���p�e�B
    public GameObject Prefab => prefab;
    public Sprite BlockSprite => blockSprite;
    public int BlockType => blockType;
    public bool IsBlock => isBlock;
}
