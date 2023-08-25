using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrigger : EnemyTrigger
{
    [Header("Camponant")]
   
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
 


   
    [SerializeField] private Collider collider_Body;
    [SerializeField] private float flt_DelayOfTerrorShot;
    [SerializeField] private float flt_Range;
    [SerializeField] private GameObject obj_TerroVFx;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    public override void SetHitOrbitBullet(float flt_Damage, float flt_Force, Vector3 direction) {
        enemyHealth.TakeDamage(flt_Damage);
        batMovement.BatKnockBack(direction, flt_Force);
    }
    public override void SetHitByTerrorShot(float flt_Damage, float flt_Force) {

       
        obj_TerroVFx.gameObject.SetActive(true);
        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
        StartCoroutine(DelayOfTerrorShot());
    }

    private IEnumerator DelayOfTerrorShot() {
        yield return new WaitForSeconds(flt_DelayOfTerrorShot);

        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_Range);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

                Vector3 direction = (all_Collider[i].transform.position - transform.position).normalized;
                enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                                        new Vector3(direction.x, 0, direction.z).normalized);
            }
        }
        obj_TerroVFx.gameObject.SetActive(false);
    }
    public override void SetDamageBlast() {
        enemyHealth.EnemyDie();
    }
    public override void SetHitbyMolotovePowerUp(float flt_Damage) {
        enemyHealth.SetHitbyMolotovePowerUp(flt_Damage);
    }
    public override void StopMolotovePowerUp() {
        enemyHealth.StopMolotovePowerUp();
    }
    public override void SethitByBullet(float flt_Damage, float _flt_Force, Vector3 _Direction) {
        enemyHealth.TakeDamage(flt_Damage);
        Debug.Log("BatForce" + _flt_Force);
        batMovement.BatKnockBack(_Direction, _flt_Force);
    }

    public override void SetHitTidalWave(Transform transform, float Damage) {
        batMovement.HitByBlackHole(transform);
        collider_Body.enabled = false;
       
    }
    public override void StopHitTidalWave(float damage) {
        collider_Body.enabled = true;
        enemyHealth.transform.parent = null;
        enemyHealth.TakeDamage(damage);

        batMovement.TidalWaveEnded();

    }

    public override void HitByBlackHole(Transform transform) {

        batMovement.HitByBlackHole(transform);
        collider_Body.enabled = false;
       
    }
   
    public override void BlackHoleBlast(float knockBackForce, Vector3 direction) {
        collider_Body.enabled = true;
        enemyHealth.transform.parent = null;

        batMovement.BatKnockBack(direction, knockBackForce);
    }
    public override void SethitByAura(float flt_Damage, float flt_Force, Vector3 direction) {

        enemyHealth.TakeDamage(flt_Damage);
        batMovement.BatKnockBack(direction, flt_Force);
    }

    

}
