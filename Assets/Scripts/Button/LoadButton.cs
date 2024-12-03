using UnityEngine;

public class LoadButton : Button
{
    private static readonly string LOAD_FILEPATH = "gridData.json";

    [SerializeField, Header("GridManager�̎Q��")]
    private GridManager gridManager;
    [SerializeField, Header("ResetButton�̎Q��")]
    private ResetButton resetButton;
    [SerializeField, Header("�m�F�_�C�A���O�̎Q��")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();


        if (confilmationDialog != null)
        {
            confilmationDialog.Show(() =>
            {
                gridManager.ReloadGridCells();
                gridManager.LoadGridData(LOAD_FILEPATH);
                Debug.Log("�f�[�^�����[�h���܂���");
            });
        }
        else
        {
            Debug.LogError("ConfilmationDialog���ݒ肳��Ă��܂���");
        }

    }
}
