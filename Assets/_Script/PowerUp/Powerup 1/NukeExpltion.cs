using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeExpltion : MonoBehaviour
{
    private float flt_RangeOfSpheareCast;
    private float flt_MinKnockBackForce;
    private float flt_MaxKnockBackForce;
    private float flt_KnockBackForce;
    private float flt_Damage;
    [SerializeField] private LayerMask layerMask;

    public void SetSpehreCastData(float flt_Range, float force, float flt_Damage) {

        flt_RangeOfSpheareCast = flt_Range;
        flt_MaxKnockBackForce = force;
        flt_MinKnockBackForce = 0;
        this.flt_Damage = flt_Damage;
        ExpandSpherCast();
    }
    private void ExpandSpherCast() {
        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_RangeOfSpheareCast
                ,layerMask);

        for (int i = 0; i < all_Collider.Length; i++) {

            float distance = Mathf.Abs(Vector3.Distance(transform.position, all_Collider[i].transform.position));
            flt_KnockBackForce = ((flt_MinKnockBackForce - flt_MaxKnockBackForce) / flt_RangeOfSpheareCast) * distance +
                flt_MaxKnockBackForce;
            Vector3 knockBackDirection = ( all_Collider[i].transform.position - transform.position).normalized;

            if (knockBackDirection == Vector3.zero) {
                Vector3 randomDirection = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)).normalized;
                knockBackDirection = randomDirection;
            }
            else {
                knockBackDirection = new Vector3(knockBackDirection.x, 0, knockBackDirection.z).normalized;
            }




                all_Collider[i].GetComponent<EnemyTrigger>()
                    .SethitByBullet(flt_Damage, flt_KnockBackForce, knockBackDirection);
            
        }
    }
}
