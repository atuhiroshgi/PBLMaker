using UnityEngine;
using UnityEngine.UI;

public class GoalButton : Button
{
    [SerializeField, Header("BlockPlacer�̎Q��")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("�S�[���ݒ胂�[�h���I�t�̎��̃A�C�R��")]
    private Sprite defaultSprite;
    [SerializeField, Header("�S�[���ݒ胂�[�h���I���̎��̃A�C�R��")]
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
