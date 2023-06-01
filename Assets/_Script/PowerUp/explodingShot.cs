using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class explodingShot : MonoBehaviour
{
    [Header("PowerUp - Cantroller")]
    [SerializeField] private bool isPowerUpStart;
   
  
  

    [Header("Bullet - Data")]
    [SerializeField] private Transform bulletSpawnpostion;
    [SerializeField] private ExplodingBulletMotion obj_Bullet;
    [SerializeField] private float flt_BulletFireRate;
    [SerializeField] private float flt_CurrentTimeForSpawnBullet;
    [SerializeField] private float flt_BulletDamage;
    [SerializeField] private float flt_BulletForce;
    [SerializeField] private float flt_BulletAreaDamage;
    [SerializeField] private float flt_Area_Range;

   
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetExplodingPowerUp();
        }

       
        BulletHandler();
    }

    private void BulletHandler() {
        if (!isPowerUpStart && LevelManager.instance.list_AllEnemyInActiveInLevel.Count == 0) {
            return;
        }
        flt_CurrentTimeForSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeForSpawnBullet > flt_BulletFireRate) {
            flt_CurrentTimeForSpawnBullet = 0;
            SpawnBullet();
        }
    }

    private void SpawnBullet() {

        if (LevelManager.instance.list_AllEnemyInActiveInLevel.Count == 0) {
            return;
        }

        int Index = Random.Range(0, LevelManager.instance.list_AllEnemyInActiveInLevel.Count);
        
        Transform target = LevelManager.instance.list_AllEnemyInActiveInLevel[Index].transform;
        bulletSpawnpostion.LookAt(target);

        ExplodingBulletMotion current = Instantiate(obj_Bullet, bulletSpawnpostion.position,
                            transform.rotation);

        Vector3 direction = bulletSpawnpostion.forward;
        current.SetBulletData(direction, flt_BulletDamage, flt_BulletForce, flt_BulletAreaDamage,
                                                    flt_Area_Range);    
    }

   

    private void SetExplodingPowerUp() {
        isPowerUpStart = true;
        flt_CurrentTimeForSpawnBullet = 0;
    }
}
