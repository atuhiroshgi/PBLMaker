using UnityEngine;

[CreateAssetMenu(fileName = "LineData", menuName = "Scriptable Objects/LineData")]
public class LineData : ScriptableObject
{
    private static readonly int DATA_LENGTH = 12;    //1行の長さ

    [SerializeField, Header("この行に含まれるブロックのBlockData")]
    private BlockData[] blockData = new BlockData[DATA_LENGTH];

    public BlockData[] BlockData => blockData;
}
