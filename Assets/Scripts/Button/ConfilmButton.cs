using UnityEngine;

public class ConfilmButton : Button
{
    [SerializeField, Header("確認ダイアログの参照")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Confilm();
    }
}
