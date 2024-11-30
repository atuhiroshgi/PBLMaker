using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;

public class BlockButton : Button
{
    [SerializeField, Header("PaletteController���Q��")]
    private PaletteController paletteController;
    [SerializeField, Header("EraserMode���Q��")]
    private EraserButton eraserButton;
    [SerializeField, Header("�ΐ��̃I�u�W�F�N�g")]
    private GameObject diagonalLine;
    [SerializeField, Header("�A�C�R���������ꂽ�Ƃ���BlockPlacer�ɐݒ肷��BlockType")]
    private int blockType;

    private Image thisObjectImage;

    private void Awake()
    {
        thisObjectImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(eraserButton != null)
        {
            // EraserButton�̏�Ԃ��m�F���AdiagonalLine��\��/��\���ɐ؂�ւ���
            if (!eraserButton.GetIsEraserMode() || !thisObjectImage.IsActive())
            {
                diagonalLine.SetActive(false);  // EraserMode���L���Ȃ�\��
            }
            else
            {
                diagonalLine.SetActive(true); // EraserMode�������Ȃ��\��
            }
        }

    }

    protected override void OnClick()
    {
        // �N���b�N���ꂽ��I�����ꂽ���Ƃ�PaletteController�ɒʒm����
        paletteController.SetSelectedBlockType(this.blockType);
    }

    /// <summary>
    /// blockType�̃Z�b�^�[
    /// </summary>
    /// <param name="blockType">�A�C�R���ɂǂ̃u���b�N�^�C�v��ݒ肷�邩</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }
}
