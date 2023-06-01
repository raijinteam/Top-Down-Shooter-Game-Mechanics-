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
    [SerializeField] private bool isPowerUpStart;

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


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPowerUp();
        }
        PowerUpHandler();
        ClusterBomeHandler();
    }

    private void ClusterBomeHandler() {
        if (!isPowerUpStart) {
            return;
        }
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
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForPowerUp) {
            isPowerUpStart = false;
            flt_CurrentTime = 0;
        }
    }

    private void SetPowerUp() {
        isPowerUpStart = true;
        flt_CurrentTime = 0;
        flt_CurrentTimeForSpwnBomb = 0;
    }
}
