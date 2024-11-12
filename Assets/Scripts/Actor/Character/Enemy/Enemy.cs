using UnityEngine;

public class Enemy : Character
{
    protected override void Death()
    {
        spriteRenderer.enabled = false;
        col.isTrigger = true;
    }
}
