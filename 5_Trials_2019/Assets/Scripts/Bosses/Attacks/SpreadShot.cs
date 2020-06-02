using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    public List<GameObject> shots = new List<GameObject>();
    public float spreadSpeed;
    public float maxSpaceBetweenShots;
    public int subtraction;

    // Start is called before the first frame update
    void Start()
    {   
        //Grabs all children and puts them in an array
        foreach (Transform item in transform)
        {
            Vector3 position = new Vector3(0, item.position.y, item.position.z);
            item.position = position;
            shots.Add(item.gameObject);
        }

        //Sets which X position each shot should be in
        int half = (int)Mathf.Floor(shots.Count/2);
        for (int i = 0; i < shots.Count; i++)
        {
            float x = i - half;
            SetShots(x, shots[i]);
        }

        //Removes an amount of shots
        for (int i = 0; i < subtraction; i++)
            RemoveShot();
    }

    private void RemoveShot()
    {
        int rng = Random.Range(0, shots.Count);

        GameObject shot = shots[rng];

        shots.Remove(shot);

        Destroy(shot);
    }

    private void SetShots(float x, GameObject shot)
    {
        x *= maxSpaceBetweenShots;
        MoveTowardsX moveTowardsX = shot.GetComponent<MoveTowardsX>();
        moveTowardsX.x = x;
        moveTowardsX.speed = spreadSpeed;
    }
}
