using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public SpriteRenderer render;
    public PlayerMove playerMove;

    public bool isInvincible;
    public float invincibleTime;

    public float hitFlashRate;
    public float damagedSpriteTime;

    private GameManager gameManager;
    private Animator animator;

    void Awake()
    {
        gameManager = playerMove.gameManager;
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Projectile")
            TakeDamge(other);
    }

    public void TakeDamge(Collider2D other)
    {
        if (isInvincible)
            return;

        gameManager.PlayerTakeDamage(1);

        if (gameManager.playerHealth <= 0)
            return;

        Vector2 playerPosition = transform.position;
        Vector2 direction = playerPosition - other.ClosestPoint(playerPosition);

        StartCoroutine(playerMove.knockBack(direction, playerMove.defaultKnockBack));
        StartCoroutine(DamageAnimation());
        StartCoroutine(Invincible());
    }

    public IEnumerator DamageAnimation()
    {
        animator.SetBool("Damaged", true);
        yield return new WaitForSeconds(damagedSpriteTime);
        animator.SetBool("Damaged", false);
    }

    //Makes the player invincible; used when they're hit
    public IEnumerator Invincible()
    {
        isInvincible = true;
        StartCoroutine(FlashRed());
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator FlashRed()
    {
        while (isInvincible)
        {
            render.color = Color.red;
            yield return new WaitForSeconds(hitFlashRate);
            render.color = Color.white;
            yield return new WaitForSeconds(hitFlashRate);
        }
    }
}