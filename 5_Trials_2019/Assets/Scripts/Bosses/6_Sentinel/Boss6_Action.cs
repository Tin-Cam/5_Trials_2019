using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Action : _ActionBase
{
    public GameObject sweepIndicator;
    public float sweepIndicatorSpeed;
    public int sweepSpreadCount;

    [Space(10)]
    public float spinSpeed;
    public float spinShootRate;
    public int spinShots;
    public int spinArms;

    [Space(10)]
    public GameObject targetIndicator;
    public GameObject targetPrefab;
    public float indicateTime;
    public int targetShootTimes;
    public float targetShootRate;
    [HideInInspector()]
    public bool holdTargeting;

    [Space(10)]
    public Bounds targetStageBounds;
    public float screenBoundsRadius;
    public GameObject gridAttackProjectile;

    private Boss6_Controller controller;
    private Boss6_Move move;
    private AudioManager audioManager;
    private ShootScripts shooter;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        move = GetComponent<Boss6_Move>();
        audioManager = AudioManager.instance;
        shooter = GetComponent<ShootScripts>();

        player = controller.player;

        actionList.Add("ShootAtPlayer");
        actionList.Add("SweepShoot");

        StartCoroutine(GridAttack());
    }

    //Shoots at player
    public void ShootAtPlayer()
    {
        shooter.Shoot(player.transform.position);
    }

    //Shoots at player with a spread shot
    public void ShootAtPlayer(int projectileAmount, float offsetAngle)
    {
        if (projectileAmount < 2)
        {
            shooter.Shoot(player.transform.position);
            return;
        }

        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;

        float angleToPlayer = (Mathf.Atan2(y, x)) * Mathf.Rad2Deg;

        ShootSpread(Quaternion.AngleAxis(angleToPlayer, Vector3.forward), projectileAmount, offsetAngle);

        return;
    }

    public void ShootSpread(Quaternion direction, int projectileAmount, float offsetAngle)
    {
        if(projectileAmount < 2)
        {
            shooter.Shoot(direction);
            return;
        }

        float angle = -((offsetAngle * (projectileAmount - 1)) / 2);   

        for (int i = 0; i < projectileAmount; i++)
        {
            shooter.Shoot(direction * Quaternion.AngleAxis(angle, Vector3.forward));
            angle += offsetAngle;
        }
    }

    public IEnumerator SweepShoot(float angle)
    {
        //Calculates speed
        float speed = sweepIndicatorSpeed * controller.bossLevel;

        //Shows indicator that telgraphs attack ----------------------------

        //Start and end positions
        Quaternion start = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion end = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //Displays and initilises the indicator
        float t = -0.1f;
        sweepIndicator.transform.position = transform.position;
        sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);
        yield return new WaitForSeconds(0.2f * GetDelay());

        //Indicator sweeps (pivots) over room
        while (t <= 1.1f)
        {
            sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);

            t += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        //Hides indicator
        yield return new WaitForSeconds(0.1f);
        sweepIndicator.transform.position = new Vector3(0, 50, 0);
        yield return new WaitForSeconds(0.5f);


        //Shoots ----------------------------------------------------------------
        int shots = 7;
        float offsetAngle = 30;
        t = 0;
        
        //Sweep shoots over room
        while (t < 1)
        {
            Quaternion direction = Quaternion.Lerp(start, end, t);

            //Shoots
            ShootSpread(direction, sweepSpreadCount, offsetAngle);

            t += (1f / shots);
            yield return new WaitForSeconds(0.1f);
        }
        DefaultState();
    }

    public IEnumerator SpinShoot()
    {
        float speed = spinSpeed * controller.bossLevel;
        int shots = (int)(20 * controller.bossLevel);
        float degrees = 0;

        for(int i = 0; i < shots; i++)
        {
            Quaternion direction = Quaternion.AngleAxis(degrees, Vector3.forward);

            float offsetAngle = 360 / spinArms;

            ShootSpread(direction, spinArms, offsetAngle);

            degrees += speed;
            yield return new WaitForSeconds(spinShootRate * GetDelay());
        }
        yield break;
    }

    public IEnumerator AimAtPlayer()
    {
        float time = indicateTime * GetDelay();
        while (time > 0)
        {
            targetIndicator.transform.position = player.transform.position;
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        DefaultState();
    }

    //Maybe find a way to calculate a safe zone near the player
    public IEnumerator AimAtStage()
    {
        float speed = 40;
        float aimHoldTime = 0.5f * GetDelay();
        int[] nodes = { 1, 2, 4, 3 };

        targetIndicator.transform.localScale = new Vector3(2, 2, 1);
        targetIndicator.transform.position = move.InnerNodes[nodes[0]].position;

        audioManager.Play("Button_Press", 0.75f, 1.5f);
        yield return new WaitForSeconds(aimHoldTime);
        for (int i = 0; i < 3; i++)
        {
            Vector3 nextPos = move.InnerNodes[nodes[i + 1]].position;
            while(targetIndicator.transform.position != nextPos)
            {
                targetIndicator.transform.position = Vector3.MoveTowards(targetIndicator.transform.position, nextPos, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            audioManager.Play("Button_Press", 0.75f, 1.5f);
            yield return new WaitForSeconds(aimHoldTime);
        }
        DefaultState();
    }

    public IEnumerator TargetPlayer()
    {
        int times = (int)(targetShootTimes * controller.bossLevel);

        while (times > 0)
        {
            Instantiate(targetPrefab, player.transform.position, new Quaternion());
            times--;
            yield return new WaitForSeconds(targetShootRate);
        }
    }

    public IEnumerator TargetPlayerHold()
    {
        holdTargeting = true;
        while (holdTargeting)
        {
            Instantiate(targetPrefab, player.transform.position, new Quaternion());
            yield return new WaitForSeconds(targetShootRate);
        }
    }

    public IEnumerator TargetStage()
    {
        int times = (int)(100 * controller.bossLevel);

        Bounds safeZone = CalculateSafeZone();

        while (times > 0)
        {
            Vector2 pos = safeZone.center;

            while (safeZone.Contains(pos))
            {
                float x = Random.Range(targetStageBounds.min.x, targetStageBounds.max.x);
                float y = Random.Range(targetStageBounds.min.y, targetStageBounds.max.y);
                pos = new Vector2(x, y);
            }
            
            Instantiate(targetPrefab, pos, new Quaternion());
            times--;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private Bounds CalculateSafeZone()
    {
        float safeZoneMaxDistance = 5;
        float safeZoneradius = 3;

        Vector3 playerPos = player.transform.position;

        float maxX = playerPos.x + safeZoneMaxDistance;
        float maxY = playerPos.y + safeZoneMaxDistance;
        float minX = playerPos.x - safeZoneMaxDistance;
        float minY = playerPos.y - safeZoneMaxDistance;

        //Ensures the max and mins are within the stage bounds
        //Maxs
        if (maxX > targetStageBounds.max.x)
            maxX = targetStageBounds.max.x;

        if (maxY > targetStageBounds.max.y)
            maxY = targetStageBounds.max.y;

        //Mins
        if (minX < targetStageBounds.min.x)
            minX = targetStageBounds.min.x;

        if (minY < targetStageBounds.min.y)
            minY = targetStageBounds.min.y;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Bounds result = new Bounds(new Vector3(x, y, 0), new Vector3(safeZoneradius, safeZoneradius, 0));

        return result;
    }

    public IEnumerator GridAttack()
    {
        List<GameObject> grid = new List<GameObject>();

        float gap = 2;
        int multiplier = 1;

        for (int i = 0; i < screenBoundsRadius * multiplier; i++)
        {
            for (int j = 0; j < screenBoundsRadius * multiplier; j++)
            {
                Vector3 point = new Vector3(i * gap, j * gap, 0);
                point = LoopScreenBounds(point);
                GameObject projectile = Instantiate(gridAttackProjectile, point, new Quaternion());
                grid.Add(projectile);
            }
        }

        while (true)
        {
            for(int i = 0; i < grid.Count; i++)
            {
                grid[i].transform.position = Vector3.MoveTowards(grid[i].transform.position, new Vector3(screenBoundsRadius + 1, grid[i].transform.position.y, 0), 5 * Time.deltaTime);
                grid[i].transform.position = LoopScreenBounds(grid[i].transform.position);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 LoopScreenBounds(Vector3 thing)
    {
        //Check x position
        if (thing.x > screenBoundsRadius)
            thing.x -= screenBoundsRadius * 2;
        else if (thing.x < -screenBoundsRadius)
            thing.x += screenBoundsRadius * 2;

        //Check y position
        if (thing.y > screenBoundsRadius)
            thing.y -= screenBoundsRadius * 2;
        else if (thing.y < -screenBoundsRadius)
            thing.y += screenBoundsRadius * 2;

        return thing;
    }

    private float GetDelay()
    {
        return 1 / controller.bossLevel;
    }

    public override void DefaultState()
    {
        Vector3 defaultPos = new Vector3(0, 50, 0);

        sweepIndicator.transform.position = defaultPos;
        targetIndicator.transform.position = defaultPos;
        targetIndicator.transform.localScale = new Vector3(1, 1, 1);
    }
}
