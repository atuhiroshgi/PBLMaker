using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    private static readonly int LINE_LENGTH = 5;

    [SerializeField, Header("PaletteControllerの参照")]
    private PaletteController paletteController;
    [SerializeField, Header("BlockStorageDataの参照")]
    private BlockStorageData blockStorageData;
    [SerializeField, Header("Storageのアイコン(ブロック)")]
    private Image[] blockIconImages;
    [SerializeField, Header("Storageのアイコン(敵)")]
    private Image[] enemyIconImages;
    [SerializeField, Header("Storageのアイコン(イベント)")]
    private Image[] eventIconImage;
    [SerializeField, Header("Storageのアイコン(その他1)")]
    private Image[] other1IconImage;
    [SerializeField, Header("Storageのアイコン(その他2)")]
    private Image[] other2IconImage;

    private LineData[] lineDatas;

    private void Awake()
    {
        // アイコン画像の配列の配列
        Image[][] iconImageGroups = new Image[][]
        {
            blockIconImages,
            enemyIconImages,
            eventIconImage,
            other1IconImage,
            other2IconImage
        };

        // LineDataの取得
        lineDatas = blockStorageData.LineDatas;
        if (lineDatas == null)
        {
            Debug.LogWarning("LineDataが存在しません");
            return;
        }

        for (int i = 0; i < LINE_LENGTH; i++)
        {
            if (i >= iconImageGroups.Length)
            {
                Debug.LogWarning($"アイコングループが不足しています。i: {i}");
                continue;
            }

            // i番目のアイコングループを取得
            Image[] currentIconImages = iconImageGroups[i];
            BlockData[] blockDataArray = lineDatas[i].BlockData;

            for (int j = 0; j < blockDataArray.Length; j++)
            {
                if (j >= currentIconImages.Length)
                {
                    Debug.LogWarning($"blockDataArrayの長さがcurrentIconImagesの長さを超えています。i: {i}, j: {j}");
                    continue;
                }

                if (blockDataArray[j] != null)
                {
                    currentIconImages[j].sprite = blockDataArray[j].BlockSprite;
                    currentIconImages[j].enabled = true;    // 必要に応じて有効化
                }
                else
                {
                    currentIconImages[j].enabled = false;   // 画像がない場合は非表示
                }
            }
        }
    }
}
