using UnityEngine;

public class BlockButton : Button
{
    [SerializeField, Header("PaletteController���Q��")]
    private PaletteController paletteController;
    [SerializeField, Header("�A�C�R���������ꂽ�Ƃ���BlockPlacer�ɐݒ肷��BlockType")]
    private int blockType;

    protected override void OnClick()
    {
        // �N���b�N���ꂽ��I�����ꂽ���Ƃ�PaletteController�ɒʒm����
        paletteController.SetSelectedBlockType(this.blockType);
    }

    /// <summary>
    /// blockType�̃Z�b�^�[
    /// </summary>
    /// <param name="blockType">�A�C�R���ɂǂ̃u���b�N�^�C�v��ݒ肷�邩</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }
}
