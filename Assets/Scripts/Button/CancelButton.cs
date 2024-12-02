using UnityEngine;

public class CancelButton : Button
{
    [SerializeField, Header("確認ダイアログの参照")]
    private ConfilmationDialog confilmationDialog;
    [SerializeField, Header("DataButtonManagerの参照")]
    private DataButtonManager dataButtonManager;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Cancel();
        dataButtonManager.ToggleWindow(false);
    }
}
