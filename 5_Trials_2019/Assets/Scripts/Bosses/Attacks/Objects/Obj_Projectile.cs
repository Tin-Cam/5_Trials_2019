using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Obj_Projectile : ScriptableObject
{
    public float moveSpeed;
    public Vector2 killDistance;
}
