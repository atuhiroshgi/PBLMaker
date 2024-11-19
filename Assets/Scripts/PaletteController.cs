using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    [SerializeField, Header("BlockPlacer���Q��")]
    private BlockPlacer blockPlacer;
    [SerializeField, Header("BlockButton���Q��")]
    private BlockButton[] blockButtons; 
    [SerializeField, Header("BlockStorageData���Q��")]
    private BlockStorageData blockStorageData;
    [SerializeField, Header("�p���b�g�̃A�C�R���摜")]
    private Image[] paletteIcons;
    [SerializeField, Header("�A�C�R�����ꂼ��̔w�i")]
    private Image[] paletteIconBgs;

    private Color selectedColor = Color.yellow;
    private Color defaultColor = Color.gray;
    private int selectedBlockType = 0;
    private int targetLineIndex = 0;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// �p���b�g�̃A�C�R�������������郁�\�b�h
    /// </summary>
    private void Init()
    {
        // LineData�̎擾
        LineData[] lineDatas = blockStorageData.LineDatas;

        if (lineDatas == null || lineDatas.Length <= targetLineIndex)
        {
            Debug.LogWarning("�w�肳�ꂽ�C���f�b�N�X��LineData�͑��݂����ւ��");
            return;
        }

        // �Ώۂ�LineData���擾
        LineData targetLineData = lineDatas[targetLineIndex];
        BlockData[] blockDataArray = targetLineData.BlockData;

        for (int i = 0; i < paletteIcons.Length; i++)
        {
            // �u���b�N�f�[�^�̔z��͈͓̔��Ȃ�\�����āA�摜��ݒ�
            if (i < blockDataArray.Length && blockDataArray[i] != null)
            {
                BlockData blockData = blockDataArray[i];

                // �A�C�R���摜��ݒ�
                paletteIcons[i].sprite = blockData.BlockSprite;
                paletteIcons[i].enabled = true;
                paletteIconBgs[i].color = i == selectedBlockType ? selectedColor : defaultColor;
                blockButtons[i].SetBlockType(blockData.BlockType);
            }
            else
            {
                // �͈͊O�Ȃ��\��
                paletteIconBgs[i].enabled = false;
                paletteIcons[i].enabled = false;
            }
        }

        // �u���b�N�^�C�v��blockPlacer�ɒʒm
        blockPlacer.SetBlockType(selectedBlockType);
    }

    /// <summary>
    /// �I������Ă���u���b�N�̃^�C�v�̃Z�b�^�[
    /// </summary>
    /// <param name="blockType">�u���b�N�^�C�v</param>
    public void SetSelectedBlockType(int blockType)
    {
        this.selectedBlockType = blockType;
        Init();
    }
}
