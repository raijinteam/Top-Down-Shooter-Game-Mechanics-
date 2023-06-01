using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAuraPowerUp : MonoBehaviour
{
    [Header("PowerUp Data")]
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    [Header("PowerUp Handler")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float Range;
    [SerializeField] private float flt_DelayOfTweSpherCast;
    [SerializeField] private float flt_MaxTimeForThisPowerUpTime;
    [SerializeField] private float flt_CurrentTimeForThisPowerUpTime;
    [SerializeField] private float flt_CurrentTimeForSphereCast;
    [SerializeField] private bool ispowerUpStart;

    [Header("Vfx")]
    [SerializeField] private GameObject obj_Aura;
   


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPowerUp();
        }
        PowerUpHandler();
        if (ispowerUpStart) {
            ChargeAura();
        }
    }

    private void ChargeAura() {
        flt_CurrentTimeForSphereCast += Time.deltaTime;
        if (flt_CurrentTimeForSphereCast > flt_DelayOfTweSpherCast) {
            SetSphereCast();
            flt_CurrentTimeForSphereCast = 0;
        }
    }

    private void SetSphereCast() {
        Collider[] all_Collider = Physics.OverlapSphere(transform.position, Range, enemyLayer);

        for (int i = 0; i < all_Collider.Length; i++) {
            //if (all_Collider[i].TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {


                Vector3 direction = (all_Collider[i].transform.position - transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                all_Collider[i].GetComponent<EnemyTrigger>().SethitByAura(flt_Damage,flt_Force,direction);
           // }
        }
    }

    private void PowerUpHandler() {
        if (!ispowerUpStart) {
            return;
        }
        flt_CurrentTimeForThisPowerUpTime += Time.deltaTime;
        if (flt_CurrentTimeForThisPowerUpTime > flt_MaxTimeForThisPowerUpTime) {
            ispowerUpStart = false;
            obj_Aura.gameObject.SetActive(false);
        }
    }

    public void SetPowerUp() {
        ispowerUpStart = true;
        flt_CurrentTimeForThisPowerUpTime = 0;
        flt_CurrentTimeForSphereCast = 0;
        obj_Aura.gameObject.SetActive(true);
    }

    

    
}
