using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrorShort : MonoBehaviour
{
    [SerializeField] private float flt_Currenttime;
    [SerializeField] private float flt_MaxPowerUpTime;
    

    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;

    [Header("Bullet")]
    [SerializeField] private TerrorShotBullet terroShotBullet;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private float flt_CurrentTimeSpawnBullet;
    [SerializeField] private float flt_FireRate;


    private void OnEnable() {
        SetTerrorShot();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxPowerUpTime);
    }
    private void Update() {
      
        PowerUpHandler();
        BulletHandler();
    }

    private void BulletHandler() {
      
        flt_CurrentTimeSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeSpawnBullet > flt_FireRate) {
            SpawnBullet();
            flt_CurrentTimeSpawnBullet = 0;
        }

    }

    private void SpawnBullet() {

        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }

        int index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);
        spawnPostion.LookAt(GameManager.instance.list_ActiveEnemies[index]);
        TerrorShotBullet currentBullet = Instantiate(terroShotBullet,
                                     spawnPostion.position, spawnPostion.rotation);
        currentBullet.SetBulletData(spawnPostion.forward,flt_Damage, flt_force);
    }

    private void PowerUpHandler() {
       

        flt_Currenttime += Time.deltaTime;
        if (flt_Currenttime > flt_MaxPowerUpTime) {
            this.gameObject.SetActive(false);
        }
    }

    private void SetTerrorShot() {
       
        flt_Currenttime = 0;
        flt_CurrentTimeSpawnBullet = 0;
    }
}
