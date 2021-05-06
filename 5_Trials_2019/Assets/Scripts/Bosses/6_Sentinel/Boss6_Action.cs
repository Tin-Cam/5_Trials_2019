using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Action : _ActionBase
{
    public GameObject sweepIndicator;
    public float sweepIndicatorSpeed;

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
    [HideInInspector()]
    public bool holdGrid;
    private List<GameObject> grid = new List<GameObject>();
    public float maxGridSpeed;
    public float maxGridSpeedCap;  
    
    [Space(10)]
    public float sineWaveSpeed;
    public float sineTime;
    public List<GameObject> sineUpper = new List<GameObject>();
    public List<GameObject> sineLower = new List<GameObject>();
    public AnimatorScripts sineWaveAnimatorScripts;
    [HideInInspector()]
    public bool holdSine;
    

    private Boss6_Controller controller;
    private Boss6_Move move;
    private AnimatorScripts animatorScripts;
    private AudioManager audioManager;
    private ShootScripts shooter;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        move = GetComponent<Boss6_Move>();
        animatorScripts = GetComponent<AnimatorScripts>();
        audioManager = AudioManager.instance;
        shooter = GetComponent<ShootScripts>();
        
        player = controller.player;

        actionList.Add("ShootAtPlayer");
        actionList.Add("SweepShoot");
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

    public IEnumerator SweepShoot(float angle, int spreadCount)
    {
        animatorScripts.PlayAnimation("Boss_6_Focus", 0);

        //Calculates speed
        float speed = sweepIndicatorSpeed * controller.bossLevel;

        //Shows indicator that telgraphs attack ----------------------------

        //Start and end positions
        Quaternion start = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion end = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //Displays and initilises the indicator
        float t = -0.1f;
        //audioManager.Play("Boss3_Indicate", 0.75f, 1.5f);
        sweepIndicator.transform.position = transform.position;
        sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);
        yield return new WaitForSeconds(0.2f * GetLevelFraction());

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
        animatorScripts.PlayAnimation("Boss_6_Shoot", 0);
        int shots = 7;
        float offsetAngle = 30;
        t = 0;
        
        //Sweep shoots over room
        while (t < 1)
        {
            Quaternion direction = Quaternion.Lerp(start, end, t);

            //Shoots
            ShootSpread(direction, spreadCount, offsetAngle);

            t += (1f / shots);
            yield return new WaitForSeconds(0.1f);
        }
        DefaultState();
    }

    public IEnumerator SpinShoot()
    {
        float speed = spinSpeed;
        int arms = spinArms;

        int shots = (int)(spinShots * controller.bossLevel);
        float degrees = Random.Range(0, 90);
        animatorScripts.PlayAnimation("Boss_6_Shoot", 0);
        for(int i = 0; i < shots; i++)
        {
            Quaternion direction = Quaternion.AngleAxis(degrees, Vector3.forward);

            float offsetAngle = 360 / arms;

            ShootSpread(direction, arms, offsetAngle);

            degrees += speed;
            yield return new WaitForSeconds(spinShootRate);
        }
        yield break;
    }

    public IEnumerator AimAtPlayer()
    {
        animatorScripts.PlayAnimation("Boss_6_Focus", 0);
        float time = indicateTime * GetLevelFraction();
        float timeDivision = time / 3;
        int soundCount = 3;

        while (time > 0)
        {
            if(time < timeDivision * soundCount){
                audioManager.Play("Button_Press", 0.75f, 1.5f);
                soundCount--;
            }

            targetIndicator.transform.position = player.transform.position;
            time -= Time.deltaTime;            

            yield return new WaitForFixedUpdate();
        }
        DefaultState();
    }

    public IEnumerator AimAtStage()
    {
        animatorScripts.PlayAnimation("Boss_6_Focus", 0);
        float speed = 40;
        float aimHoldTime = 0.5f * GetLevelFraction();
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
        grid = new List<GameObject>();

        //Generate Grid
        float gap = 3;
        float perRow = ((screenBoundsRadius * 2) / gap);
        audioManager.Play("Door_Open", 0.75f, 0.75f);
        for (int i = 0; i < perRow; i++)
        {
            for (int j = 0; j < perRow; j++)
            {
                Vector3 point = new Vector3(i * gap, j * gap, 0);
                point = LoopScreenBounds(point);
                GameObject projectile = Instantiate(gridAttackProjectile, point, new Quaternion());
                grid.Add(projectile);
            }
        }
        //Animation
        yield return new WaitForSeconds(1);
        for (int i = 0; i < grid.Count; i++)
        {
            Animator tempAnimator = grid[i].GetComponent<Animator>();
            tempAnimator.Play("GridAttack_Idle");
        }

        //Move grid
        float maxSpeed = maxGridSpeed * controller.bossLevel;
        if(maxSpeed > maxGridSpeedCap)
            maxSpeed = maxGridSpeedCap;

        float speed = 0;
        float acceleration = 1;

        float angle = 30 * Random.Range(1, 11);
        //Ensures the grid doesn't move parallel to the axis
        if(angle % 90 == 0)
            angle += 30;

        Quaternion directionQat = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 direction = directionQat * Vector3.right;

        holdGrid = true;
        while (holdGrid)
        {
            for(int i = 0; i < grid.Count; i++)
            {
                Vector3 pos = grid[i].transform.position;
                pos = pos + direction * speed * Time.deltaTime;
                grid[i].transform.position = LoopScreenBounds(pos);
            }
            
            if (speed >= maxSpeed)
                speed = maxSpeed;
            else
                speed += acceleration * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Exit Animation
        for (int i = 0; i < grid.Count; i++)
        {
            Animator tempAnimator = grid[i].GetComponent<Animator>();  
            tempAnimator.Play("GridAttack_Exit");

            //Wait for the last grid projectile to finish its animation
            if(i == grid.Count - 1){
                yield return new WaitForEndOfFrame();
                while (tempAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                {
                    yield return new WaitForFixedUpdate();
                }
            }                
        }
        DefaultState();
    }

    //Sarts the sine attack by bringing the sine object into scene
    public IEnumerator StartSineAttack()
    {
        DefaultSine();       
        StartCoroutine(MoveSineWave());

        float speedMultiplier = 0.5f;
        sineWaveAnimatorScripts.animator.SetFloat("Speed", speedMultiplier);

        audioManager.Play("Boss_Charge", 0.75f, 0.5f);
        yield return sineWaveAnimatorScripts.PlayWholeAnimation("SineWave_Entry", 0);
    }

    //Holds then finishes Sine Attack
    public IEnumerator DoSineAttack()
    {
        yield return new WaitForSeconds(sineTime * GetLevelFraction());
        yield return sineWaveAnimatorScripts.PlayWholeAnimation("SineWave_Exit", 0);
        holdSine = false;
    }

    //MODE 0 - Waves go in opposite directions
    //MODE 1 - Waves go in same direction
    private IEnumerator MoveSineWave()
    {
        //int mode = Random.Range(0, 2);
        int mode = 1;
        float speed = sineWaveSpeed * controller.bossLevel;
        holdSine = true;
        while(holdSine){

            foreach(GameObject sine in sineUpper){
                Vector3 pos = sine.transform.position;
                pos.x = pos.x + speed * Time.deltaTime;
                sine.transform.position = LoopScreenBounds(pos);
            }

            foreach(GameObject sine in sineLower){
                Vector3 pos = sine.transform.position;
                pos.x = pos.x + speed * Time.deltaTime * (float)(-1 + (2 * mode));
                sine.transform.position = LoopScreenBounds(pos);
            }
            yield return new WaitForFixedUpdate();
        }      
    }

    private void DefaultSine(){
        float spacing = 6;

        for(int i = 0; i < sineUpper.Count; i++){
            Vector3 pos = new Vector3( i * spacing, sineUpper[i].transform.position.y, 0);
            sineUpper[i].transform.position = LoopScreenBounds(pos);
        }
        for(int i = 0; i < sineLower.Count; i++){
            Vector3 pos = new Vector3( i * spacing + (spacing/2), sineLower[i].transform.position.y, 0);
            sineLower[i].transform.position = LoopScreenBounds(pos);
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

    private float GetLevelFraction()
    {
        return 1 / controller.bossLevel;
    }

    private void ClearGrid()
    {
        if (grid.Count < 1)
            return;

        foreach(GameObject projectile in grid)
        {
            Destroy(projectile);
        }
    }

    public override void DefaultState()
    {
        Vector3 defaultPos = new Vector3(0, 50, 0);

        sweepIndicator.transform.position = defaultPos;
        targetIndicator.transform.position = defaultPos;
        targetIndicator.transform.localScale = new Vector3(1, 1, 1);

        holdGrid = false;
        ClearGrid();

        sineWaveAnimatorScripts.PlayAnimation("SineWave_Inactive", 0);
        holdSine = false;
        ShowDesperationFilter(false);
    }
}
