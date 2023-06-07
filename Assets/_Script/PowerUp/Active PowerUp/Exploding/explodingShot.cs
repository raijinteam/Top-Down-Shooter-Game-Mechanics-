using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class explodingShot : MonoBehaviour
{
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
             
        BulletHandler();
    }

    private void BulletHandler() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
        flt_CurrentTimeForSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeForSpawnBullet > flt_BulletFireRate) {
            flt_CurrentTimeForSpawnBullet = 0;
            SpawnBullet();
        }
    }

    private void SpawnBullet() {

        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }

        int Index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);
        
        Transform target = GameManager.instance.list_ActiveEnemies[Index].transform;
        bulletSpawnpostion.LookAt(target);

        ExplodingBulletMotion current = Instantiate(obj_Bullet, bulletSpawnpostion.position,
                            transform.rotation);

        Vector3 direction = bulletSpawnpostion.forward;
        current.SetBulletData(direction, flt_BulletDamage, flt_BulletForce, flt_BulletAreaDamage,
                                                    flt_Area_Range);    
    }

   

    private void SetExplodingPowerUp() {
        flt_CurrentTimeForSpawnBullet = 0;
    }
}
