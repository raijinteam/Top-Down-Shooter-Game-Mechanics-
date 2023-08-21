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
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;

   

    [Header("Vfx")]
    [SerializeField] private GameObject obj_Aura;


    private void OnEnable() {
        SetPowerUp();
        damageIncreased.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeForThisPowerUpTime);
    }

    private void SetCoolDown() {
        flt_DelayOfTweSpherCast += 0.01f * flt_DelayOfTweSpherCast * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void OnDisable() {
        damageIncreased.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }

    private void SetDamage() {

        flt_Damage += 0.01f * flt_Damage * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void Update() {
       
        PowerUpHandler();
       
        ChargeAura();
       
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
           
                Vector3 direction = (all_Collider[i].transform.position - transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
          
            all_Collider[i].GetComponent<EnemyTrigger>().SethitByAura(flt_Damage,flt_Force,direction);
           
        }
    }

    private void PowerUpHandler() {
        
        flt_CurrentTimeForThisPowerUpTime += Time.deltaTime;
        if (flt_CurrentTimeForThisPowerUpTime > flt_MaxTimeForThisPowerUpTime) {
            this.gameObject.SetActive(false);
            obj_Aura.gameObject.SetActive(false);
        }
    }

    public void SetPowerUp() {
      
        flt_CurrentTimeForThisPowerUpTime = 0;
        flt_CurrentTimeForSphereCast = 0;
        obj_Aura.gameObject.SetActive(true);
    }

    

    
}
