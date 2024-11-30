using Unity.VisualScripting;
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

        blockPlacer.SetIsEraserMode(isEraserMode);
        eraserButtonImage.sprite = isEraserMode ? eraserModeSprite : defaultSprite;

    }

    /// <summary>
    /// 消しゴムモードが有効かどうか
    /// </summary>
    /// <returns>消しゴムモードかどうか</returns>
    public bool GetIsEraserMode()
    {
        return isEraserMode;
    }
}
