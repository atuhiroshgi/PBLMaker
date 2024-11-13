using UnityEngine;

public class ResetButton : Button
{
    [SerializeField, Header("GridGenerator���Q��")]
    private GridGenerator gridGenerator = null;

    private bool isPressing = false;            // �{�^����������Ă��邩�ǂ���
    private float pressTime = 0f;               // �{�^����������Ă��鎞��
    private const float requiredHoldTime = 3f;  // 3�b�ԉ������K�v

    protected override void OnPointerDown()
    {
        base.OnPointerDown();
        isPressing = true;
        pressTime = 0f;     // �^�C�}�[�����Z�b�g
    }

    protected override void OnPointerUp()
    {
        base.OnPointerUp();
        isPressing = false;
        pressTime = 0f;     // �^�C�}�[�����Z�b�g
    }

    private void Update()
    {
        // �{�^����������Ă���ԁA���Ԃ��v��
        if (isPressing)
        {
            pressTime += Time.deltaTime;

            // �������Ԃ��w�肳�ꂽ���ԂɒB������OnClick���������s
            if (pressTime >= requiredHoldTime)
            {
                ResetGridCell();
                isPressing = false;     // ��������x�����s�����߃t���O���I�t��
            }
        }
    }

    /// <summary>
    /// ���ꂽ�}�b�v��S�ď����čĐ�������
    /// </summary>
    private void ResetGridCell()
    {
        gridGenerator.ClearAllGridCells();
        gridGenerator.GenerateGrid();
    }
}
