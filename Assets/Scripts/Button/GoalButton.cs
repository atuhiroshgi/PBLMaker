using UnityEngine;
using UnityEngine.UI;

public class GoalButton : Button
{
    [SerializeField, Header("BlockPlacerの参照")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("ゴール設定モードがオフの時のアイコン")]
    private Sprite defaultSprite;
    [SerializeField, Header("ゴール設定モードがオンの時のアイコン")]
    private Sprite goalSettingModeSprite;

    private Image goalButtonImage;
    private bool isGoalSettingMode;

    private void Awake()
    {
        goalButtonImage = GetComponent<Image>();
    }

    private void Update()
    {
        goalButtonImage.enabled = ExecuteManager.Instance.GetIsExecute() ? false : true;
    }

    protected override void OnClick()
    {
        base.OnClick();
        isGoalSettingMode = !isGoalSettingMode;

        blockPlacer.SetIsGoalSettingMode(isGoalSettingMode);
        goalButtonImage.sprite = isGoalSettingMode ? goalSettingModeSprite : defaultSprite;
    }
}
