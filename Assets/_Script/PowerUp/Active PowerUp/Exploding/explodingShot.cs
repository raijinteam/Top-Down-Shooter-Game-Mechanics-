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



    private void Update() {

        BulletHandler();
    }

    private void BulletHandler() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(exploding_Data.FireRate);
        flt_CurrentTimeForSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeForSpawnBullet > CoolDown) {
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




        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(exploding_Data.Damage);

        current.SetBulletData(direction, Damage, exploding_Data.Force, exploding_Data.AreaDamage,
                                                    exploding_Data.AreaRange);
    }



    private void SetExplodingPowerUp() {
        flt_CurrentTimeForSpawnBullet = 0;
    }
}
