using UnityEngine;

[CreateAssetMenu(fileName = "BlockStorageData", menuName = "Scriptable Objects/BlockStorageData")]
public class BlockStorageData : ScriptableObject
{
    [SerializeField, Header("ブロックストレージに含まれている行のLineData")]
    private LineData[] lineDatas = new LineData[6];

    // データにアクセスするためのプロパティ
    public LineData[] LineDatas => lineDatas;
}
