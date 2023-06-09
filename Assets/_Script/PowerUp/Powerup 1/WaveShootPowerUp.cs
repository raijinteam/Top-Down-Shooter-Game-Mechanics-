using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class WaveShootPowerUp : MonoBehaviour
{
   

    [Header("Bullet Instantiate")]
    [SerializeField] private GameObject obj_Bullet; // give script reference
    [SerializeField] private Transform spawnPosition_Bullet;

    [Header("RunTimeValue")]
    [SerializeField]private int NoOfWave = 5 ;
    [SerializeField] private float flt_CurrentBulletForce;
    [SerializeField] private float flt_CurrentFirerate;
    [SerializeField] private float flt_CurrentDamage;
    [SerializeField] private float flt_CurrentTimeForFireRate;
    private float flt_DealyOfTwoBullet = 0.2f;
    [SerializeField]private bool IsStartShootBullet;

    [Header("PowerUp")]
   
    [SerializeField]private float flt_CurrentTimeForThisPoawerUp;
    [SerializeField]private float flt_maxTimePowerUp;

    private void OnEnable() {
        SetPowerUpData();
       UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_maxTimePowerUp);
    }
    private void Update() {
       

        PowerUpHandler();
        FireBullet();
        
       
    }

    private void PowerUpHandler() {
      
        flt_CurrentTimeForThisPoawerUp += Time.deltaTime;
        if (flt_CurrentTimeForThisPoawerUp > flt_maxTimePowerUp) {
            this.gameObject.SetActive(false);
        }
    }

    public void SetPowerUpData() {

        IsStartShootBullet = false;
        flt_CurrentTimeForThisPoawerUp = 0;
        flt_CurrentTimeForFireRate = 0;
    }

    private void FireBullet() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
        if (IsStartShootBullet) {
            return;
        }
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_CurrentFirerate);
        flt_CurrentTimeForFireRate += Time.deltaTime;
        if (flt_CurrentTimeForFireRate > CoolDown) {
            flt_CurrentTimeForFireRate = 0;
            IsStartShootBullet = true;
            StartCoroutine(SpawnBullet());
        }
    }

    private void FindTarget() {

        int Index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);

        spawnPosition_Bullet.LookAt(GameManager.instance.list_ActiveEnemies[Index]);

    }

   

    private IEnumerator SpawnBullet() {

        for (int i = 0; i < NoOfWave; i++) {
            FindTarget();
            GameObject spawnedBullet = Instantiate(obj_Bullet, spawnPosition_Bullet.position, spawnPosition_Bullet.rotation);

            float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_CurrentDamage);
            spawnedBullet.GetComponent<PlayerBulletMotion>().
                SetBulletData(spawnPosition_Bullet.forward, Damage, flt_CurrentBulletForce,null,0,0,0);

           
            yield return new WaitForSeconds(flt_DealyOfTwoBullet);
        }


        IsStartShootBullet = false;
    }
}
