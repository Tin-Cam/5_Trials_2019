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

    private Boss6_Controller controller;
    private ShootScripts shooter;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        shooter = GetComponent<ShootScripts>();

        player = controller.player;

        actionList.Add("ShootAtPlayer");
        actionList.Add("SweepShoot");
    }

    public void ShootAtPlayer()
    {
        shooter.Shoot(player.transform.position);
    }

    public void ShootAtPlayer(int projectileAmount, float offsetAngle)
    {
        if(projectileAmount < 2)
        {
            shooter.Shoot(player.transform.position);
            return;
        }

        float x = player.transform.position.x - transform.position.x;
        float y = player.transform.position.y - transform.position.y;

        float angleToPlayer = (Mathf.Atan2(y, x)) * Mathf.Rad2Deg;
        float angle;

        angle = angleToPlayer - ((offsetAngle * (projectileAmount - 1)) / 2);   

        Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
        for (int i = 0; i < projectileAmount; i++)
        {
            shooter.Shoot(Quaternion.AngleAxis(angle, Vector3.forward));
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
            shooter.Shoot(direction);

            //Adds extra bullets per shot
            for (int i = 0; i < sweepSpreadCount; i++)
            {
                shooter.Shoot(direction * Quaternion.AngleAxis(offsetAngle * (i + 1), Vector3.forward));
                shooter.Shoot(direction * Quaternion.AngleAxis(offsetAngle * (i + 1), Vector3.back));
            }

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

            shooter.Shoot(direction);

            float offsetAngle = 360 / spinArms;
            for (int j = 1; j < spinArms; j++)
            {
                shooter.Shoot(direction * Quaternion.AngleAxis(offsetAngle * j, Vector3.forward));
            }

            degrees += speed;
            yield return new WaitForSeconds(spinShootRate * GetDelay());
        }
        yield break;
    }

    private float GetDelay()
    {
        return 1 / controller.bossLevel;
    }

    public override void DefaultState()
    {
        sweepIndicator.transform.position = new Vector3(0, 50, 0);
    }
}
