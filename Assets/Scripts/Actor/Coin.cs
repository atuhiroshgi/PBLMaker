using UnityEngine;

public class Coin : Actor
{
    [SerializeField, Header("êGÇÍÇΩÇ∆Ç´Ç…ìæÇÁÇÍÇÈÉXÉRÉA")]
    private int numberOfCoins;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool isGotten;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        col.enabled = true;
        col.isTrigger = true;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute() && !isGotten)
        {
            col.enabled = true;
        }
        else if(!ExecuteManager.Instance.GetIsExecute())
        {
            sr.enabled = true;
            col.enabled = false;
            isGotten = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ExecuteManager.Instance.GetIsExecute())
        {
            isGotten = true;
            ScoreCounter.Instance.AddScore(numberOfCoins);
            Delete();
        }
    }

    private void Delete()
    {
        sr.enabled = false;
        col.enabled = false;
    }
}
