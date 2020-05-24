  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Action : _ActionBase
{
    public bool desperation = false;

    public int shootCycleCount;
    public float shootRate;
    public float shootMaxDelay;

    public int bombCycleCount;
    public float bombRate;

    public int maxAttackRNG;
    private int defaultAttackRNG;

    public Projectile bomb;

    private SnakeMovement head;

    private Boss4_Controller controller;
    private Boss4_Move move;
    private GameObject player;

    public void Init(SnakeMovement head)
    {
        controller = GetComponent<Boss4_Controller>();
        move = GetComponent<Boss4_Move>();

        this.head = head;
        player = controller.player;

        actionList.Add("DesperationMode");
        actionList.Add("Shoot");
        actionList.Add("LayBomb");
    }

    public void DecideActions()
    {
        int rng;

        //Decide if desperate
        if (!desperation)
        {
            //Return if the boss goes desperate
        }

        //Decide if attacking
        rng = Random.Range(0, maxAttackRNG);
        Debug.Log("RNG is :" + rng);
        switch (rng)
        {
            //Shoot
            case 0:
                StartCoroutine(PrepareShoot());
                break;
            case 1:
                StartCoroutine(PrepareShoot());
                break;

            //Bomb
            case 2:
                StartCoroutine(PrepareBomb());
                break;
            case 3:
                StartCoroutine(PrepareBomb());
                break;

            //Shoot and Bomb
            case 4:
                StartCoroutine(PrepareShoot());
                StartCoroutine(PrepareBomb());
                break;

            default:
                Debug.LogError("Unexpected attack RNG value: " + rng);
                break;

        }

        
    }

    public IEnumerator PrepareShoot()
    {
        yield return WaitForMoveCount(2);

        //Calculates the delay of attack
        float rng = Random.Range(1, shootMaxDelay);
        yield return new WaitForSeconds(rng);

        StartCoroutine(Shoot());
    }

    public IEnumerator PrepareBomb()
    {
        yield return WaitForMoveCount(3);
        StartCoroutine(LayBomb());
    }

    private IEnumerator WaitForMoveCount(int targetCount)
    {
        while(move.moveCount < targetCount)
            yield return new WaitForFixedUpdate();
    }

    public IEnumerator DesperationMode()
    {
        if (desperation)
            yield break;

        ShowDesperationFilter(true);

        move.SetSpeed(2);
        yield return new WaitForSeconds(1);

        //Toggle desperation mode
        desperation = true;
        move.SetSpeed(10);

        Color color = new Color(0.5f, 0.5f, 1, 1);
        foreach(Transform bodyPart in head.body)
        {
            SpriteRenderer render = bodyPart.GetComponent<SpriteRenderer>();
            //render.color = color;
        }

        yield return new WaitForSeconds(5);
        ExitDesperationMode();
    }

    private void ExitDesperationMode()
    {
        ShowDesperationFilter(false);
        move.ResetSpeed();
        desperation = false;
        foreach (Transform bodyPart in head.body)
        {
            SpriteRenderer render = bodyPart.GetComponent<SpriteRenderer>();
            render.color = Color.white;
        }
    }

    public IEnumerator Shoot()
    {
        int counter = 0;

        //Cycles through each segment (except the last one) and attempts to shoot it
        while (counter < shootCycleCount)
        {
            for (int i = 1; i < head.body.Count; i++)
            {
                Segment segment = head.body[i - 1].GetComponent<Segment>();
                if (!segment.IsDestroyed())
                {
                    segment.Shoot(player.transform.position);
                    counter++;
                    yield return new WaitForSeconds(shootRate);
                }
            }
        }
    }

    public IEnumerator LayBomb()
    {
        for (int i = 0; i < bombCycleCount; i++)
        {
            Segment segment = head.body[head.body.Count - 1].GetComponent<Segment>();
            segment.Shoot(player.transform.position);
            yield return new WaitForSeconds(bombRate);
        }
    }

    public bool CheckSegmentBounds(int segmentNO)
    {
        Segment segment = head.body[segmentNO].GetComponent<Segment>();
        return segment.CheckShootBounds();
    }


    public override void DefaultState()
    {
        ExitDesperationMode();
        
    }
}
