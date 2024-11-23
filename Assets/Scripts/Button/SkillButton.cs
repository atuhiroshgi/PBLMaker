using UnityEngine;
using UnityEngine.UI;

public class SkillButton : Button
{
    [SerializeField, Header("ピクトグラム画像")]
    private Image icon;
    [SerializeField, Header("どのスキルか判別するためのインデックス")]
    private int index;

    private bool isChecked;

    private void Update()
    {
        if (isChecked)
        {
            icon.color = Color.yellow;
        }
        else
        {
            icon.color = Color.white;
        }
    }

    protected override void OnClick()
    {
        base.OnClick();
        isChecked = !isChecked;
    }

    public void SetIsChecked(bool isChecked)
    {
        this.isChecked = isChecked;
    }

    public bool GetIsChecked()
    {
        return isChecked;
    }
}
