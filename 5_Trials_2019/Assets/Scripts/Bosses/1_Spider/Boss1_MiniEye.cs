using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_MiniEye : MonoBehaviour
{
    public bool isEyeOpen;
    public bool isActing;
    public float stunTime;
    private Animator animator;
    private AudioManager audioManager;

    public GameObject projectile;
    public GameObject player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = AudioManager.instance;
    }

    //Shoots the eye if it can
    public void shoot()
    {
        if (!isEyeOpen | isActing)
            return;

        audioManager.Play("Boss_Shoot"); //Sound is played here to avoid it playing during a desperation attack (Reduces it's volume during it #savetheears)
        ForceShoot();
    }

    //Shoots eye regardless of its state
    public void ForceShoot()
    {
        //Creates the projectile
        GameObject tempProjectile;
        tempProjectile = Instantiate(projectile, transform.position, transform.rotation);

        //Calculates the direction of the player
        Vector2 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();

        //'Fires' the projectile
        tempProjectile.GetComponent<Projectile_Simple>().direction = direction;
    }

    //Toggle the eye
    public void openEye()
    {
        this.isEyeOpen = !isEyeOpen;
        openEye(isEyeOpen);
    }

    //Sets the state of the eye
    public void openEye(bool isEyeOpen)
    {
        this.isEyeOpen = isEyeOpen;
        if (isEyeOpen)
            animator.SetTrigger("Open");
        else if (!isEyeOpen)
            animator.SetTrigger("Closed");
    }

    public void chargeEye()
    {
        StopCoroutine(Stun());
        isEyeOpen = false;
        isActing = true;
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Closed");
        animator.SetTrigger("Charging_Closed");
    }

    //Eye will deactivate for a few second after being hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
            Hit();
    }

    private void Hit()
    {
        if (!isEyeOpen | isActing)
            return;
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        animator.ResetTrigger("Open");
        animator.ResetTrigger("Closed");

        openEye(false);
        isActing = true;
        yield return new WaitForSeconds(stunTime);
        openEye(true);
        isActing = false;
    }
}
