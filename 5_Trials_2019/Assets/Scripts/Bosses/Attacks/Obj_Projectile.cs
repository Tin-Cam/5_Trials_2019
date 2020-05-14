using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Obj_Projectile : ScriptableObject
{
    public int damage;
    public float moveSpeed;
    public float lifeTime;
    public Vector2 killDistance;
}
