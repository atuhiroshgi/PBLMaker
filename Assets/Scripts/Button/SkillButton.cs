using UnityEngine;
using UnityEngine.UI;

public class SkillButton : Button
{
    [SerializeField, Header("�s�N�g�O�����摜")]
    private Image icon;
    [SerializeField, Header("�ǂ̃X�L�������ʂ��邽�߂̃C���f�b�N�X")]
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
