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

    [Header("ClusterBombHandler")]
    [SerializeField] private float flt_CurrentTimeForSpwnBomb;
    [SerializeField] private float flt_BombFireRate;

    private void OnEnable() {
        SetPowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeForPowerUp);
    }
    private void Update() {
        
        PowerUpHandler();
        ClusterBomeHandler();
    }

    private void ClusterBomeHandler() {
       
        flt_CurrentTimeForSpwnBomb += Time.deltaTime;

        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_BombFireRate);
        if (flt_CurrentTimeForSpwnBomb > CoolDown) {
            flt_CurrentTimeForSpwnBomb = 0;
            SpawnClusterBomb();
        }
    }

    private void SpawnClusterBomb() {
        ClusterBombMotion currentCluster = Instantiate(clusterBomb, spawnPostion.position, transform.rotation);

        Vector3 randomDiection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
        currentCluster.SetBombData(randomDiection, flt_BulletFireRate, Damage, flt_Force,
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
