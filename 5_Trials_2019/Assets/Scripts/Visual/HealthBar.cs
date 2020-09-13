using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject healthBar;

    private float unit; //Determines the scale of a single unit of health on the game screen
    private float length;
    private float maxLength;
    private float maxHealth;

    private Image barImage;

	void Awake () {
        maxLength = healthBar.transform.localScale.x;
        barImage = healthBar.GetComponent<Image>();
    }

    public void addOrSubtractHealth(float value)
    {
        length += value * unit;
        updateBar();
    }

    public void initHealth(float value)
    {
        //Handles a bug
        if(maxLength == 0)
            maxLength = healthBar.transform.localScale.x;

        maxHealth = value;
        length = maxLength;
        unit = maxLength / maxHealth;
        updateBar();
    }

    public void SetHealth(float amount){
        length = amount * unit;
        updateBar();
    }

    private void updateBar()
    {
        if (length < 0)
            length = 0;
        healthBar.transform.localScale = new Vector3(length, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    //Making it flash looks awful. Maybe try changing the colour or shaking it instead
    public void FlashBar(){
        //StartCoroutine(FlashBarCO());
    }

    private IEnumerator FlashBarCO(){
        float time = 0.2f;
        float flashCount = 4;
        float timeDivision = time / flashCount;

        while(time > 0){
            if(time < timeDivision * flashCount){
                barImage.enabled = !barImage.enabled;
                flashCount--;
            }
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        barImage.enabled = true;
    }
}
