using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class TimeSetter : Button
{
    private static readonly Vector2 CANVAS_OFFSET = new Vector2(960, 540);
    private static readonly Vector3 DEFAULT_POSITION = new Vector3(615 + CANVAS_OFFSET.x, 300 + CANVAS_OFFSET.y, 0);
    private static readonly Vector3 EXECUTE_POSITION = new Vector3(770 + CANVAS_OFFSET.x, 465 + CANVAS_OFFSET.y, 0);
    private static readonly int OPTION0_TIME = 10;
    private static readonly int OPTION1_TIME = 50;
    private static readonly int OPTION2_TIME = 100;
    private static readonly int OPTION3_TIME = 200;
    private static readonly int OPTION4_TIME = 500;

    [SerializeField, Header("Player�̎Q��")]
    private Player player;
    [SerializeField, Header("�c��b����\������e�L�X�g")]
    private TextMeshProUGUI countDownText;
    [SerializeField, Header("�I�����̔z��")]
    private GameObject[] timeOptions;
    [SerializeField, Header("�I�����̃e�L�X�g")]
    private TextMeshProUGUI[] optionsText;
    [SerializeField, Header("�J�E���g�_�E������(�b)")]
    private int countDownTime = 10;

    private float remainingTime;
    private int selectedIndex;
    private bool isOpen = false;

    private void Awake()
    {
        SetOptionTexts();
        HideAllTimeOptions();           // �I������S�Ĕ�\��

        // ������
        Init();
    }

    private void SetOptionTexts()
    {
        int[] optionTimes = { OPTION0_TIME, OPTION1_TIME, OPTION2_TIME, OPTION3_TIME, OPTION4_TIME };

        for (int i = 0; i < optionsText.Length; i++)
        {
            if (i < optionTimes.Length && optionsText[i] != null)
            {
                // �������b������ݒ�
                optionsText[i].text = $"{optionTimes[i]}";
            }
            else if (optionsText[i] != null)
            {
                // �͈͊O�܂��͑Ή����鎞�Ԃ��Ȃ��ꍇ
                optionsText[i].text = "null";
            }
        }
    }

    private void Init()
    {

        this.gameObject.transform.position = DEFAULT_POSITION;
        remainingTime = countDownTime;  // �c�莞�Ԃ������l�ɖ߂�

        UpdateCountDownText();          // �����l��\��
    }

    private void Update()
    {
        if (!ExecuteManager.Instance.GetIsExecute())
        {
            Init();
            return;
        }

        this.gameObject.transform.position = EXECUTE_POSITION;

        remainingTime -= Time.deltaTime;

        if(remainingTime < 0)
        {
            remainingTime = 0;
            player.SetIsDead();
        }

        UpdateCountDownText();
    }

    private void UpdateCountDownText()
    {
        countDownText.text = ((int)remainingTime).ToString("D3");
    }

    private void HideAllTimeOptions()
    {
        foreach (var option in timeOptions)
        {
            if(option != null)
            {
                option.SetActive(false);
            }
        }
    }

    private void ToggleSelectedOption()
    {
        isOpen = !isOpen;

        foreach(var option in timeOptions)
        {
            if(option != null)
            {
                option.SetActive(isOpen);     // �S�Ă̗v�f��\��
            }
        }
    }

    protected override void OnPointerDown()
    {
        base.OnClick();

        // �I�����̕\��/��\����؂�ւ���
        ToggleSelectedOption();
    }

    public void SetSelectedIndex(int index)
    {
        this.selectedIndex = index;

        switch (index)
        {
            case 0:
                countDownTime = OPTION0_TIME;
                break;

            case 1:
                countDownTime = OPTION1_TIME;
                break;

            case 2:
                countDownTime = OPTION2_TIME;
                break;

            case 3:
                countDownTime = OPTION3_TIME;
                break;

            case 4:
                countDownTime = OPTION4_TIME;
                break;

            default:
                Debug.LogError("�\�����Ă��Ȃ��C���f�b�N�X���Q�Ƃ���܂���");
                break;
        }

        ToggleSelectedOption();
    }
}
