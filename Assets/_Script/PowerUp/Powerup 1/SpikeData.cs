using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeData : MonoBehaviour
{
    [SerializeField] private Collider[] collider_this;
    [SerializeField] private float flt_DestroyTime;
    public  float flt_Damage;
    public float flt_Force;

    public void SetSpikeData(float flt_Damage, float flt_Force) {

        Destroy(this.gameObject, flt_DestroyTime);
        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
        Invoke("DelayCollderEnbled", 1);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            if (other.transform.localPosition.x - transform.localPosition.x > 0) {
                enemyTrigger.SethitByBullet(flt_Damage, flt_Force, transform.right);
            }
            else {
                enemyTrigger.SethitByBullet(flt_Damage, flt_Force, -transform.right);
            }
        }
    }

    private void DelayCollderEnbled() {
        for (int i = 0; i < collider_this.Length; i++) {
            collider_this[i].enabled = false;
        }
    }




}
