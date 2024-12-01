using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    private static readonly int LINE_LENGTH = 5;

    [SerializeField, Header("PaletteController�̎Q��")]
    private PaletteController paletteController;
    [SerializeField, Header("BlockStorageData�̎Q��")]
    private BlockStorageData blockStorageData;
    [SerializeField, Header("Storage�̃A�C�R��(�u���b�N)")]
    private Image[] blockIconImages;
    [SerializeField, Header("Storage�̃A�C�R��(�G)")]
    private Image[] enemyIconImages;
    [SerializeField, Header("Storage�̃A�C�R��(�C�x���g)")]
    private Image[] eventIconImage;
    [SerializeField, Header("Storage�̃A�C�R��(���̑�1)")]
    private Image[] other1IconImage;
    [SerializeField, Header("Storage�̃A�C�R��(���̑�2)")]
    private Image[] other2IconImage;

    private LineData[] lineDatas;

    private void Awake()
    {
        // �A�C�R���摜�̔z��̔z��
        Image[][] iconImageGroups = new Image[][]
        {
            blockIconImages,
            enemyIconImages,
            eventIconImage,
            other1IconImage,
            other2IconImage
        };

        // LineData�̎擾
        lineDatas = blockStorageData.LineDatas;
        if (lineDatas == null)
        {
            Debug.LogWarning("LineData�����݂��܂���");
            return;
        }

        for (int i = 0; i < LINE_LENGTH; i++)
        {
            if (i >= iconImageGroups.Length)
            {
                Debug.LogWarning($"�A�C�R���O���[�v���s�����Ă��܂��Bi: {i}");
                continue;
            }

            // i�Ԗڂ̃A�C�R���O���[�v���擾
            Image[] currentIconImages = iconImageGroups[i];
            BlockData[] blockDataArray = lineDatas[i].BlockData;

            for (int j = 0; j < blockDataArray.Length; j++)
            {
                if (j >= currentIconImages.Length)
                {
                    Debug.LogWarning($"blockDataArray�̒�����currentIconImages�̒����𒴂��Ă��܂��Bi: {i}, j: {j}");
                    continue;
                }

                if (blockDataArray[j] != null)
                {
                    currentIconImages[j].sprite = blockDataArray[j].BlockSprite;
                    currentIconImages[j].enabled = true;    // �K�v�ɉ����ėL����
                }
                else
                {
                    currentIconImages[j].enabled = false;   // �摜���Ȃ��ꍇ�͔�\��
                }
            }
        }
    }
}
