using UnityEngine;

public class BlockButton : Button
{
    [SerializeField, Header("�A�C�R���������ꂽ�Ƃ���BlockPlacer�ɐݒ肷��BlockType")]
    private int blockType;

    /// <summary>
    /// blockType�̃Z�b�^�[
    /// </summary>
    /// <param name="blockType">�A�C�R���ɂǂ̃u���b�N�^�C�v��ݒ肷�邩</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }
}
