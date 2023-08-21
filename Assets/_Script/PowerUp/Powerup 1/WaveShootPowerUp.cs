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
    [SerializeField] private float flt_CoolDownTime;
    [SerializeField] private float flt_CurrentDamage;
    [SerializeField] private float flt_CurrentTimeForFireRate;
    private float flt_DealyOfTwoBullet = 0.2f;
    [SerializeField]private bool IsStartShootBullet;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;

    [Header("PowerUp")]
   
    [SerializeField]private float flt_CurrentTimeForThisPoawerUp;
    [SerializeField]private float flt_maxTimePowerUp;

    private void OnEnable() {
        SetPowerUpData();
        flt_CoolDownTime = flt_CurrentFirerate;
        coolDown.SetCoolDown += setCoolDown;
        damageIncreased.setDamageIncreased += SetDamage;
       UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_maxTimePowerUp);
    }
    private void OnDisable() {
        coolDown.SetCoolDown -= setCoolDown;
        damageIncreased.setDamageIncreased -= SetDamage;
    }

    private void SetDamage() {

        flt_CurrentDamage += flt_CurrentDamage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void setCoolDown() {
        flt_CoolDownTime -= flt_CoolDownTime * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
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
      
        flt_CurrentTimeForFireRate += Time.deltaTime;
        if (flt_CurrentTimeForFireRate > flt_CoolDownTime) {
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

        
            spawnedBullet.GetComponent<PlayerBulletMotion>().
                SetBulletData(spawnPosition_Bullet.forward, flt_CurrentDamage, flt_CurrentBulletForce,null,0,0,0);

           
            yield return new WaitForSeconds(flt_DealyOfTwoBullet);
        }


        IsStartShootBullet = false;
    }
}
