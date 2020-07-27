using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss6_Action : _ActionBase
{
    private Boss6_Controller controller;
    private ShootScripts shooter;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss6_Controller>();
        shooter = GetComponent<ShootScripts>();

        player = controller.player;

        actionList.Add("ShootAtPlayer");
    }

    public IEnumerator ShootAtPlayer()
    {
        shooter.Shoot(player.transform.position);
        yield break;
    }

    public override void DefaultState()
    {
        throw new System.NotImplementedException();
    }
}
