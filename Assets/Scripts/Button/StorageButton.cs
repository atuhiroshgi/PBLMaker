using UnityEngine;

public class StorageButton : Button
{
    [SerializeField, Header("PaletteControllerの参照")]
    private PaletteController paletteController;
    [SerializeField, Header("CloseButtonの参照")]
    private CloseButton closeButton;
    [SerializeField, Header("これが何行目に含まれているかのインデックス")]
    private int lineIndex;

    protected override void OnClick()
    {
        base.OnClick();
        paletteController.GetSelectedLineIndex(lineIndex);
        closeButton.SlideInput();
    }
}
