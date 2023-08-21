using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb_Player;
    [SerializeField] private PlayerShooting playerShooting;
    public PlayerVFX playerVFX;
    public Transform body;

    [Header("Player Current Stats")]
    public float flt_Damage;
    public float flt_Force;
    public float flt_Firerate;
    public float IncreasedPersantageHealth;

    [Header("Critical  Damage")]
    public float persantage_CriticalDamageChance;
    public float persantage_CriticalDamage;

   
    [Header("MicroMissile Data")]
    public int MissileCounter;
    public float flt_MissileDamage;

    [Header("MultyShot")]
    public float multyShotDamage;
   

    [Header("Rechoest Data")]
    public int RechoestCounter;
    public int Rechoest_damagePersantage;

    [Header("DealthBlowData")]
    public int persantageOfDelathBow;


    [SerializeField] private DamageIncreasedPowerUp damageIncreased;

    private void OnEnable() {
        damageIncreased.setDamageIncreased += SetDamage;
    }

    private void OnDisable() {
        damageIncreased.setDamageIncreased -= SetDamage;
    }

    public void PlayerJumpAnimationCompleted() {
        playerVFX.PlayJumpLandEffect();
        rb_Player.useGravity = true;
    }

    private void SetDamage() {

        flt_Damage += flt_Damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
      
    }
}
