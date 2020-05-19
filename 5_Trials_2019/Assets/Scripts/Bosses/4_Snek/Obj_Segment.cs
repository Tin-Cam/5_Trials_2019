using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Segment", menuName = "Boss Objects/Segment")]
public class Obj_Segment : ScriptableObject
{
    public GameObject projectile;
    public int health;
    public Bounds shootBounds;
    public string shootSFX;
}
