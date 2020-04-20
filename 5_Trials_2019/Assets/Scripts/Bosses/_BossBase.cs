using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _BossBase : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    public GameManager gameManager;
    public GameObject deathExplosion;
    public SpriteRenderer hitSprite;

    protected _ActionBase actionBase;
    protected _MoveBase moveBase;

    [Space(15)]
    public GameObject player;
    public float health;
    protected float maxHealth;
    public HealthBar healthBar;

    protected Rigidbody2D rig;
    protected Animator animator;
    private SpriteRenderer render;

    public int aiTimer;
    protected int aiTimerCount;
    public int maxAiTimerRngValue;
    public bool hasAI = true;

    public int phase; //Value determines how the boss behaves


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        audioManager = FindObjectOfType<AudioManager>();

        maxHealth = health;

        healthBar.initHealth(health);
        Init();       
    }

    protected void AI()
    {
        if (!hasAI)
            return;

        if (actionBase.isActing)
            return;

        aiTimerCount++;

        if (aiTimerCount >= aiTimer)
        {
            Act();
            //DefaultState();
        }
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

        StartCoroutine(FlashRed());
        audioManager.Play("Boss_Hit");
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

        Instantiate(deathExplosion, transform.position, transform.rotation);
        audioManager.Play("Boss_Death");

        Destroy(this.gameObject);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    //Makes the boss flash red when hit
    private IEnumerator FlashRed()
    {
        hitSprite.enabled = true;
        yield return new WaitForSeconds((float)0.1);
        hitSprite.enabled = false;
    }

    abstract protected void Init();
    abstract protected void Act();
    abstract public void BossHurt();
    abstract protected void IncreasePhase();
    abstract protected void CheckHealth();
    abstract protected void StartDeath();
    abstract public void DefaultState(); 
}
