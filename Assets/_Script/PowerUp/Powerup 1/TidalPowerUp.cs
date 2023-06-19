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

    private void OnEnable() {
        SetTidalPowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimePowerUp);
    }

    private void Update() {
       

        TidalPowerUpHandler();
       
         SpawnTidal();

    }

    private void SpawnTidal() {
        flt_CurrentTimeForFireRate += Time.deltaTime;

        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_DelayBetweenTwoWave);
        if (flt_CurrentTimeForFireRate > CoolDown) {

            SpawnTidalWave();
            flt_CurrentTimeForFireRate = 0;

        }
    }

    private void SpawnTidalWave() {

        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(this.flt_Damage);
        TidalWave current =   Instantiate(tidalWave, spawnPostion.position, transform.rotation);
        current.setBulletData(spawnPostion.forward, 0,Damage, flt_DestryTime);
       
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
