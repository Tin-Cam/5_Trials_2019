using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Spider : _BossBase
{

    //Movement Variables
    [Space(15)]
    public float speed;
    Vector2 movement;

    public bool isMoving;
    public float moveRange;
    private float moveTime = 0;


    //Eye Variables
    [Space(15)]
    public bool isEyeOpen;
    public Animator animator;

    [Space(15)]
    public bool miniEyesOpen;
    public Boss1_MiniEye miniEyeR;
    public Boss1_MiniEye miniEyeL;

    //Attack Variables
    [Space(15)]
    public GameObject projectile;
    public GameObject player;


    //Action Variables
    [Space(15)]
    public float eyeTime;
    public float attackFrequency;
    public float chargeTime;
    

    // Start is called before the first frame update
    override protected void BossStart()
    {
        movement = new Vector2(rig.position.x, rig.position.y);
        animator = GetComponent<Animator>();
        isMoving = true;
        isActing = false;
        //openMiniEyes(false);

        //Set actions
        actionList.Add("exposeEye");
        actionList.Add("attackShort");
        actionList.Add("attackLong");

        
    }

    // Update is called once per frame
    void Update()
    {
        //pickAction();
        //eyeUpdate();
        ai();
        moveUpdate();
    }

    //ACTIONS-------------------------------

    //Action 0 - Expose Eye (No attacking)
    private IEnumerator exposeEye()
    {
        openEye(true);
        isMoving = false;
        yield return new WaitForSeconds(eyeTime);
        openEye(false);
        isMoving = true;
        isActing = false;
    }

    //Action 1 - Short Attack
    private IEnumerator attackShort()
    {
        openEye(true);
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < attackFrequency; j++)
                yield return new WaitForEndOfFrame();
            shoot();
        } 
        openEye(false);
        isActing = false;
    }

    //Action 2 - Long Attack
    private IEnumerator attackLong()
    {
        
        openEye(true);
        chargeEye(true);
        yield return new WaitForSeconds(chargeTime);
        

        openEye(true);
        for (int i = 0; i < 30; i++)
        {                      
            shoot();
            for(int j = 0; j < 10; j++)
                yield return new WaitForEndOfFrame();
        }
        openEye(false);
        //animator.ResetTrigger("Charging");
        chargeEye(false);
        isActing = false;
    }

    //AI------------------------------------

    private int aiTimer;
    private bool isActing;

    void ai()
    {
        if (!hasAI)
            return;

        if (isActing)
            return;

        aiTimer++;

        if (aiTimer >= 1000)
        {
            phase1();
            aiTimer = 0;
        }
    }

    private int vulCounter;
    private Vector2Int last2Actions;

    void phase1()
    {
        int rng = Random.Range(0, 3);

        while(checkLastAction(rng))
            rng = Random.Range(0, 3);

        isActing = true;

        //Forces the boss to become vulnerable if it hasn't been so after a few cycles
        if (rng == 0 | vulCounter >= 4)
        {
            vulCounter = 0;
            pickAction(0);
            return;
        }

        vulCounter++;
        pickAction(rng);
    }

    override protected void increasePhase()
    {

    }

    override protected void checkHealth()
    {
        if(health <= maxHealth * 0.5 & phase < 1)
        {
            speed *= 2;
            setMiniEyeTimer(500);
            phase++;
        }
    }

    //Ensures no action is used 3 times in a row
    bool checkLastAction(int action)
    {
        int actionSum = action * 2;
        int vecSum = last2Actions.x + last2Actions.y;

        if (actionSum == vecSum)
            return true;

        last2Actions.y = last2Actions.x;
        last2Actions.x = action;

        return false;
    }

    //MISC----------------------------------

    void shoot()
    {
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player
        Vector2 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    void shootMiniEyes()
    {
        miniEyeL.shoot();
        miniEyeR.shoot();
    }

    //Starts the timers for the mini eyes. Eyes will alternate shots
    void setMiniEyeTimer(int timer)
    {
        miniEyeL.setShootTimer(0, timer);
        miniEyeR.setShootTimer(timer / 2, timer);
        openMiniEyes(true);
    }

    void openMiniEyes(bool isOpen)
    {
        miniEyeL.openEye(isOpen);
        miniEyeR.openEye(isOpen);
    }


    //Toggle the eye
    void openEye()
    {
        isEyeOpen = !isEyeOpen;
        animator.SetBool("isOpen", isEyeOpen);
    }

    //Sets the state of the eye
    void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;
        animator.SetBool("isOpen", isEyeOpen);
    }

    void chargeEye(bool isCharging)
    {        
        //openEye(isCharging);
        animator.SetBool("isCharging", isCharging);
    }

    override protected void bossHurt()
    {
        if (isEyeOpen)
            takeDamage(1);
    }

    protected override void death()
    {
        Destroy(miniEyeL.gameObject);
        Destroy(miniEyeR.gameObject);
        Destroy(this.gameObject);
    }

    //Updates the values for movement
    void moveUpdate()
    {
        if (!isMoving)
            return;

        moveTime += 1 * (speed / 10);
        movement = new Vector2(Mathf.Sin(moveTime * Mathf.Deg2Rad) * moveRange, movement.y);
    }

    void FixedUpdate()
    {
        if (!isMoving)
            return;
        _move();
    }

    void _move()
    {
        rig.MovePosition(movement * new Vector2(Time.fixedDeltaTime, 1));
      
    }
}
