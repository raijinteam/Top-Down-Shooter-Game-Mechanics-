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
    [SerializeField] private ExplodingData exploding_Data;
    [SerializeField] private float flt_CurrentTimeForSpawnBullet;


    [Header("Properites")]
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;
    [SerializeField] private float flt_AreaDamage;
    [SerializeField] private float flt_AreaRange;



    private void OnEnable() {
        
        
    }

    private void Update() {

        BulletHandler();
    }

    private void BulletHandler() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
       
        flt_CurrentTimeForSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeForSpawnBullet > flt_FireRate) {
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




        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);

        current.SetBulletData(direction, Damage, flt_force, flt_AreaDamage,
                                                    flt_AreaRange);
    }



    private void SetExplodingPowerUp() {
        flt_CurrentTimeForSpawnBullet = 0;
    }
}
