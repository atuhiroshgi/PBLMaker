using Cysharp.Threading.Tasks;
using UnityEngine;

public class GridCell : Actor
{
    [SerializeField, Header("���̃Z���������牽�}�X�ڂ�")]
    private int xGrid;
    [SerializeField, Header("���̃Z���������牽�}�X�ڂ�")]
    private int yGrid;
    [SerializeField, Header("���̃Z���ɂǂ̃u���b�N���u����Ă��邩")]
    private int blockType = -1;

    [SerializeField, Header("�S�[���}�[�J�[�I�u�W�F�N�g")]
    private GameObject goalMarker;
    [SerializeField, Header("��̎��̃Z���̌�����")]
    private Sprite emptySprite;
    [SerializeField, Header("��Ŏ��s���ꂽ���̌�����")]
    private Sprite executeEmptySprite;
    [SerializeField, Header("�u���b�N�̌����ڂ�ύX���邽�߂̉摜�̔z��")]
    private Sprite[] cellSprites;

    private Collider2D col;
    private GameObject placedObject;        // ���̃Z���̏�ɑ��݂���I�u�W�F�N�g���i�[���邽�߂̃����o
    private SpriteRenderer spriteRenderer;  // ���̃Z���̌����ڂ�ύX���邽��
    private bool isGoal = false;

    private async void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(goalMarker != null) goalMarker.SetActive(false);

        while (true)
        {
            await UniTask.WaitUntil(() => !ExecuteManager.Instance.GetIsExecute());
            if (blockType == -1) spriteRenderer.sprite = emptySprite;
            await UniTask.WaitUntil(() => ExecuteManager.Instance.GetIsExecute());
            if (blockType == -1) spriteRenderer.sprite = null;
        }
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            goalMarker.SetActive(false);
            return;
        }
        if (goalMarker != null)
        {
            goalMarker.SetActive(isGoal);
        }
    }

    /// <summary>
    /// ���̃N���X����̃Z���̏������������������ɌĂ΂�郁�\�b�h
    /// </summary>
    /// <param name="newBlockType"></param>
    public void SetBlockType(int newBlockType)
    {
        blockType = newBlockType;
        UpdateAppearance();
    }

    /// <summary>
    /// �ݒu�����u���b�N�^�C�v���O�̂ƈႤ���ǂ���
    /// </summary>
    /// <param name="blockType">�ݒu����u���b�N�^�C�v</param>
    /// <returns>�Ⴄ�Ȃ�true, �����Ȃ�false</returns>
    public bool canPlaySEBlock(int blockType)
    {
        return this.blockType != blockType;
    }

    /// <summary>
    /// �u���b�N�^�C�v�ɉ����������ڂ̐ݒ�
    /// </summary>
    private void UpdateAppearance()
    {
        if (spriteRenderer == null) return;

        // �u���b�N�ɉ�����Sprite��ݒ�
        spriteRenderer.sprite = blockType == -1 ? emptySprite : cellSprites[blockType];
        // �F�Ɠ����x��ݒ�
        spriteRenderer.color = blockType == -1 ? new Color(0, 0, 0, 120f / 255f) : Color.white;
        // �R���C�_�[�̃g���K�[�ݒ�
        col.isTrigger = blockType == -1;

        this.gameObject.tag = blockType == -1 ? "Untagged" : "Ground";
    }

    /// <summary>
    /// �Z����ɔz�u����I�u�W�F�N�g��ݒ肷�郁�\�b�h
    /// </summary>
    /// <param name="newObjectPrefab"></param>
    public void SetPlacedObject(GameObject newObjectPrefab)
    {
        // ���ɔz�u�ς݂̃I�u�W�F�N�g������΍폜
        if(placedObject != null)
        {
            Destroy(placedObject);
        }

        // null����Ȃ���ΐݒu����
        if(newObjectPrefab != null)
        {
            placedObject = Instantiate(newObjectPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {

        }
    }

    /// <summary>
    /// �ݒu�����I�u�W�F�N�g���O�̂ƈႤ���ǂ���
    /// </summary>
    /// <param name="placeObject">�ݒu����I�u�W�F�N�g</param>
    /// <returns>�Ⴄ�Ȃ�true, �����Ȃ�false</returns>
    public bool canPlaySEObject(GameObject placeObject)
    {
        return placedObject != placeObject;
    }

    /// <summary>
    /// ���݂̃Z���̏�ɂ���Z�����������A�����Ɋ�Â��čX�V����
    /// </summary>
    public void UpdateUpperCells()
    {
        // GridManager���g�p���Č��݂̃Z���̏�iY���W��1�傫���Z���j���擾
        GridCell upperCell = GridManager.Instance.GetCellAtPosition(this.xGrid, this.yGrid + 1);

        // ��̃Z�������݂��ABlockType��0�ł���ꍇ
        if (upperCell != null && (upperCell.GetBlockType() == 0 || upperCell.GetBlockType() == 1))
        {
            // ��̃Z����BlockType��0�Ȃ�A���݂̃Z����BlockType��1�ɐݒ�
            SetBlockType(1);
        }

        // GridManager���g�p���Č��݂̃Z���̉��iY���W��1�������Z���j���擾
        GridCell lowerCell = GridManager.Instance.GetCellAtPosition(this.xGrid, this.yGrid - 1);

        // ���̃Z�������݂��ABlockType��0�ł���ꍇ
        if (lowerCell != null && lowerCell.GetBlockType() == 0)
        {
            // ���̃Z����BlockType��1�ɐݒ�
            lowerCell.SetBlockType(1);
        }
    }


    /// <summary>
    /// �S�[���ɐݒ肳��Ă��邩�̃Z�b�^�[
    /// </summary>
    /// <param name="isGoal">�S�[���ɐݒ肳��Ă��邩</param>
    public void SetIsGoal(bool isGoal)
    {
        this.isGoal = isGoal;
    }

    /// <summary>
    /// x���W�̃Z�b�^�[
    /// </summary>
    /// <param name="x"></param>
    public void SetX(int x)
    {
        this.xGrid = x;
    }

    /// <summary>
    /// y���W�̃Z�b�^�[
    /// </summary>
    /// <param name="y"></param>
    public void SetY(int y)
    {
        this.yGrid = y;
    }

    /// <summary>
    /// x���W�̃Q�b�^�[
    /// </summary>
    /// <returns>�Z����x���W(�Z�����J�E���g)</returns>
    public int GetX()
    {
        return xGrid;
    }

    /// <summary>
    /// y���W�̃Q�b�^�[
    /// </summary>
    /// <returns>�Z����y���W(�Z�����J�E���g)</returns>
    public int GetY()
    {
        return yGrid;
    }

    public int GetBlockType()
    {
        return blockType;
    }

    public bool GetIsGoal()
    {
        return isGoal;
    }

    public GameObject GetPlacedObject()
    {
        return placedObject;
    }
}
