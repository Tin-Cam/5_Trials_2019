using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Action : _ActionBase
{
    public GameObject sweepIndicator;
    public float sweepIndicatorSpeed;

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

    public IEnumerator ShootAtPlayer()
    {
        shooter.Shoot(player.transform.position);
        yield break;
    }

    public IEnumerator SweepShoot(float angle)
    {
        //Calculates speed
        float speed = sweepIndicatorSpeed * controller.bossLevel;
        float delay = 1 / controller.bossLevel;

        //Shows indicator that telgraphs attack ----------------------------

        //Start and end positions
        Quaternion start = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion end = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        //Displays and initilises the indicator
        float t = -0.1f;
        sweepIndicator.transform.position = transform.position;
        sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);
        yield return new WaitForSeconds(0.2f * delay);

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

            //Shoots multiple shots
            shooter.Shoot(direction);
            shooter.Shoot(direction * Quaternion.AngleAxis(offsetAngle, Vector3.forward));
            shooter.Shoot(direction * Quaternion.AngleAxis(offsetAngle, Vector3.back));

            t += (1f / shots);
            yield return new WaitForSeconds(0.1f);
        }
        DefaultState();
    }

    public IEnumerator SpinShoot()
    {
        float speed = 1;
        float times = 20;
        float t = 0;

        for(int i = 0; i < times; i++)
        {
            Quaternion angle = Quaternion.AngleAxis(t, Vector3.forward);
        }
        yield break;
    }

    public override void DefaultState()
    {
        sweepIndicator.transform.position = new Vector3(0, 50, 0);
    }
}
