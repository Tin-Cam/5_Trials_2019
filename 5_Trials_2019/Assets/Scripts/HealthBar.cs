using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    private float unit; //Determines the scale of a single unit of health on the game screen
    private float length;
    private float maxLength;
    private float maxHealth;

	void Start () {
        maxLength = transform.localScale.x;
    }

    public void addOrSubtractHealth(float value)
    {
        length += value * unit;
        updateBar();
    }

    public void initHealth(float value)
    {
        maxHealth = value;
        length = maxLength;
        unit = maxLength / maxHealth;
        updateBar(); 
    }

    private void updateBar()
    {
        if (length < 0)
            length = 0;
        transform.localScale = new Vector3(length, transform.localScale.y, transform.localScale.z);
    }
}
