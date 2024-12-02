using UnityEngine;

public class LoadButton : Button
{
    private static readonly string LOAD_FILEPATH = "gridData.json";

    [SerializeField, Header("GridManagerの参照")]
    private GridManager gridManager;
    [SerializeField, Header("確認ダイアログの参照")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();

        gridManager.ReloadGridCells();

        if (confilmationDialog != null)
        {
            confilmationDialog.Show(() =>
            {
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
