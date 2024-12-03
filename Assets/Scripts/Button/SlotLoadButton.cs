using UnityEngine;
using UnityEngine.UI;

public class SlotLoadButton : Button
{
    private static readonly string[] LOAD_FILEPATHS = {
        "gridData1.json",
        "gridData2.json",
        "gridData3.json"
    };

    [SerializeField, Header("ボタンに表示するテキストUI")]
    private Text buttonText;

    private ResetButton resetButton;
    private int slotNumber;

    public void Initialize(int number, ResetButton resetButton)
    {
        slotNumber = number;
        this.resetButton = resetButton;
        buttonText.text = $"{slotNumber}";
    }

    protected override void OnClick()
    {
        base.OnClick();

        resetButton.ResetGridCell();
        GridManager.Instance.LoadGridData(LOAD_FILEPATHS[slotNumber - 1]);
        transform.parent.gameObject.SetActive(false);
    }
}
