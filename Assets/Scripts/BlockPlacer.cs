using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
public class BlockPlacer : MonoBehaviour
{
    [SerializeField, Header("PaletteController�̎Q��")]
    private PaletteController paletteController;
    [SerializeField, Header("�ǂ̃��C���[��GridCell���^�[�Q�b�g�ɂ��邩")]
    private LayerMask gridCellLayer;
    [SerializeField, Header("�z�u����u���b�N�^�C�v")]
    private int blockType = 0;
    [SerializeField, Header("�d�Ȃ��Ă���ԃu���b�N��u���Ăق����Ȃ�UI")]
    private GameObject[] uiObjects;

    private GameObject objectToPlace;
    private bool isEraserMode = false;

    private async void Update()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            await PlaceBlocksWhileMouseHeld();
        }
    }

    /// <summary>
    /// �}�E�X��������Ă���ԁA�u���b�N��z�u����
    /// </summary>
    private async UniTask PlaceBlocksWhileMouseHeld()
    {
        while (Input.GetMouseButton(0))
        {
            // �}�E�X������GridCell��T���āASetBlockType���\�b�h���Ăяo��
            PlaceObjectUnderMouse();

            // ���͂���������A���Ԋu���ƂɎ��̃t���[���܂őҋ@
            await UniTask.DelayFrame(1);
        }
    }

    /// <summary>
    /// �}�E�X������GridCell�ɑ΂���SetBlockType���\�b�h���Ăяo��
    /// </summary>
    private void PlaceObjectUnderMouse()
    {
        // �}�E�X�̈ʒu�����[���h���W�ɕϊ�
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2D���C�L���X�g�ŁA�w�背�C���[��̃R���C�_�[��T��
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, gridCellLayer);

        // GridCell�R���|�[�l���g���擾
        if (hit.collider != null)
        {
            if (isEraserMode)
            {
                DeleteObject(hit);
            }
            else
            {
                if(paletteController.GetSelectedIsBlock()) PlaceBlock(hit);
                if(!paletteController.GetSelectedIsBlock()) PlaceObject(hit);
            }
        }
    }

    /// <summary>
    /// �u���b�N��ݒu����p�̃��\�b�h
    /// </summary>
    /// <param name="hit">�w�背�C���[��̃R���C�_�[�̈ʒu</param>
    private void PlaceBlock(RaycastHit2D hit)
    {
        // Button�N���X�ɏd�Ȃ��Ă���Ƃ��̓u���b�N��u���Ȃ�
        Player player = hit.collider.gameObject.GetComponent<Player>();
        Button button = hit.collider.GetComponent<Button>();
        if (button != null)
        {
            return;
        }

        GridCell cell = hit.collider.GetComponent<GridCell>();
        if (cell != null)
        {
            // �u���b�N�^�C�v��ݒ�
            cell.SetBlockType(blockType);

            // GridCell�̃��C���[��"Wall"�ɕύX
            cell.gameObject.layer = blockType == -1 ? LayerMask.NameToLayer("Grid") : LayerMask.NameToLayer("Wall");

            if(blockType != -1)
            {
                cell.gameObject.tag = "Ground";
            }
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g��ݒu����p�̃��\�b�h
    /// </summary>
    /// <param name="hit">�w�背�C���[��̃R���C�_�[�̈ʒu</param>
    private void PlaceObject(RaycastHit2D hit)
    {
        GridCell cell = hit.collider.GetComponent<GridCell>();
        if(cell != null)
        {
            cell.SetPlacedObject(null);
            cell.SetPlacedObject(objectToPlace);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g������
    /// </summary>
    /// <param name="hit">�w�背�C���[��̃R���C�_�[�̈ʒu</param>
    private void DeleteObject(RaycastHit2D hit)
    {
        // Button�N���X�ɏd�Ȃ��Ă���Ƃ��̓u���b�N��u���Ȃ�
        Player player = hit.collider.gameObject.GetComponent<Player>();
        Button button = hit.collider.GetComponent<Button>();
        GridCell cell = hit.collider.GetComponent<GridCell>();
        if (button != null)
        {
            return;
        }

        if (cell != null)
        {
            // �u���b�N�^�C�v��ݒ�
            cell.SetBlockType(-1);
            cell.gameObject.layer = LayerMask.NameToLayer("Grid");
            cell.gameObject.tag = "Grid";

            // null��u��(����)
            cell.SetPlacedObject(null);
        }
    }

    /// <summary>
    /// �}�E�X�|�C���^��UI�v�f�ɏd�Ȃ��Ă��邩�ǂ������m�F���郁�\�b�h
    /// </summary>
    /// <returns>�d�Ȃ��Ă��邩</returns>
    private bool IsPointerOverUIObject()
    {
        // Raycast���g����UI�����o
        UnityEngine.EventSystems.PointerEventData eventDataCurrentPosition = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // UI�I�u�W�F�N�g�z��Ɋ܂܂�Ă��邩���`�F�b�N
        foreach (var result in results)
        {
            foreach (var uiObject in uiObjects)
            {
                if (result.gameObject == uiObject)
                {
                    //�d�Ȃ��Ă���I�u�W�F�N�g����������
                    return true;
                }
            }
        }
        //�d�Ȃ��Ă���I�u�W�F�N�g��������Ȃ�����
        return false;
    }

    /// <summary>
    /// �u���b�N�^�C�v�̃Z�b�^�[
    /// </summary>
    /// <param name="blockType">�u���b�N�^�C�v</param>
    public void SetBlockType(int blockType)
    {
        this.blockType = blockType;
    }

    /// <summary>
    /// �ݒu����I�u�W�F�N�g�̃Z�b�^�[
    /// </summary>
    /// <param name="objectToPlace">�ݒu����I�u�W�F�N�g</param>
    public void SetObjectToPlace(GameObject objectToPlace)
    {
        this.objectToPlace = objectToPlace;
    }

    /// <summary>
    /// �����S�����[�h���ǂ����̃t���O�̃Z�b�^�[
    /// </summary>
    /// <param name="isEraserMode">�����S�����[�h���ǂ���</param>
    public void SetIsEraserMode(bool isEraserMode)
    {
        this.isEraserMode = isEraserMode;
    }
}

