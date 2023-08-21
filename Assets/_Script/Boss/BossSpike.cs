using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpike : MonoBehaviour {

    public float flt_Damage;
    public  float flt_Force;
    [SerializeField] private float flt_DestroyTime;
    [SerializeField] private Collider myCollider;
    [SerializeField] private float collderDisableTime = 0.5f;

    public void SetSpike(float flt_Damage, float flt_Force) {
        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
        myCollider.enabled = true;
        StartCoroutine(DeisbleGameObject());
    }

    private IEnumerator DeisbleGameObject() {
        yield return new WaitForSeconds(collderDisableTime);
        myCollider.enabled = false;
        yield return new WaitForSeconds(flt_DestroyTime);
        this.gameObject.SetActive(false);

    }
}

