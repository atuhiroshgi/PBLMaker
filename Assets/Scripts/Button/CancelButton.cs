using UnityEngine;

public class CancelButton : Button
{
    [SerializeField, Header("�m�F�_�C�A���O�̎Q��")]
    private ConfilmationDialog confilmationDialog;
    [SerializeField, Header("DataButtonManager�̎Q��")]
    private DataButtonManager dataButtonManager;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Cancel();
        dataButtonManager.ToggleWindow(false);
    }
}
