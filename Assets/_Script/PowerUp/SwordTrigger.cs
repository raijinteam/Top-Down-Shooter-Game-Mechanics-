using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    public void SetSwordData(float flt_Damage , float flt_Force) {

        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent <EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

            Vector3 direction = (other.gameObject.transform.position - transform.
               position).normalized;
            enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                new Vector3(direction.x, 0, direction.z).normalized);
        }
    }
}
