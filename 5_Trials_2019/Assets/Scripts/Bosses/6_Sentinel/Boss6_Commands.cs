using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Commands : MonoBehaviour
{
    private Boss6_Action action;
    private Boss6_Move move;
    private Boss6_Controller controller;
    private AnimatorScripts animatorScripts;

    public bool randomCommands;
    public int idleTime;

    public int sweepShootTimes;
    public int sweepSpreadCount;

    public int circleSpread;
    public float circleOffset;
    public int circleShootTimes;
    public float circleShootDelay;

    public int targetStageTimes;

    public int gridGlides;

    public int maxDespCount;
    private int despCounter;
    private bool despCounterActive;


    private List<string> commandList = new List<string>();
    private Queue<string> commandQueue = new Queue<string>();
    private int nextCommandNumber;

    public void Init()
    {
        action = GetComponent<Boss6_Action>();
        move = GetComponent<Boss6_Move>();
        controller = GetComponent<Boss6_Controller>();
        animatorScripts = GetComponent<AnimatorScripts>();

        ChangeCommandList(0);

        StartCoroutine(Intro());
    }

    public IEnumerator Intro()
    {
        yield return new WaitForSeconds(1);
        yield return move.Exit();
        StartCoroutine(NextCommand());
    }

    public IEnumerator NextCommand()
    {
        DefaultState();
        //ResetAnimation();
        
        if (commandQueue.Count != 0)
        {
            //Uses queued command if any
            yield return StartCoroutine(commandQueue.Dequeue());
        }
        else
        {
            //Picks a command if nothing is queued
            nextCommandNumber = NextCommandNumber();
            yield return StartCoroutine(commandList[nextCommandNumber]);
        }

        //ResetAnimation();
        IncrementDespCount();

        StartCoroutine(NextCommand());
    }

    public IEnumerator Idle(float multiplier)
    {
        float time = idleTime * multiplier * GetLevelFraction();
        yield return new WaitForSeconds(time);
    }

    public IEnumerator Idle()
    {
        yield return Idle(1);
    }

    public IEnumerator SweepShoot()
    {
        int times = (int)(sweepShootTimes * controller.bossLevel);       

        int rng = Random.Range(1, 5);
        int lastNode = rng;
        int spreadCount = sweepSpreadCount;
        yield return move.EnterToInner(rng);
        yield return action.SweepShoot((-90) * (rng - 1), spreadCount);

        for (int i = 0; i < (times - 1); i++)
        {           
            while (rng == lastNode)
                rng = Random.Range(1, 5);
            lastNode = rng;

            Vector3 node = move.InnerNodes[rng].position;
            yield return move.Teleport(node);
            yield return action.SweepShoot((-90) * (rng - 1), spreadCount);
        }
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        yield return Idle();
        yield return move.Exit();
    }

    public IEnumerator CircleAndShoot()
    {
        //Starts the movement
        yield return move.EnterToCircle();
        StartCoroutine(move.CircleCentre());


        float delay = circleShootDelay * GetLevelFraction();
        int times = (int)(circleShootTimes * controller.bossLevel);
        animatorScripts.PlayAnimation("Boss_6_Focus", 0);
        yield return new WaitForSeconds(delay);
        //Starts shooting
        animatorScripts.PlayAnimation("Boss_6_Shoot", 0);
        for (int i = 0; i < times; i++)
        {
            action.ShootAtPlayer(circleSpread, circleOffset);
            yield return new WaitForSeconds(0.2f);
        }
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        yield return Idle(1.5f);

        //Exits the action
        move.isCircling = false;
        yield return move.Exit();
    }

    public IEnumerator SpinShoot()
    {       
        yield return move.ExitTeleport(Vector3.zero);
        animatorScripts.PlayAnimation("Boss_6_Focus", 0);
        yield return Idle(0.75f);
        yield return action.SpinShoot();
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        yield return Idle(0.5f);
        yield return move.EnterTeleport(new Vector3(0, 20, 0));
        yield return Idle(0.5f);
    }

    public IEnumerator TargetPlayer()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        StartCoroutine(move.Exit());
        yield return action.TargetPlayer();
    }

    public IEnumerator TargetStage()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtStage();
        StartCoroutine(move.Exit());

        int times = (int) (targetStageTimes * controller.bossLevel);
        for(int i = 0; i < times; i++){
            yield return action.TargetStage();
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator TargetPlayerAndAttack()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        StartCoroutine(action.TargetPlayerHold());
        yield return move.Exit();

        int rng = Random.Range(0, 3);
        switch(rng){
            case 0:
                yield return SweepShoot();
                break;
            case 1:
                yield return CircleAndShoot();
                break;
            case 2:
                yield return SpinShoot();
                break;
            default:
                yield return SweepShoot();
                break;
        }
        action.holdTargeting = false; 
        yield return Idle();
    }

    public IEnumerator TargetPlayerInfinite()
    {
        yield return move.EnterToInner(0);
        yield return new WaitForSeconds(0.2f);
        yield return action.AimAtPlayer();
        StartCoroutine(action.TargetPlayerHold());
        yield return move.Exit();
        yield return Idle();
    }

    public IEnumerator GridAttack()
    {
        //Play animation when gliding

        //Start Attack
        yield return GlideOver();
        animatorScripts.PlayAnimation("Boss_6_Act", 0);
        StartCoroutine(action.GridAttack());
        while(!action.holdGrid)
            yield return new WaitForFixedUpdate();
        
        //Glide over stage X times
        int times = (int)(gridGlides * controller.bossLevel);
        for(int i = 0; i < times; i++)
            yield return GlideOver();

        action.holdGrid = false;
        yield return Idle();
    }

    public IEnumerator GlideOver()
    {
        //Set start position
        float rng = Random.Range(-3, 3);
        Vector2 point = new Vector2(-12, rng);
        transform.position = point;

        //Set and move to end position
        rng = Random.Range(-3, 3);
        point = new Vector2(12, rng);
        yield return move.GlideToPosition(point);
    }

    public IEnumerator DesperationAttack()
    {
        yield return move.MoveToDesperation();
        action.ShowDesperationFilter(true);
        yield return Idle(0.5f);
        animatorScripts.PlayAnimation("Boss_6_Act", 0);
        yield return action.SineAttack();
        action.ShowDesperationFilter(false);
        yield return move.Exit();
        ResetDespCount();
    }

    public void ChangeCommandList(int phase)
    {
        //Always has random commands unless indicated otherwise
        randomCommands = true;
        commandList.Clear();
        float animationSpeed = controller.bossLevel;
        if(animationSpeed < 1)
            animationSpeed = 1;

        animatorScripts.animator.SetFloat("Speed", animationSpeed);

        switch (phase)
        {
            case 0:
                randomCommands = false;
                commandList.Add("CircleAndShoot");
                commandList.Add("SweepShoot");

                break;

            case 1:
                commandQueue.Enqueue("TargetPlayer");
                commandList.Add("SweepShoot");
                commandList.Add("CircleAndShoot");
                commandList.Add("TargetPlayer");
                commandList.Add("SpinShoot");

                sweepSpreadCount = 2;
                break;
            case 2:
                commandList.Add("SweepShoot");
                commandList.Add("TargetPlayer");
                commandList.Add("TargetStage");
                commandList.Add("SpinShoot");

                sweepSpreadCount = 3;
                break;
            case 3:
                commandQueue.Enqueue("GridAttack");

                commandList.Add("SweepShoot");
                commandList.Add("GridAttack");
                commandList.Add("TargetStage");
                commandList.Add("SpinShoot");

                action.spinArms += 2;
                break;
            case 4:
                commandQueue.Enqueue("DesperationAttack");

                commandList.Add("TargetPlayerAndAttack");
                commandList.Add("TargetStage");
                commandList.Add("GridAttack");

                break;
            case 5:
                commandQueue.Clear();
                commandQueue.Enqueue("TargetPlayerInfinite"); 

                commandList.Add("SweepShoot");
                commandList.Add("CircleAndShoot");
                commandList.Add("SpinShoot");
                commandList.Add("GridAttack");

                BreakCommand();
                break;
            default:
                Debug.LogError("Error when setting command list. No commands set for phase " + phase);
                commandList.Add("SweepShoot");
                break;
        }
    }

    private int NextCommandNumber()
    {
        int result = nextCommandNumber;
        result++;

        if (randomCommands)
        {
            result = Random.Range(0, commandList.Count);

            //Failsafe incase random commands is used with only one command
            if (commandList.Count == 1)
                return 0;

            //Ensure a command isn't used twice in a row
            while (result == nextCommandNumber)
                result = Random.Range(0, commandList.Count);

            return result;
        }

        if (result >= commandList.Count)
            return 0;

        return result;
    }

    //Stops all boss commands
    public void StopCommand(){
        StopAllCoroutines();       
        DefaultState();
    }

    //Forces boss to start next command
    private void BreakCommand(){
        StopCommand();
        StartCoroutine(BreakCommandCO());
    }

    private IEnumerator BreakCommandCO(){
        yield return move.Exit();
        StartCoroutine(NextCommand());
    }

    public void IncrementDespCount()
    {
        if (!despCounterActive)
            return;

        despCounter++;

        if (despCounter >= maxDespCount)
        {
            commandQueue.Enqueue("DesperationAttack");
            
            despCounterActive = false;
        }
    }

    public void ResetDespCount()
    {
        despCounter = 0;
        despCounterActive = true;
    }

    private float GetLevelFraction()
    {
        return 1 / controller.bossLevel;
    }

    public void DefaultState()
    {
        action.DefaultState();
        animatorScripts.PlayAnimation("Boss_6_Idle", 0);
        animatorScripts.PlayAnimation("Boss_6_Teleport_Normal", 2);
    }
}