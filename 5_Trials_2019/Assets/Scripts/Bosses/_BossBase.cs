using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _BossBase : MonoBehaviour
{
    protected _ActionBase actionBase;
    protected _MoveBase moveBase;

    [Space(15)]
    public GameObject player;
    public float health;
    protected float maxHealth;
    public HealthBar healthBar;

    protected Rigidbody2D rig;
    protected Animator animator;

    public int aiTimer;
    protected int aiTimerCount;
    public int maxAiTimerRngValue;
    public bool hasAI = true;

    public int phase; //Value determines how the boss behaves
    public List<string> actionList = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        maxHealth = health;

        healthBar.initHealth(health);
        Init();       
    }

    abstract protected void Init();

    public void StopAction()
    {
        DefaultState();
    }

    public void PickAction(int value)
    {
        SetAITimer();
        actionBase.isActing = true;
        actionBase.StartAction(value);
    }
 

    protected void takeDamage(float value)
    {
        health -= value;
        healthBar.addOrSubtractHealth(-1);
        checkHealth();
        if (health <= 0)
            death();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
            bossHurt();
    }

    protected void SetAITimer()
    {
        aiTimerCount = Random.Range(0, maxAiTimerRngValue);
    }


    abstract protected void bossHurt();
    abstract protected void IncreasePhase();
    abstract protected void checkHealth();
    abstract protected void death();
    abstract public void DefaultState();
}
