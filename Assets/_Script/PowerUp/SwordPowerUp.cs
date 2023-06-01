using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUp : MonoBehaviour
{
    [Header("PowerUp - Cantroller")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeToThisPowerUp;
    [SerializeField] private bool isPowerUpStart;

    [Header("Sword - Data")]
    [SerializeField] private SwordMovement swordMovement;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    [Header("Sword - Handler")]
    [SerializeField] private float flt_SwordFireRate;
    [SerializeField] private float flt_CurrentSpawnSwordTime;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            SetSwordPowerUp();
        }
        PowerUpHandler();
        SpawnHandler();
    }

    private void SpawnHandler() {
        if (!isPowerUpStart) {
            return;
        }

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
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeToThisPowerUp) {
            isPowerUpStart = false;
            flt_CurrentTime = 0;
            flt_CurrentSpawnSwordTime = 0;
        }
    }

    private void SetSwordPowerUp() {
        isPowerUpStart = true;
        flt_CurrentSpawnSwordTime = 0;
        flt_CurrentTime = 0;
    }
}
