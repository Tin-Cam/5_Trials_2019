using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public float healthUnitRatio; //Determines the scale of a single unit of health on the game screen
    private float length;

	void Start () {
        //TODO: Add function that stops healthbar reseting on harder difficulties
    }

    public void addHealth(float value)
    {
        length += value * healthUnitRatio;
        updateBar();
    }

    public void setHealth(float value)
    {
        length = value * healthUnitRatio;
        updateBar();
    }

    private void updateBar()
    {
        if (length < 0)
            length = 0;
        transform.localScale = new Vector3(length, transform.localScale.y, transform.localScale.z);
    }
}
