using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _BossBase : MonoBehaviour
{
    public GameManager gameManager;

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
 

    protected void TakeDamage(float value)
    {
        health -= value;
        healthBar.addOrSubtractHealth(-1);
        CheckHealth();
        if (health <= 0)
            StartDeath();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
            BossHurt();
    }

    protected void SetAITimer()
    {
        aiTimerCount = Random.Range(0, maxAiTimerRngValue);
    }

    public void Die()
    {
        gameManager.BossDefeated();

        Destroy(this.gameObject);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    abstract protected void Init();
    abstract protected void BossHurt();
    abstract protected void IncreasePhase();
    abstract protected void CheckHealth();
    abstract protected void StartDeath();
    abstract public void DefaultState();
}
