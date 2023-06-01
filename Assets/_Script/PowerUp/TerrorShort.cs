using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrorShort : MonoBehaviour
{
    [SerializeField] private float flt_Currenttime;
    [SerializeField] private float flt_MaxPowerUpTime;
    [SerializeField] private bool isPowerUpStart;

    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;

    [Header("Bullet")]
    [SerializeField] private TerrorShotBullet terroShotBullet;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private float flt_CurrentTimeSpawnBullet;
    [SerializeField] private float flt_FireRate;
     

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetTerrorShot();
        }
        PowerUpHandler();
        BulletHandler();
    }

    private void BulletHandler() {
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentTimeSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeSpawnBullet > flt_FireRate) {
            SpawnBullet();
            flt_CurrentTimeSpawnBullet = 0;
        }

    }

    private void SpawnBullet() {

        if (LevelManager.instance.list_AllEnemyInActiveInLevel.Count == 0) {
            return;
        }

        int index = Random.Range(0, LevelManager.instance.list_AllEnemyInActiveInLevel.Count);
        spawnPostion.LookAt(LevelManager.instance.list_AllEnemyInActiveInLevel[index].transform);
        TerrorShotBullet currentBullet = Instantiate(terroShotBullet,
                                     spawnPostion.position, spawnPostion.rotation);
        currentBullet.SetBulletData(spawnPostion.forward,flt_Damage, flt_force);
    }

    private void PowerUpHandler() {
        if (!isPowerUpStart) {
            return;
        }

        flt_Currenttime += Time.deltaTime;
        if (flt_Currenttime > flt_MaxPowerUpTime) {
            isPowerUpStart = false;
        }
    }

    private void SetTerrorShot() {
        isPowerUpStart = true;
        flt_Currenttime = 0;
        flt_CurrentTimeSpawnBullet = 0;
    }
}
