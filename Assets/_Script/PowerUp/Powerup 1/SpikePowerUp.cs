using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePowerUp : MonoBehaviour
{
    [SerializeField] private SpikeData spikeData;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_FireRate;

    [SerializeField] private float flt_Currentime;
    [SerializeField] private float flt_MaxPowerUpTime;
    [SerializeField] private float flt_CurrentPowerUptime;


    private void OnEnable() {
        SetSpikePowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxPowerUpTime);
    }
    private void Update() {
       
        SpikeHandler();
        PowerUpHandler();

    }

    private void PowerUpHandler() {
        
        flt_CurrentPowerUptime += Time.deltaTime;
        if (flt_CurrentPowerUptime > flt_MaxPowerUpTime) {
            this.gameObject.SetActive(false);
        }
    }

    private void SpikeHandler() {
       
        flt_Currentime += Time.deltaTime;
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
        if (flt_Currentime > CoolDown) {
            SpawnSpike();
            flt_Currentime = 0;
        }
    }

    private void SpawnSpike() {
        SpikeData currentSpike = Instantiate(spikeData, spawnPostion.position, spawnPostion.rotation);
        Debug.Log("Spawn Spike");
        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
        currentSpike.SetSpikeData(Damage, flt_Force);
    }

    private void SetSpikePowerUp() {
        
        flt_Currentime = 0;
        flt_CurrentPowerUptime = 0;
    }
} 
