using UnityEngine;

public class GoalObject : Actor
{
    private Collider2D col;
    private SpriteRenderer sr;
    private bool isPlayerTouched = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        AddGoal();
    }

    private void Update()
    {
        // é¿çsÇ™é~Ç‹Ç¡ÇƒÇ¢ÇÈèÍçáï\é¶Ç≥ÇπÇÈ
        if (!ExecuteManager.Instance.GetIsExecute())
        {
            sr.enabled = true;
            isPlayerTouched = false;
        }
        else
        {
            sr.enabled = !isPlayerTouched;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ExecuteManager.Instance.GetIsExecute())
        {
            sr.enabled = false;
            isPlayerTouched = true;
        }
    }
}
