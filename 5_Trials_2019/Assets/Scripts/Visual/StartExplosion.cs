using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartExplosion : MonoBehaviour
{
    public GameObject explosion;
    public Bounds explodeArea;

    public void ExplodeOnSelf(){
        Explode(transform.position);
    }

    public void ExplodeOverArea(){
        float x = Random.Range(explodeArea.min.x, explodeArea.max.x);
        float y = Random.Range(explodeArea.min.y, explodeArea.max.y);
        Vector2 pos = new Vector2(x, y);

        Explode(pos);
    }

    private void Explode(Vector2 pos){
        AudioManager.instance.Play("Crash", 0.75f, 1f);
        Instantiate(explosion, pos, new Quaternion());
    }
}
