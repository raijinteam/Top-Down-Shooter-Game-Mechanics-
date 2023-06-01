using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonData : EnemyHandler {
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;
    private GameObject spawn_Indicator;
   
    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttacking enemyAttacking;
    [SerializeField] private AttckHandler attckHandler;
    [SerializeField] private SkeletonCollisionHandler skeletonCollisionHandler;
    [SerializeField] private NavMeshAgent navMeshAgent;

    public int damage = 5;
    public int flt_knockBackForce = 30;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;


    public override void SetHitByLaser(Vector3 _Dircetion ,float _force, float _damage) {
        enemyMovement.KnockBack(_Dircetion, _force);
        enemyHealth.SetLaserAffacted(_damage);
    }

    public override void StopChainPowerUp() {
        enemyMovement.SetVisible();
        enemyAttacking.IsVisible = true;
        obj_ChainVfx.gameObject.SetActive(false);
    }
    public override void StartChainPowerUp() {
        enemyMovement.SetInVisible();
        enemyAttacking.IsVisible = false;
        obj_ChainVfx.gameObject.SetActive(true);
    }
    public override void SetInVisible() {
        enemyMovement.SetInVisible(); 
        enemyAttacking.IsVisible = false;
    }
    public override void SetVisible() {
        enemyMovement.SetVisible();
        enemyAttacking.IsVisible = true;
    }




    public void SetSpawnIndicator(GameObject _Indicator) {
        this.spawn_Indicator = _Indicator;
    }
    public void DestroyIndicator() {
        Destroy(spawn_Indicator);
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
        enemyMovement.enabled = true;
        enemyAttacking.enabled = true;
        attckHandler.enabled = true;
        skeletonCollisionHandler.enabled = true;
        //navMeshAgent.enabled = true;

    }
}
