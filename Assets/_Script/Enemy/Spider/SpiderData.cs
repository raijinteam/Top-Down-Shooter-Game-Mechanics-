using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderData : EnemyHandler { 
    
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;


    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private SlimeMovement slimeMovement;
    [SerializeField] private SlimeAttacking slimeAttacking;

   
   
    public int damage;
    public float flt_knockBackForce;
    private GameObject obj_Indicator;

    [Header("Chain VFx")]
    [SerializeField] private GameObject Obj_ChainVfx;

    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {
        enemyHealth.SetLaserAffacted(damage);
        slimeMovement.KnockBack(_Direction, force);
    }

    public override void StartChainPowerUp() {
        slimeMovement.SetInVisible();
        slimeAttacking.isvisible = false;
        Obj_ChainVfx.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        slimeMovement.SetVisible();
        slimeAttacking.isvisible = true;
        Obj_ChainVfx.gameObject.SetActive(false);
    }

    public override void SetInVisible() {
        slimeMovement.SetInVisible();
        slimeAttacking.isvisible = false;
    }
    public override void SetVisible() {
        slimeMovement.SetVisible();
        slimeAttacking.isvisible = true;
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
        slimeMovement.enabled = true;
        slimeAttacking.enabled = true;
       

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.obj_Indicator = indicator;
    }

    public void DestroyIndicator() {
        Destroy(obj_Indicator);
    }


}
