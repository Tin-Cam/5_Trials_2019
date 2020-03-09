using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _ActionBase : MonoBehaviour
{
    public List<string> actionList = new List<string>();

    public bool isActing;
    public SpriteRenderer desperationSprite;
    private static readonly float fadeSpeed = (float)0.05;
    private static readonly float fadeAlpha = (float)0.5;

    private Vector2Int last2Actions;

    //Makes the boss perform an action
    public void StartAction(int action)
    {
        if (action > actionList.Count)
        {
            Debug.LogError("Error starting an action: Action number cannot be higher than actionList's count");
            return;
        }

        StartCoroutine(actionList[action]);
    }

    //Stops the boss from performing any action
    public void StopActing()
    {
        StopAllCoroutines();
        isActing = false;
    }

    //Ensures no action is used 3 times in a row
    public bool CheckLastAction(int action)
    {
        int actionSum = action * 2;
        int vecSum = last2Actions.x + last2Actions.y;

        if (actionSum == vecSum)
            return true;

        last2Actions.y = last2Actions.x;
        last2Actions.x = action;

        return false;
    }

    public void ShowDesperationFilter(bool isVisible)
    {
        if (isVisible)
            StartCoroutine(FadeOut());
        if (!isVisible)
            StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        //desperationSprite.color = new Color(0, 0, 0, fadeAlpha);

        float alpha = desperationSprite.color.a;

        while (desperationSprite.color.a > 0)
        {
            alpha -= fadeSpeed;
            if (alpha < 0)
                alpha = 0;
            desperationSprite.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator FadeOut()
    {
        desperationSprite.color = new Color(0, 0, 0, 0);

        float alpha = desperationSprite.color.a;

        while (desperationSprite.color.a < fadeAlpha)
        {
            alpha += fadeSpeed;
            if (alpha > fadeAlpha)
                alpha = fadeAlpha;

            desperationSprite.color = new Color(0, 0, 0, alpha);
            yield return new WaitForFixedUpdate();
        }
    }

    abstract public void DefaultState();
}
