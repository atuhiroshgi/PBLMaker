using UnityEngine;
using Cysharp.Threading.Tasks;

public class SaveButton : Button
{
    private static readonly string SAVE_FILEPATH = "gridData.json";

    [SerializeField, Header("GridManager�̎Q��")]
    private GridManager gridManager;
    [SerializeField, Header("�m�F�_�C�A���O�̎Q��")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();


        if(confilmationDialog != null)
        {
            confilmationDialog.Show(() =>
            {
                gridManager.SaveGridData(SAVE_FILEPATH);
                Debug.Log("�f�[�^���Z�[�u���܂���");
            });
        }
        else
        {
            Debug.LogError("ConfilmationDialog���ݒ肳��Ă��܂���");
        }

    }
}
