using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField, Header("���̃Z���������牽�}�X�ڂ�")]
    private int xGrid;
    [SerializeField, Header("���̃Z���������牽�}�X�ڂ�")]
    private int yGrid;
    [SerializeField, Header("���̃Z���ɂǂ̃u���b�N���u����Ă��邩")]
    private int blockType = -1;
    
    [SerializeField, Header("��̎��̃Z���̌�����")]
    private Sprite emptySprite;
    [SerializeField, Header("�u���b�N�̌����ڂ�ύX���邽�߂̉摜�̔z��")]
    private Sprite[] cellSprites;

    private Collider2D col;
    private GameObject placedObject;        // ���̃Z���̏�ɑ��݂���I�u�W�F�N�g���i�[���邽�߂̃����o
    private SpriteRenderer spriteRenderer;  // ���̃Z���̌����ڂ�ύX���邽��

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
}
