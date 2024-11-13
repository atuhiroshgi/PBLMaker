using UnityEngine;
using UnityEngine.UI;

public class EraserButton : Button
{
    [SerializeField, Header("BlockPlacer�̎Q��")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("�����S�����[�h���I�t�̎��̃A�C�R��")]
    private Sprite defaultSprite;
    [SerializeField, Header("�����S�����[�h���I���̎��̃A�C�R��")]
    private Sprite eraserModeSprite;

    private Image eraserButtonImage;
    private bool isEraserMode;

    private void Awake()
    {
        eraserButtonImage = GetComponent<Image>();
    }

    private void Update()
    {
        eraserButtonImage.enabled = ExecuteManager.Instance.GetIsExecute() ? false : true;
    }

    protected override void OnClick()
    {
        base.OnClick();
        isEraserMode = !isEraserMode;

        if (isEraserMode) blockPlacer.SetBlockType(-1);
        eraserButtonImage.sprite = isEraserMode ? eraserModeSprite : defaultSprite;

    }
}
