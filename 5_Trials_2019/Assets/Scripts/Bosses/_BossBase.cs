﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _BossBase : MonoBehaviour
{
    public float health;
    public HealthBar healthBar;

    protected Rigidbody2D rig;

    public int phase; //Value determines how the boss behaves

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        healthBar.initHealth(health);
        BossStart();
    }

    abstract protected void BossStart();

    // Update is called once per frame
    void Update()
    {

    }

    void takeDamage(float value)
    {
        health -= value;
        healthBar.addOrSubtractHealth(-1);
        if (health <= 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        takeDamage(1);
    }

}
