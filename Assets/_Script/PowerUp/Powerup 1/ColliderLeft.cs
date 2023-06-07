using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLeft : MonoBehaviour
{
    [SerializeField] private SpikeData spikeData;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

            enemyTrigger.SethitByBullet(spikeData.flt_Damage, spikeData.flt_Force, -transform.right);
        }
    }
}
