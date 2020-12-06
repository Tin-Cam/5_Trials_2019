using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject healthBar;
    public GameObject healthBarHider;
    public Text healthBarText;
    public bool hideHealthBar = false;

    private float unit; //Determines the scale of a single unit of health on the game screen
    private float length;
    private float maxLength;
    private float maxHealth;

    private Vector3 origin;

    private string healthBarName;

    private Image barImage;

	void Awake () {
        maxLength = healthBar.transform.localScale.x;
        barImage = healthBar.GetComponent<Image>();

        healthBarName = healthBarText.text;
        SetBarHider(hideHealthBar);

        origin = transform.position;
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

    public void ShakeBar(){
        StartCoroutine(ShakeBarCO());
    }

    public IEnumerator ShakeBarCO(){
        float t = 3;
        float speed = 10;
        float intensity = 5;

        while(t > 0){
            t -= Time.deltaTime * speed;

            float x = Mathf.Sin(t * speed) * intensity;
            x += origin.x;

            Vector3 position = new Vector3(x, origin.y, origin.z);
            transform.position = position;
            yield return new WaitForFixedUpdate();
        }
        transform.position = origin;
    }

    public void SetBarHider(bool value){
        hideHealthBar = value;
        healthBarHider.SetActive(value);
        if(!value)
            healthBarText.text = healthBarName;
        else
            healthBarText.text = "??????";
    }
}
