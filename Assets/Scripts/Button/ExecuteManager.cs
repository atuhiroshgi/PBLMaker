using System.Runtime.CompilerServices;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteManager : Button
{
    #region シングルトン
    private static ExecuteManager instance;
    
    public static ExecuteManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    [SerializeField, Header("CameraControllerの参照")]
    private CameraController cameraController = null;
    [SerializeField, Header("UICanvasの参照")]
    private Canvas uiCanvas;
    [SerializeField, Header("再生ボタン")]
    private Sprite playSprite = null;
    [SerializeField, Header("一時停止ボタン")]
    private Sprite pauseSprite = null;

    private Image executeIconImage;
    private Color playColor = new Color(181f / 255f, 218f / 255f, 164f / 255f);     // 黄緑色
    private Color pauseColor = new Color(208f / 255f, 74f / 255f, 57f / 255f);      // 赤
    private bool isExecute = false;

    private void Awake()
    {
        // シングルトンインスタンスの設定
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            // もし他のインスタンスが存在した場合、自身を破棄する
            Destroy(gameObject);
        }
        
        executeIconImage = GetComponent<Image>();
        executeIconImage.sprite = playSprite;
        executeIconImage.color = playColor;
        isExecute = false;
    }

    protected override void OnClick()
    {
        base.OnClick();

        isExecute = !isExecute;

        if (isExecute)
        {
            // 実行を開始した時点でのカメラの座標を保存
            cameraController.RememberStartPosition();

            uiCanvas.enabled = false;
        }
        else
        {
            //実行した時点でのカメラの座標に戻す
            cameraController.MoveToStartPosition();
        }
    }

    private void Update()
    {
        ChangeIcon();
    }

    /// <summary>
    /// アイコンをの表示を変更する
    /// </summary>
    private void ChangeIcon()
    {
        executeIconImage.sprite = isExecute ? pauseSprite : playSprite;
        executeIconImage.color = isExecute ? pauseColor : playColor;
    }

    /// <summary>
    /// 実行しているかどうかのゲッター
    /// </summary>
    /// <returns></returns>
    public bool GetIsExecute()
    {
        return isExecute;
    }
}
