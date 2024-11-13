using UnityEngine;
using UnityEngine.UI;

public class EraserButton : Button
{
    [SerializeField, Header("BlockPlacerの参照")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("消しゴムモードがオフの時のアイコン")]
    private Sprite defaultSprite;
    [SerializeField, Header("消しゴムモードがオンの時のアイコン")]
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
