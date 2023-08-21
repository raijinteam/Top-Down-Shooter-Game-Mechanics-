using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClusterPowerUp : MonoBehaviour
{
    [Header("PowerUp Data")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeForPowerUp;
   

    [Header("ClusterBombData")]
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private ClusterBombMotion clusterBomb;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_BulletFireRate;
    [SerializeField] private int counter;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;

    [Header("ClusterBombHandler")]
    [SerializeField] private float flt_CurrentTimeForSpwnBomb;
    [SerializeField] private float flt_BombFireRate;

    private void OnEnable() {
        SetPowerUp();
        damageIncreased.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeForPowerUp);
    }
    private void OnDisable() {
        damageIncreased.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }

    private void SetCoolDown() {
        flt_BulletFireRate -= flt_BulletFireRate * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
        flt_BombFireRate -= flt_BombFireRate * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
    }

    private void SetDamage() {
        flt_Damage += flt_Damage * 0.01F * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void Update() {
        
        PowerUpHandler();
        ClusterBomeHandler();
    }

    private void ClusterBomeHandler() {
       
        flt_CurrentTimeForSpwnBomb += Time.deltaTime;

       
        if (flt_CurrentTimeForSpwnBomb > flt_BombFireRate) {
            flt_CurrentTimeForSpwnBomb = 0;
            SpawnClusterBomb();
        }
    }

    private void SpawnClusterBomb() {
        ClusterBombMotion currentCluster = Instantiate(clusterBomb, spawnPostion.position, transform.rotation);

        Vector3 randomDiection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
      
        currentCluster.SetBombData(randomDiection, flt_BulletFireRate, flt_Damage, flt_Force,
            counter);
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForPowerUp) {
            this.gameObject.SetActive(false);
            flt_CurrentTime = 0;
        }
    }

    private void SetPowerUp() {
     
        flt_CurrentTime = 0;
        flt_CurrentTimeForSpwnBomb = 0;
    }
}
