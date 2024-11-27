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
    [SerializeField, Header("再生ボタン")]
    private Sprite playSprite = null;
    [SerializeField, Header("一時停止ボタン")]
    private Sprite pauseSprite = null;
    [SerializeField, Header("実行したら非表示にしたいUI要素")]
    private GameObject[] switchingUIs;

    private Image executeIconImage;
    private Color playColor = new Color(181f / 255f, 218f / 255f, 164f / 255f);     // 黄緑色
    private Color pauseColor = new Color(208f / 255f, 74f / 255f, 57f / 255f);      // 赤
    private bool isExecute = false;                     // 実行中かどうか
    private bool isSkillSelectWindowOpen = false;       // スキルセレクトウィンドウが開いているかどうか

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
        if (isSkillSelectWindowOpen) return;
        base.OnClick();

        isExecute = !isExecute;

        if (isExecute)
        {
            // 実行を開始した時点でのカメラの座標を保存
            cameraController.RememberStartPosition();

            HideAllUI();
        }
        else
        {
            // 実行した時点でのカメラの座標に戻す
            cameraController.MoveToStartPosition();

            DisplayAllUI();
        }
    }

    private void Update()
    {
        ChangeIcon();
    }

    /// <summary>
    /// 指定されたUIを全て表示する
    /// </summary>
    public void DisplayAllUI()
    {
        // switchingUIsの要素を全て表示する
        foreach (GameObject switchingUI in switchingUIs)
        {
            switchingUI.SetActive(true);
        }
    }

    /// <summary>
    /// 指定されたUIを全て非表示にする
    /// </summary>
    public void HideAllUI()
    {
        // switchingUIsの要素を全て非表示する
        foreach (GameObject switchingUI in switchingUIs)
        {
            switchingUI.SetActive(false);
        }
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
    /// 実行しているかどうかのセッター
    /// </summary>
    /// <param name="isExecute">実行しているかどうか</param>
    public void SetIsExecute(bool isExecute)
    {
        this.isExecute = isExecute;

        if (isExecute)
        {
            HideAllUI();
        }
        else
        {
            DisplayAllUI();
        }
    }

    /// <summary>
    /// 実行しているかどうかのゲッター
    /// </summary>
    /// <returns></returns>
    public bool GetIsExecute()
    {
        return isExecute;
    }

    public void SetSkillSelectorIsOpen(bool isSkillSelectWindowOpen)
    {
        this.isSkillSelectWindowOpen = isSkillSelectWindowOpen;
    }
}
