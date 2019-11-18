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

    public IEnumerator StartSpin()
    {
        ResetSpin();
        SetRotation();
        yield return new WaitForEndOfFrame();
        animator.Play("Start");
        yield return WaitForAnimation();
        isSpinning = true;
    }

    public IEnumerator EndSpin()
    {   
        animator.Play("End");
        yield return WaitForAnimation();
        isSpinning = false;
        this.gameObject.SetActive(false);
    }

    private float CalculateSpeed()
    {
        float speed = currentSpeed;
        speed += acceleration;

        if (currentSpeed >= Mathf.Abs(maxSpeed))
            return maxSpeed;
        return speed;
    }

    //Sets the laser to a random rotation
    private void SetRotation()
    {
        int rng = Random.Range(0, 3);
        Quaternion rotation = Quaternion.AngleAxis(45 * rng, Vector3.forward);
        this.transform.rotation = rotation;
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
