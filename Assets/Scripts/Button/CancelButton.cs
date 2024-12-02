using UnityEngine;

public class CancelButton : Button
{
    [SerializeField, Header("確認ダイアログの参照")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Cancel();
    }
}
