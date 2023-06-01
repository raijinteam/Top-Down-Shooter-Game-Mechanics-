using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class orcEnemyData : EnemyHandler {
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;
    public float flt_knockBackForce;
    public float damage;


    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private OrcEnemyMovement orcEnemyMovement;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private GameObject Obj_Indicator;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {

        enemyHealth.SetLaserAffacted(damage);
        orcEnemyMovement.OrcKnockBack(_Direction, force);
    }
    public override void StartChainPowerUp() {
        orcEnemyMovement.SetInVisible();
        obj_ChainVfx.gameObject.SetActive(true);
    }

    public override void StopChainPowerUp() {
        orcEnemyMovement.SetVisible();
        obj_ChainVfx.gameObject.SetActive(false);
    }
    public override void SetInVisible() {
        orcEnemyMovement.SetInVisible();
    }
    public override void SetVisible() {
        orcEnemyMovement.SetVisible();
    }
    public void SetAllScriptData() {

        SetData();
        LevelManager.instance.ADDListOfEnemy(this.gameObject);
        ExpandSpherCast();
        Instantiate(obj_Explotion, transform.position, obj_Explotion.transform.rotation);
    }

    private void ExpandSpherCast() {
        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_RangeOfSpheareCast);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].TryGetComponent<PlayerMovement>(out PlayerMovement playermovemet)) {

                float distance = Mathf.Abs(Vector3.Distance(transform.position, all_Collider[i].transform.position));
                flt_KnockBackForce = ((flt_MinKnockBackForce - flt_MaxKnockBackForce) / flt_RangeOfSpheareCast) * distance +
                    flt_MaxKnockBackForce;
                Vector3 knockBackDirection = (transform.position - all_Collider[i].transform.position).normalized;
                if (knockBackDirection == Vector3.zero) {
                    Vector3 randomDirection = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)).normalized;
                    knockBackDirection = randomDirection;
                }
                playermovemet.KnockBack(-knockBackDirection, flt_KnockBackForce);
            }
        }
    }
    public void SetData() {
        enemyHealth.enabled = true;
        orcEnemyMovement.enabled = true;
        //navMeshAgent.enabled = true;

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.Obj_Indicator = indicator;
    }

    public void DestroyIndicator() {
        Destroy(Obj_Indicator);
    }
}

