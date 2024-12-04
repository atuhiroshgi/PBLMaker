using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private static ScoreCounter instance;

    public static ScoreCounter Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject scoreCounter = new GameObject("ScoreCounter");
                instance = scoreCounter.AddComponent<ScoreCounter>();
                DontDestroyOnLoad(scoreCounter);
            }
            return instance;
        }
    }

    [SerializeField, Header("スコアウィンドウの参照")]
    private GameObject scoreWindow;
    [SerializeField, Header("スコアテキストの参照")]
    private TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake()
    {
        SingletonInit();
    }

    /// <summary>
    /// シングルトンの初期設定
    /// </summary>
    private void SingletonInit()
    {
        // 既にインスタンスが存在する場合は自身を破棄する
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute())
        {
            UpdateText();
            scoreWindow.SetActive(true);
        }
        else
        {
            score = 0;
            scoreWindow.SetActive(false);
        }
    }

    private void UpdateText()
    {
        scoreText.text = this.score.ToString("D7");
    }

    public void SetScore(int score)
    {
        this.score += score;
        UpdateText();
    }

    public int GetScore()
    {
        return this.score;
    }
}
