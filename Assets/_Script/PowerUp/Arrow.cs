using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float flt_Speed;
    [SerializeField] private float flt_Damage;
    public void SetBulletData(float flt_Damage) {
        flt_Damage = this.flt_Damage;
    }

    private void Update() {
        transform.Translate(Vector3.down * flt_Speed * Time.deltaTime, Space.World);
        
    }
    

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            enemyTrigger.SethitByBullet(flt_Damage, 0, Vector3.zero);
        }
        if (other.CompareTag("Water")) {
            Destroy(gameObject);
        }
    }
  
}
