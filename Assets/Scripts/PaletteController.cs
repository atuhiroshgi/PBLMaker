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

    private GameObject currentObject;           // ���ݑI������Ă���I�u�W�F�N�g
    private Color selectedColor = Color.red;    // �I�΂�Ă��鎞�̐F
    private Color defaultColor = Color.gray;    // �I�΂�Ă��Ȃ����̐F
    private int selectedBlockType = 0;          // �I�΂�Ă���u���b�N�^�C�v
    private int targetLineIndex = 0;            // ���s�ڂ�I�����Ă��邩�̃C���f�b�N�X
    private bool selectedIsBlock = true;        // �I�΂�Ă���̂��u���b�N���ǂ���

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

                // �I�΂�Ă���I�u�W�F�N�g���u���b�N���ǂ����𔻒f���ĕϐ��Ɋi�[
                if(i == selectedBlockType)
                {
                    selectedIsBlock = blockData.IsBlock;
                    if (!selectedIsBlock)
                    {
                        currentObject = blockData.Prefab;
                    }
                }
            }
            else
            {
                // �͈͊O�Ȃ��\��
                paletteIconBgs[i].enabled = false;
                paletteIcons[i].enabled = false;
            }
        }
        
        // �I�΂�Ă���̂��u���b�N���ǂ����ŏ�����ύX
        if (selectedIsBlock)
        {
            // �u���b�N�^�C�v��blockPlacer�ɒʒm
            blockPlacer.SetBlockType(selectedBlockType);
        }
        else
        {
            Debug.Log("������");
            // �ݒu����I�u�W�F�N�g��BlockPlacer�ɓn��
            blockPlacer.SetObjectToPlace(currentObject);
        }
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

    /// <summary>
    /// �I������Ă���I�u�W�F�N�g���u���b�N���ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns>�I������Ă���I�u�W�F�N�g���u���b�N���ǂ���</returns>
    public bool GetSelectedIsBlock()
    {
        return selectedIsBlock;
    }
}
