using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalPowerUp : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_CurrentTimeForFireRate;
    [SerializeField] private float flt_MaxTimePowerUp;
    

    [SerializeField] private float flt_DelayBetweenTwoWave;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private TidalWave tidalWave;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_DestryTime;
    [SerializeField] private float flt_CoolDownTime;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;

    private void OnEnable() {
        SetTidalPowerUp();
        flt_CoolDownTime = flt_DelayBetweenTwoWave;
        damageIncreased.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimePowerUp);
    }
    private void OnDisable() {
        damageIncreased.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }


    private void SetCoolDown() {

        flt_CoolDownTime -= flt_CoolDownTime * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
    }

   

    private void SetDamage() {
        flt_Damage += flt_Damage * 0.01F * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void Update() {


        //TidalPowerUpHandler();
        // SpawnTidal();

        if (Input.GetKeyDown(KeyCode.C)) {
            SpawnTidalWave();
        }

    }

    private void SpawnTidal() {
        flt_CurrentTimeForFireRate += Time.deltaTime;

        if (flt_CurrentTimeForFireRate > flt_CoolDownTime) {

            SpawnTidalWave();
            flt_CurrentTimeForFireRate = 0;

        }
    }

    private void SpawnTidalWave() {

      
        TidalWave current =   Instantiate(tidalWave, spawnPostion.position, transform.rotation);
        current.setBulletData(spawnPostion.forward, 0,flt_Damage, flt_DestryTime);
       
    }

    private void TidalPowerUpHandler() {
        
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_MaxTimePowerUp) {
            this.gameObject.SetActive(false);
            
        }
    }

    private void SetTidalPowerUp() {
       
        flt_CurrentTime = 0;
    }
}
