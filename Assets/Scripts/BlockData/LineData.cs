using UnityEngine;

[CreateAssetMenu(fileName = "LineData", menuName = "Scriptable Objects/LineData")]
public class LineData : ScriptableObject
{
    private static readonly int DATA_LENGTH = 12;    //1�s�̒���

    [SerializeField, Header("���̍s�Ɋ܂܂��u���b�N��BlockData")]
    private BlockData[] blockData = new BlockData[DATA_LENGTH];

    public BlockData[] BlockData => blockData;
}
