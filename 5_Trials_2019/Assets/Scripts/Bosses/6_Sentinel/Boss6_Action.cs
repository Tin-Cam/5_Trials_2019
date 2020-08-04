using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Action : _ActionBase
{
    public GameObject sweepIndicator;

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

        sweepIndicator.transform.position = transform.position;
        sweepIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        yield return new WaitForSeconds(1);

        Quaternion start = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion end = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        float t = 0;

        while (sweepIndicator.transform.rotation != end)
        {
            sweepIndicator.transform.rotation = Quaternion.Slerp(start, end, t);

            t += Time.deltaTime * 5;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);
        DefaultState();
    }

    public override void DefaultState()
    {
        sweepIndicator.transform.position = new Vector3(0, 50, 0);
    }
}
