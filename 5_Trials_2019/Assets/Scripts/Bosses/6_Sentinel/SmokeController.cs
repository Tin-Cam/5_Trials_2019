using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public List<GameObject> smokeArray = new List<GameObject>();
    public int smokeCount;

    void Start()
    {
        UpdateSmoke();
    }

    public void SetSmoke(int count){
        smokeCount = count;
        UpdateSmoke();
    }

    private void UpdateSmoke(){
        for(int i = 0; i < smokeArray.Count; i++){
            bool isActive = ((i + 1) <= smokeCount);
            smokeArray[i].SetActive(isActive);
        }
    }
}
