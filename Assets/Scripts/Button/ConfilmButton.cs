using UnityEngine;

public class ConfilmButton : Button
{
    [SerializeField, Header("ConfilmationDialog�̎Q��")]
    private ConfilmationDialog confilmationDialog;
    [SerializeField, Header("DataButtonManager�̎Q��")]
    private DataButtonManager dataButtonManager;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Confilm();
        dataButtonManager.ToggleWindow(false);
    }
}
