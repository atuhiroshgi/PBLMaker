using UnityEngine;

[CreateAssetMenu(fileName = "BlockStorageData", menuName = "Scriptable Objects/BlockStorageData")]
public class BlockStorageData : ScriptableObject
{
    [SerializeField, Header("�u���b�N�X�g���[�W�Ɋ܂܂�Ă���s��LineData")]
    private LineData[] lineDatas = new LineData[6];

    // �f�[�^�ɃA�N�Z�X���邽�߂̃v���p�e�B
    public LineData[] LineDatas => lineDatas;
}
