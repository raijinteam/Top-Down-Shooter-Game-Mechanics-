using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisblePowerup : MonoBehaviour
{
    [Header("Componanat")]
    [SerializeField] private Collider body_Collider;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;


    [Header("Invisble Data")]
    [SerializeField] private int flt_DamagePercentage;
    [SerializeField] private int flt_ForcePercentage;
    [SerializeField] private int flt_FireratePercentage;


    [Header("PowerUp Cantroller")]
    [SerializeField] private float flt_MaxTimeInVisible;
    [SerializeField] private float flt_CurrentTimeInvisible;

  
    
    [SerializeField] private GameObject obj_PowerUp;

    private void OnEnable() {
        SetInvisiblePowerup();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeInVisible);
    }

    private void Update() {
        

        InvisblePowerUpHandler();
    }

    private void InvisblePowerUpHandler() {
      
      
        flt_CurrentTimeInvisible += Time.deltaTime;
        if (flt_CurrentTimeInvisible > flt_MaxTimeInVisible) {
    
            obj_PowerUp.SetActive(false);
            All_EnemyVisible();
            this.gameObject.SetActive(false);
        }
    }

    private void MakeAllEnemyInVisible() {

        GameManager.instance.IsInVisblePowerUpActive = true;
        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {
            if (GameManager.instance.list_ActiveEnemies[i].
                TryGetComponent<EnemyHandler>(out EnemyHandler enemyHandler)) {
                enemyHandler.SetInVisible();
            }
        }
    }

    private void All_EnemyVisible() {

        GameManager.instance.IsInVisblePowerUpActive = false;
        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {
            if (GameManager.instance.list_ActiveEnemies[i].
                TryGetComponent<EnemyHandler>(out EnemyHandler enemyHandler)) {
                enemyHandler.SetVisible();
                //body_Collider.enabled = true;
                playerShooting.DefaultBulletData();
            }
        }
    }

   

    public void SetInvisiblePowerup() {
        
        flt_CurrentTimeInvisible = 0;
        MakeAllEnemyInVisible();
       // body_Collider.enabled = false;
        obj_PowerUp.SetActive(true);
        IncreasePersantage();
    }

    public void IncreasePersantage() {
       

        float damage = playerData.flt_Damage + ((playerData.flt_Damage * flt_DamagePercentage)/100);
        float force = playerData.flt_Force + ((playerData.flt_Force * flt_ForcePercentage / 100));
        float firerate = playerData.flt_Firerate - ((playerData.flt_Firerate * flt_FireratePercentage / 100));

        Debug.Log("Damage" + damage);
        Debug.Log(flt_DamagePercentage);
        Debug.Log(playerData.flt_Damage +  " playerData.flt_Damage");

        if (damageIncreased.gameObject.activeSelf) {
            damage += damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
        }
        if (coolDown.gameObject.activeSelf) {
            firerate -= firerate * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
        }
       
        playerShooting.SetPowerupTime(damage, force, firerate);

       
    }
}
