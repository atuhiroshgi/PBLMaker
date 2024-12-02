using UnityEngine;

public class ConfilmButton : Button
{
    [SerializeField, Header("�m�F�_�C�A���O�̎Q��")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();
        confilmationDialog.Confilm();
    }
}
