using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField, Header("�z�u����GridCell�̃v���n�u")]
    private GameObject gridCellPrefab;

    [SerializeField, Header("�������̃Z���̐�")]
    private int width = 5;

    [SerializeField, Header("�c�����̃Z���̐�")]
    private int height = 5;

    [SerializeField, Header("�Z�����m�̊Ԋu")]
    private float cellSpacing = 1.28f;

    private GridCell[,] gridCells;  // �e�Z���̎Q�Ƃ�ێ�����2�����z��


    /// <summary>
    /// �O���b�h�𐶐����郁�\�b�h
    /// </summary>
    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// �w�肳�ꂽ���ƍ����̃O���b�h�𐶐�
    /// </summary>
    public void GenerateGrid()
    {
        // �O���b�h�T�C�Y�Ɋ�Â���2�����z���������
        gridCells = new GridCell[width, height];

        // ��ʂ̍����̃��[���h���W���擾
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeft.x += cellSpacing / 2;
        bottomLeft.y += cellSpacing / 2;
        bottomLeft.z = 0;  // Z���̈ʒu��0�ɐݒ�

        // �w�肵�����ƍ����ɉ����ăZ���𐶐����A���ׂ�
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // �Z���̈ʒu���v�Z
                Vector3 cellPosition = bottomLeft + new Vector3(x * cellSpacing, y * cellSpacing, 0);

                // �v���n�u���C���X�^���X��
                GameObject cellObj = Instantiate(gridCellPrefab, cellPosition, Quaternion.identity, transform);

                // GridCell�R���|�[�l���g���擾
                GridCell cell = cellObj.GetComponent<GridCell>();

                cell.SetX(x);
                cell.SetY(y);

                // �ʒu����ݒ�i�Z���̃C���X�y�N�^�[��Ŋm�F�ł���j
                cellObj.name = $"GridCell ({x}, {y})";

                // Grid�ɐF��t����
                if(x <= 3 && y == 2)
                {
                    cell.SetBlockType(0);
                }
                else if(x <= 3 && y < 2)
                {
                    cell.SetBlockType(1);
                }
                else
                {
                    cell.SetBlockType(-1);
                }

                // �O���b�h�z��Ɋi�[
                gridCells[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// �C�ӂ̃Z���Ƀu���b�N�^�C�v��ݒ�
    /// </summary>
    /// <param name="x">x���W</param>
    /// <param name="y">y���W</param>
    /// <param name="blockType">�ݒ肷��u���b�N�^�C�v</param>
    public void SetBlockTypeAt(int x, int y, int blockType)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridCells[x, y].SetBlockType(blockType);
        }
        else
        {
            Debug.LogWarning("�w�肳�ꂽ�Z���̈ʒu���͈͊O�ł�");
        }
    }

    /// <summary>
    /// �S�Ă�GridCell�N���X���A�^�b�`���ꂽ�I�u�W�F�N�g���폜
    /// </summary>
    public void ClearAllGridCells()
    {
        // �z������Ɋm�F���đS�Ă�GridCell���폜
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (gridCells[x, y] != null)
                {
                    Destroy(gridCells[x, y].gameObject);
                    gridCells[x, y] = null;
                }
            }
        }
    }

    public GridCell[,] GetGridCells()
    {
        return gridCells;
    }
}