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

    public IEnumerator SweepShoot()
    {
        float angle = 0;
        float speed = sweepIndicatorSpeed * controller.bossLevel;

        Quaternion start = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion end = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        float t = -0.1f;

        sweepIndicator.transform.position = transform.position;
        sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);

        yield return new WaitForSeconds(0.2f);
        while (t <= 1.1f)
        {

            sweepIndicator.transform.rotation = Quaternion.SlerpUnclamped(start, end, t);

            t += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.1f);
        sweepIndicator.transform.position = new Vector3(0, 50, 0);

        yield return new WaitForSeconds(0.5f);
        t = 0;
        int shots = 5;

        while (t < 1)
        {
            Quaternion direction = Quaternion.Lerp(start, end, t);
            shooter.Shoot(direction);

            t += (1f / shots);
            yield return new WaitForSeconds(0.1f);
        }
        DefaultState();
    }


    public override void DefaultState()
    {
        sweepIndicator.transform.position = new Vector3(0, 50, 0);
    }
}
