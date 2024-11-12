using UnityEngine;

public class Character : Actor
{
    protected SpriteRenderer spriteRenderer;
    protected Collider2D col;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// €‚ñ‚¾‚Æ‚«‚ÉŒÄ‚Î‚ê‚éƒƒ\ƒbƒh
    /// </summary>
    protected virtual void Death()
    {
    }
}
