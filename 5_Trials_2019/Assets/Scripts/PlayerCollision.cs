using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public SpriteRenderer render;
    public PlayerMove playerMove;

    public bool godMode;
    public bool isInvincible;

    public float flashAmount = 10;
    public float flashSpeed = 1;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = playerMove.gameManager;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (godMode | isInvincible)
            return;

        if (other.tag == "Projectile")
        {
            gameManager.PlayerTakeDamage(1);

            if (gameManager.playerHealth <= 0)
                return;

            Vector2 playerPosition = transform.position;
            Vector2 direction = playerPosition - other.ClosestPoint(playerPosition);

            StartCoroutine(playerMove.knockBack(direction, playerMove.defaultKnockBack));
            StartCoroutine(invincible());
        }
    }

    //Makes the player invincible; used when they're hit
    public IEnumerator invincible()
    {
        isInvincible = true;
        for (int i = 0; i < flashAmount; i++)
        {
            flashRed();
            yield return new WaitForSeconds(flashSpeed);
        }
        render.color = Color.white;
        isInvincible = false;
    }

    //Alternates player's color from white to red when hit
    private void flashRed()
    {
        if (render.color.Equals(Color.red))
        {
            render.color = Color.white;
            return;
        }

        if (render.color.Equals(Color.white))
        {
            render.color = Color.red;
            return;
        }
    }
}
