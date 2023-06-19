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
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
        if (flt_CurrentTimeSpawnBullet > CoolDown) {
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
        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
        TerrorShotBullet currentBullet = Instantiate(terroShotBullet,
                                     spawnPostion.position, spawnPostion.rotation);
        currentBullet.SetBulletData(spawnPostion.forward,Damage, flt_force);
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
