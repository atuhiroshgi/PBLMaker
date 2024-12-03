using UnityEngine;
using UnityEngine.UI;
using System;

public class SlotButton : Button
{
    private static readonly string[] SAVE_FILEPATHS = {
        "gridData1.json",
        "gridData2.json", 
        "gridData3.json"
    };

    [SerializeField, Header("Buttonに使うテキストUI")]
    private Text buttonText;

    private SaveButton saveButton;
    private int slotNumber;

    public void Initialize(int number)
    {
        slotNumber = number;
        buttonText.text = $"{slotNumber}";
    }

    protected override void OnClick()
    {
        base.OnClick();
        
        GridManager.Instance.SaveGridData(SAVE_FILEPATHS[slotNumber - 1]);

        // 親パネルを非表示
        transform.parent.gameObject.SetActive(false);
    }
}