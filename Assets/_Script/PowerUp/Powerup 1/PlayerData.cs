using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    [SerializeField] private PlayerShooting playerShooting;
    public Transform body;
    public float flt_Damage;
    public float flt_Force;
    public float flt_Firerate;
    public float IncreasedPersantageHealth;

    [Header("Critical  Damage")]
    public float persantage_CriticalDamageChance;
    public float persantage_CriticalDamage;

    [Header("DamageIncreased")]
    public float damageIncreasePercent;

    [Header("FireRateDecreased")]
    public float DecreasedPersentageFireRate;

    [Header("CoolDownPercentage")]
    public float DecreasedPersentageCoolDown;

    [Header("MicroMissile Data")]
    public int MissileCounter;
    public float flt_MissileDamage;
    public int PersantageOfMissileSpawn;

    [Header("Rechoest Data")]
    public int RechoestCounter;
    public int Rechoest_damagePersantage;

    [Header("DealthBlowData")]
    public int persantageOfDelathBow;


    public float DecreasedCoolDown(float flt_FireRate) {
        float flt_CurrentFireRate = flt_FireRate + ((DecreasedPersentageCoolDown / 100) *
                          flt_FireRate);
        return flt_CurrentFireRate;
    }
   public float GetDecreasedFirerate(float fireRate) {
        float flt_CurrentFireRate = fireRate + ((DecreasedPersentageFireRate / 100) *
                            fireRate);
        return flt_CurrentFireRate;
    }

   public float GetIncreasedDamage(float currentDamage) {

        // DamageIncreased
        currentDamage = currentDamage + ((damageIncreasePercent / 100) *
                             currentDamage);

       

       

        return currentDamage;
   }

    public void RemoveShootingTarget(Transform enemy) {
        //if (playerShooting.target == enemy.gameObject) {
        //    playerShooting.target = transform.gameObject;
        //}
    }
}
