﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Actions : _ActionBase
{
    public Laser laser;

    private Boss3_Controller controller;
    private GameObject player;

    public void Init()
    {
        controller = GetComponent<Boss3_Controller>();
        player = controller.player;

        StartCoroutine(ShootLaser());
    }

    //ACTIONS ---------------------

    public IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(1);
        ShootLaser(player.transform.position);        
    }

    public void ShootLaser(Vector3 target)
    {
        Vector2 targetVector = target - transform.position;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;

        Quaternion targetAngle = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(laser, transform.position, targetAngle);
    }

    public override void DefaultState()
    {
        
    }
}