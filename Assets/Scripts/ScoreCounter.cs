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

    [SerializeField, Header("�X�R�A�E�B���h�E�̎Q��")]
    private GameObject scoreWindow;
    [SerializeField, Header("�X�R�A�e�L�X�g�̎Q��")]
    private TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake()
    {
        SingletonInit();
    }

    /// <summary>
    /// �V���O���g���̏����ݒ�
    /// </summary>
    private void SingletonInit()
    {
        // ���ɃC���X�^���X�����݂���ꍇ�͎��g��j������
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
