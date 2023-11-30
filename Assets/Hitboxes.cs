using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitboxes : MonoBehaviour
{
    public bool deactivateHitboxes = false;
    public GameObject hitEffect;


    public void CreateHitEffect(Vector3 effectPos, float scaling)
    {
        GameObject effect = Instantiate(hitEffect, effectPos, Quaternion.identity);
        int rng = Random.Range(1, 4);
        effect.transform.localScale *= scaling;
        effect.GetComponent<Animator>().Play("hitFX_" + rng);
        Destroy(effect, 0.5f);

    }
}
