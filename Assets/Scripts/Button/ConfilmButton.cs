using UnityEngine;

public class ConfilmButton : Button
{
    [SerializeField, Header("ConfilmationDialogの参照")]
    private ConfilmationDialog confilmationDialog;
    [SerializeField, Header("DataButtonManagerの参照")]
    private DataButtonManager dataButtonManager;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Confilm();
        dataButtonManager.ToggleWindow(false);
    }
}
