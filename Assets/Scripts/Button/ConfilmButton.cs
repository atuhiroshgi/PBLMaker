using UnityEngine;

public class ConfilmButton : Button
{
    [SerializeField, Header("ConfilmationDialog‚ÌQÆ")]
    private ConfilmationDialog confilmationDialog;
    [SerializeField, Header("DataButtonManager‚ÌQÆ")]
    private DataButtonManager dataButtonManager;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Confilm();
        dataButtonManager.ToggleWindow(false);
    }
}
