using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagePowerUp : MonoBehaviour
{
    [Header("Componant")]
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerData playerData;
    [Header("Powerup Data")]
    [SerializeField] private float flt_PowerupTime;
    [SerializeField] private float flt_CurrentTime;
    //[SerializeField] private bool isPowerUpStart;

    [Header("PowerupProperty")]
    [SerializeField] private float flt_DamagePercentage;
    [SerializeField] private float flt_ForcePercentage;
    [SerializeField] private float flt_FireratePercentage;

  
    private void Update() {  
        RagePowerUpTimer();
    }

    public void SetRagePowerup() {
       // isPowerUpStart = true;
        flt_CurrentTime = 0;

        float damage = playerData.flt_Damage + ((playerData.flt_Damage * flt_DamagePercentage) / 100);
        float force = playerData.flt_Force + ((playerData.flt_Force * flt_ForcePercentage / 100));
        float firerate = playerData.flt_Firerate - ((playerData.flt_Firerate * flt_FireratePercentage / 100));
        playerShooting.SetPowerupTime(damage, force, firerate);

        gameObject.SetActive(true);
    }

    private void RagePowerUpTimer() {
      
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_PowerupTime) {

            playerShooting.DefaultBulletData();
            flt_CurrentTime = 0;
            gameObject.SetActive(false);
        }
    }

   
}
