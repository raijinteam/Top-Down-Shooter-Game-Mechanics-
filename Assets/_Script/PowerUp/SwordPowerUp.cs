using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUp : MonoBehaviour
{
    [Header("PowerUp - Cantroller")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeToThisPowerUp;
   
    [Header("Sword - Data")]
    [SerializeField] private SwordMovement swordMovement;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    [Header("Sword - Handler")]
    [SerializeField] private float flt_SwordFireRate;
    [SerializeField] private float flt_CurrentSpawnSwordTime;

    private void OnEnable() {
        SetSwordPowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeToThisPowerUp);
    }
    private void Update() {
        
        PowerUpHandler();
        SpawnHandler();
    }

    private void SpawnHandler() {
      

        flt_CurrentSpawnSwordTime += Time.deltaTime;
        if (flt_CurrentSpawnSwordTime > flt_SwordFireRate) {
            SpawnSoword();
            flt_CurrentSpawnSwordTime = 0;
        }
    }

    private void SpawnSoword() {
      SwordMovement current =   Instantiate(swordMovement, transform.position, transform.rotation);
        current.SetSwordData(flt_Force, flt_Damage);
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeToThisPowerUp) {
            this.gameObject.SetActive(false);
            flt_CurrentTime = 0;
            flt_CurrentSpawnSwordTime = 0;
        }
    }

    private void SetSwordPowerUp() {
       
        flt_CurrentSpawnSwordTime = 0;
        flt_CurrentTime = 0;
    }
}
