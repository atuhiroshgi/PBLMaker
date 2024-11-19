using UnityEngine;
using UnityEngine.UI;

public class PaletteController : MonoBehaviour
{
    [SerializeField, Header("BlockStorageDataを参照")]
    private BlockStorageData blockStorageData;
    [SerializeField, Header("パレットのアイコン画像")]
    private Image[] paletteIcons;
    [SerializeField, Header("アイコンそれぞれの背景")]
    private Image[] paletteIconBgs;

}
