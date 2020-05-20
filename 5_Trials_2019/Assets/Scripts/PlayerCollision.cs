using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public SpriteRenderer render;
    public PlayerMove playerMove;

    public bool godMode;
    public bool isInvincible;
    public float invincibleTime;

    public float hitFlashRate;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = playerMove.gameManager;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Projectile")
            TakeDamge(other);
    }

    public void TakeDamge(Collider2D other)
    {
        if (godMode || isInvincible)
            return;

        gameManager.PlayerTakeDamage(1);

        if (gameManager.playerHealth <= 0)
            return;

        Vector2 playerPosition = transform.position;
        Vector2 direction = playerPosition - other.ClosestPoint(playerPosition);

        StartCoroutine(playerMove.knockBack(direction, playerMove.defaultKnockBack));
        StartCoroutine(Invincible());
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