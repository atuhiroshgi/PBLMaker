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
    [SerializeField, Header("�Đ��{�^��")]
    private Sprite playSprite = null;
    [SerializeField, Header("�ꎞ��~�{�^��")]
    private Sprite pauseSprite = null;
    [SerializeField, Header("���s�������\���ɂ�����UI�v�f")]
    private GameObject[] switchingUIs;

    private Image executeIconImage;
    private Color playColor = new Color(181f / 255f, 218f / 255f, 164f / 255f);     // ���ΐF
    private Color pauseColor = new Color(208f / 255f, 74f / 255f, 57f / 255f);      // ��
    private bool isExecute = false;                     // ���s�����ǂ���
    private bool isSkillSelectWindowOpen = false;       // �X�L���Z���N�g�E�B���h�E���J���Ă��邩�ǂ���

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
        if (isSkillSelectWindowOpen) return;
        base.OnClick();

        isExecute = !isExecute;

        if (isExecute)
        {
            // ���s���J�n�������_�ł̃J�����̍��W��ۑ�
            cameraController.RememberStartPosition();

            HideAllUI();
        }
        else
        {
            // ���s�������_�ł̃J�����̍��W�ɖ߂�
            cameraController.MoveToStartPosition();

            DisplayAllUI();
        }
    }

    private void Update()
    {
        ChangeIcon();
    }

    /// <summary>
    /// �w�肳�ꂽUI��S�ĕ\������
    /// </summary>
    public void DisplayAllUI()
    {
        // switchingUIs�̗v�f��S�ĕ\������
        foreach (GameObject switchingUI in switchingUIs)
        {
            switchingUI.SetActive(true);
        }
    }

    /// <summary>
    /// �w�肳�ꂽUI��S�Ĕ�\���ɂ���
    /// </summary>
    public void HideAllUI()
    {
        // switchingUIs�̗v�f��S�Ĕ�\������
        foreach (GameObject switchingUI in switchingUIs)
        {
            switchingUI.SetActive(false);
        }
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
    /// ���s���Ă��邩�ǂ����̃Z�b�^�[
    /// </summary>
    /// <param name="isExecute">���s���Ă��邩�ǂ���</param>
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
    /// ���s���Ă��邩�ǂ����̃Q�b�^�[
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
