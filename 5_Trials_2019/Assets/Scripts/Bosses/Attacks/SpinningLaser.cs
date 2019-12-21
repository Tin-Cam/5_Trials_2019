using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLaser : MonoBehaviour
{
    public bool isSpinning;
    public float maxSpeed;
    public float acceleration;

    private float currentSpeed;
    private Animator animator;
    private AudioManager audioManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void FixedUpdate()
    {
        if (!isSpinning)
            return;

        currentSpeed = CalculateSpeed();

        transform.Rotate(Vector3.forward * Time.deltaTime * currentSpeed);
    }

    //Starts spinning the laser
    public IEnumerator Spin(float time)
    {
        ResetSpin();
        SetRotation();
        
        int rng = Random.Range(0, 2);
        if (rng == 1)
            ReverseDirection();

        yield return new WaitForEndOfFrame();

        animator.Play("Start");
        yield return WaitForAnimation();

        isSpinning = true;

        yield return new WaitForSeconds(time);
        yield return EndSpin();
    }

    //Ends the spin
    public IEnumerator EndSpin()
    {   
        animator.Play("End");
        yield return WaitForAnimation();
        isSpinning = false;
        this.gameObject.SetActive(false);
    }

    //Calculates and applies acceleration to speed
    private float CalculateSpeed()
    {
        float speed = currentSpeed;
        speed += acceleration;

        //If maxspeed is reached, return maxspeed (returns negative maxSpeed if currentSpeed is negative)
        if (Mathf.Abs(currentSpeed) >= maxSpeed)
            return maxSpeed * Mathf.Sign(acceleration);

        return speed;
    }

    //Sets the laser to a random rotation
    private void SetRotation()
    {
        int rng = Random.Range(0, 3);
        Quaternion rotation = Quaternion.AngleAxis(45 * rng, Vector3.forward);
        this.transform.rotation = rotation;
    }

    public void ReverseDirection()
    {
        acceleration = -acceleration;
    }

    public void RandomDirection()
    {
        int rng = Random.Range(0, 2);
        if (rng == 1)
            ReverseDirection();
    }

    public void ResetSpin()
    {
        currentSpeed = 0;
        isSpinning = false;
    }

    //Finishes when an animation stops playing
    public IEnumerator WaitForAnimation()
    {
        yield return new WaitForEndOfFrame();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
