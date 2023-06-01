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
    [SerializeField] private bool isPowerUpStart;
    [SerializeField] private float flt_MaxPowerUpTime;
    [SerializeField] private float flt_CurrentPowerUptime;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetSpikePowerUp();
        }
        SpikeHandler();
        PowerUpHandler();

    }

    private void PowerUpHandler() {
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentPowerUptime += Time.deltaTime;
        if (flt_CurrentPowerUptime > flt_MaxPowerUpTime) {
            isPowerUpStart = false;
        }
    }

    private void SpikeHandler() {
        if (!isPowerUpStart) {
            return;
        }
        flt_Currentime += Time.deltaTime;
        if (flt_Currentime > flt_FireRate) {
            SpawnSpike();
            flt_Currentime = 0;
        }
    }

    private void SpawnSpike() {
        SpikeData currentSpike = Instantiate(spikeData, spawnPostion.position, spawnPostion.rotation);
        Debug.Log("Spawn Spike");
        currentSpike.SetSpikeData(flt_Damage, flt_Force);
    }

    private void SetSpikePowerUp() {
        isPowerUpStart = true;
        flt_Currentime = 0;
        flt_CurrentPowerUptime = 0;
    }
} 
