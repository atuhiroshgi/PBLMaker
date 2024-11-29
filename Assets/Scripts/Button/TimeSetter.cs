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

    [SerializeField, Header("Playerの参照")]
    private Player player;
    [SerializeField, Header("残り秒数を表示するテキスト")]
    private TextMeshProUGUI countDownText;
    [SerializeField, Header("選択候補の配列")]
    private GameObject[] timeOptions;
    [SerializeField, Header("選択候補のテキスト")]
    private TextMeshProUGUI[] optionsText;
    [SerializeField, Header("カウントダウン時間(秒)")]
    private int countDownTime = 10;

    private float remainingTime;
    private int selectedIndex;
    private bool isOpen = false;

    private void Awake()
    {
        SetOptionTexts();
        HideAllTimeOptions();           // 選択肢を全て非表示

        // 初期化
        Init();
    }

    private void SetOptionTexts()
    {
        int[] optionTimes = { OPTION0_TIME, OPTION1_TIME, OPTION2_TIME, OPTION3_TIME, OPTION4_TIME };

        for (int i = 0; i < optionsText.Length; i++)
        {
            if (i < optionTimes.Length && optionsText[i] != null)
            {
                // 正しい秒数情報を設定
                optionsText[i].text = $"{optionTimes[i]}";
            }
            else if (optionsText[i] != null)
            {
                // 範囲外または対応する時間がない場合
                optionsText[i].text = "null";
            }
        }
    }

    private void Init()
    {

        this.gameObject.transform.position = DEFAULT_POSITION;
        remainingTime = countDownTime;  // 残り時間を初期値に戻す

        UpdateCountDownText();          // 初期値を表示
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
                option.SetActive(isOpen);     // 全ての要素を表示
            }
        }
    }

    protected override void OnPointerDown()
    {
        base.OnClick();

        // 選択肢の表示/非表示を切り替える
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
                Debug.LogError("予期していないインデックスが参照されました");
                break;
        }

        ToggleSelectedOption();
    }
}
