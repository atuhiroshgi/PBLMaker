using UnityEngine;

[CreateAssetMenu(fileName = "LineData", menuName = "Scriptable Objects/LineData")]
public class LineData : ScriptableObject
{
    private static readonly int DATA_LENGTH = 12;    //1�s�̒���

    [SerializeField, Header("���̍s�Ɋ܂܂��u���b�N��BlockData")]
    private BlockData[] blockData = new BlockData[DATA_LENGTH];

    // �f�[�^�ɃA�N�Z�X���邽�߂̃v���p�e�B
    public BlockData[] BlockData => blockData;
}
