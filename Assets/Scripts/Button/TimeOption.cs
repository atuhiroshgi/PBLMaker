using UnityEngine;

public class TimeOption : Button
{
    [SerializeField, Header("���̑I�����̃C���f�b�N�X")]
    private int index;

    private TimeSetter timeSetter;

    private void Awake()
    {
        timeSetter = transform.parent.GetComponent<TimeSetter>();
    }

    protected override void OnClick()
    {
        base.OnClick();
        timeSetter.SetSelectedIndex(index);
    }
}
