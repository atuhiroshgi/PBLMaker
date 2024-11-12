using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
public class BlockPlacer : MonoBehaviour
{
    [SerializeField, Header("�ǂ̃��C���[��GridCell���^�[�Q�b�g�ɂ��邩")]
    private LayerMask gridCellLayer;
    [SerializeField, Header("�z�u����u���b�N�^�C�v")]
    private int blockType = 0;

    private void Start()
    {
        //�}�E�X�N���b�N���Ď�����񓯊��^�X�N���J�n
        WaitForMouseClickAsync().Forget();
    }

    /// <summary>
    /// �}�E�X���N���b�N�����܂őҋ@���A�N���b�N���ꂽ�璼����GridCell�Ƀu���b�N��z�u����
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid WaitForMouseClickAsync()
    {
        while (true)
        {
            //���N���b�N���������܂őҋ@
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

            while (Input.GetMouseButton(0))
            {
                //�}�E�X������GridCell��T���āASetBlockType���\�b�h���Ăяo��
                PlaceBlockUnderMouse();

                //���̃t���[���܂őҋ@
                await UniTask.Yield();
            }
        }
    }

    /// <summary>
    /// �}�E�X������GridCell�ɑ΂���SetBlockType���\�b�h���Ăяo��
    /// </summary>
    private void PlaceBlockUnderMouse()
    {
        //�}�E�X�̈ʒu�����[���h���W�ɕϊ�
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //2D���C�L���X�g�ŁA�w�背�C���[��̃R���C�_�[��T��
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, gridCellLayer);

        //GridCell�R���|�[�l���g���擾
        if(hit.collider != null)
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();

            if (cell == null) return;
            cell.SetBlockType(blockType);
        }
    }
}
