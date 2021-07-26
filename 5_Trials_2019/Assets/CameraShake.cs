using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //Referenced from Brakeys
    public void TimelineShake(float magnitude){
        StartCoroutine(ShakeCO(0.5f, magnitude));
    }

    public IEnumerator ShakeCO(float duration, float magnitude){
        Vector3 originalPosition = transform.localPosition;

        float time = 0f;

        while(time < duration){
            float x = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);

            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
