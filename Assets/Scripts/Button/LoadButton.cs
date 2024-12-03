using UnityEngine;

public class LoadButton : Button
{
    private static readonly string LOAD_FILEPATH = "gridData.json";

    [SerializeField, Header("GridManagerの参照")]
    private GridManager gridManager;
    [SerializeField, Header("ResetButtonの参照")]
    private ResetButton resetButton;
    [SerializeField, Header("確認ダイアログの参照")]
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
                Debug.Log("データをロードしました");
            });
        }
        else
        {
            Debug.LogError("ConfilmationDialogが設定されていません");
        }

    }
}
