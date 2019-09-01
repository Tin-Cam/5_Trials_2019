using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _BossBase : MonoBehaviour
{
    [Space(15)]
    public float health;
    public HealthBar healthBar;

    protected Rigidbody2D rig;

    public int phase; //Value determines how the boss behaves
    public List<string> actionList = new List<string>();


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

    public void pickAction()
    {
        StartCoroutine(actionList[0]);
    }

    public void pickAction(int action)
    { 
        if (action > actionList.Count)
            return;

        StartCoroutine(actionList[action]);
    }

    

    protected void takeDamage(float value)
    {
        health -= value;
        healthBar.addOrSubtractHealth(-1);
        if (health <= 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
            playerAttacked();
    }

    abstract protected void playerAttacked();
}
