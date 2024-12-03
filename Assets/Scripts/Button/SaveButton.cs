using UnityEngine;
using Cysharp.Threading.Tasks;

public class SaveButton : Button
{
    private static readonly string SAVE_FILEPATH = "gridData.json";

    [SerializeField, Header("GridManagerの参照")]
    private GridManager gridManager;
    [SerializeField, Header("確認ダイアログの参照")]
    private ConfilmationDialog confilmationDialog;

    protected override void OnClick()
    {
        base.OnClick();


        if(confilmationDialog != null)
        {
            confilmationDialog.Show(() =>
            {
                gridManager.SaveGridData(SAVE_FILEPATH);
                Debug.Log("データをセーブしました");
            });
        }
        else
        {
            Debug.LogError("ConfilmationDialogが設定されていません");
        }

    }
}
