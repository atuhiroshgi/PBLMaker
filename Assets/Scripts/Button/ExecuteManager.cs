using System.Runtime.CompilerServices;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteManager : Button
{
    #region �V���O���g��
    private static ExecuteManager instance;
    
    public static ExecuteManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    [SerializeField, Header("CameraController�̎Q��")]
    private CameraController cameraController = null;
    [SerializeField, Header("UICanvas�̎Q��")]
    private Canvas uiCanvas;
    [SerializeField, Header("�Đ��{�^��")]
    private Sprite playSprite = null;
    [SerializeField, Header("�ꎞ��~�{�^��")]
    private Sprite pauseSprite = null;

    private Image executeIconImage;
    private Color playColor = new Color(181f / 255f, 218f / 255f, 164f / 255f);     // ���ΐF
    private Color pauseColor = new Color(208f / 255f, 74f / 255f, 57f / 255f);      // ��
    private bool isExecute = false;

    private void Awake()
    {
        // �V���O���g���C���X�^���X�̐ݒ�
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            // �������̃C���X�^���X�����݂����ꍇ�A���g��j������
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
            // ���s���J�n�������_�ł̃J�����̍��W��ۑ�
            cameraController.RememberStartPosition();

            uiCanvas.enabled = false;
        }
        else
        {
            //���s�������_�ł̃J�����̍��W�ɖ߂�
            cameraController.MoveToStartPosition();
        }
    }

    private void Update()
    {
        ChangeIcon();
    }

    /// <summary>
    /// �A�C�R�����̕\����ύX����
    /// </summary>
    private void ChangeIcon()
    {
        executeIconImage.sprite = isExecute ? pauseSprite : playSprite;
        executeIconImage.color = isExecute ? pauseColor : playColor;
    }

    /// <summary>
    /// ���s���Ă��邩�ǂ����̃Q�b�^�[
    /// </summary>
    /// <returns></returns>
    public bool GetIsExecute()
    {
        return isExecute;
    }
}
