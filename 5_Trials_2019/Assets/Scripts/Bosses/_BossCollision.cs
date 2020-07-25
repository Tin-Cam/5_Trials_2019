using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BossCollision : MonoBehaviour
{
    public _BossBase bossController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
            bossController.BossHurt();
    }
}
