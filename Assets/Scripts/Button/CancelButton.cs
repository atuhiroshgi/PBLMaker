using UnityEngine;

public class CancelButton : Button
{
    [SerializeField, Header("�m�F�_�C�A���O�̎Q��")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Cancel();
    }
}
