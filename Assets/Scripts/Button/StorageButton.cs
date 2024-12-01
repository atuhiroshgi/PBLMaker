using UnityEngine;

public class StorageButton : Button
{
    [SerializeField, Header("PaletteController�̎Q��")]
    private PaletteController paletteController;
    [SerializeField, Header("CloseButton�̎Q��")]
    private CloseButton closeButton;
    [SerializeField, Header("���ꂪ���s�ڂɊ܂܂�Ă��邩�̃C���f�b�N�X")]
    private int lineIndex;

    protected override void OnClick()
    {
        base.OnClick();
        paletteController.GetSelectedLineIndex(lineIndex);
        closeButton.SlideInput();
    }
}
