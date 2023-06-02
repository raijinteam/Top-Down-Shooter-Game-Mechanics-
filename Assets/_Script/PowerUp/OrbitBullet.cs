using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBullet : MonoBehaviour
{
   
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Range;
     private float flt_DelayOfSpheracast = 2;
  

   


    public void SetBulletData(float flt_Range, float force, float flt_Damage) {
        this.flt_Range = flt_Range;
        this.flt_Force = force;
        this.flt_Damage = flt_Damage;
       
        StartCoroutine(Sphercast());
    }

    private IEnumerator Sphercast() {
        yield return new WaitForSeconds(flt_DelayOfSpheracast);

        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_Range);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
                Vector3 direction = (all_Collider[i].transform.position - transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                enemyTrigger.SetHitOrbitBullet(flt_Damage, flt_Force, direction);
            }
        }

        yield return new WaitForSeconds(flt_DelayOfSpheracast);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, flt_Range);
    }
}
