using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORCTrigger : EnemyTrigger
{
    [Header("Camponant")]

    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private OrcEnemyMovement orcEnemyMotion;
    [SerializeField] private Rigidbody enemyRb;
    [SerializeField] private GameObject obj_WaterParicle;


    private string tag_Water = "Water";
    [SerializeField]private Collider collider_Body;
    [SerializeField] private float flt_DelayOfTerrorShot;
    [SerializeField] private float flt_Range;
    [SerializeField] private GameObject obj_TerroVFx;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

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
    public override void SetHitTidalWave(Transform transform) {
        orcEnemyMotion.SethitByTidal(transform);
        collider_Body.enabled = false;
        enemyRb.useGravity = false;
    }
    public override void StopHitTidalWave() {

        collider_Body.enabled = true;
        enemyHealth.transform.parent = null;
        enemyRb.useGravity = true;
    }
    public override void SethitByAura(float flt_Damage, float flt_Force, Vector3 direction) {

        enemyHealth.TakeDamage(flt_Damage);
        orcEnemyMotion.SethitByAura(direction, flt_Force);
    }
    public override void SethitByBullet(float flt_Damage, float _flt_Force, Vector3 _Direction) {

        enemyHealth.TakeDamage(flt_Damage);
        orcEnemyMotion.OrcKnockBack(_Direction, _flt_Force);

    }


    public override void HitByBlackHole(Transform transform) {

        orcEnemyMotion.HitByBlackHole(transform);
        collider_Body.enabled = false;
        enemyRb.useGravity = false;

    }

    public override void BlackHoleBlast() {

        collider_Body.enabled = true;
        enemyHealth.transform.parent = null;
        enemyRb.useGravity = true;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag(tag_Water)) {

            Instantiate(obj_WaterParicle, transform.position, transform.rotation);
            LevelManager.instance.RemoveListOfEnemy(transform.parent.parent.gameObject);
        }
       
    }
}
