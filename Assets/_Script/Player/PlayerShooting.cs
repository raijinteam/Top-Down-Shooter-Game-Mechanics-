using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShooting : MonoBehaviour
{
    [Header("Componant")]
    [SerializeField] private PlayerData playerData;


    [Header("Bullet Instantiate")]
    [SerializeField] private GameObject obj_Missile;
    [SerializeField] private Transform[] all_SpawnPostion;
    [SerializeField] private GameObject obj_Bullet;
    [SerializeField] private Transform spawnPosition_Bullet;
    private GameObject target;
    private float MinDistnce = 0;


    [Header("RunTimeValue")]
    
    [SerializeField] private float flt_CurrentBulletForce;
    [SerializeField] private float flt_CurrentFirerate;
    [SerializeField] private float flt_CurrentDamage;
    
    [SerializeField] private float flt_CurrentTimeForFireRate;

    [Header("VFX")]
    [SerializeField] private ParticleSystem bulletMuzzle;
    
    [Header("Enemy Handling")]
    public bool isEnemyAcive;
    [SerializeField] private float targetAngle;
    [SerializeField] private float currentAngle;
    [SerializeField] private float flt_SpeedOfTarget = 250;


   

    private void Start() {
        DefaultBulletData();
    }


    private void Update() {

        FireBullet();
    }

    public void SetPowerupTime(float flt_Bulletdamage, float flt_BulletForce, float flt_BulletFirerate) {
        flt_CurrentDamage = flt_Bulletdamage;
        flt_CurrentBulletForce = flt_BulletForce;
        flt_CurrentFirerate = flt_BulletFirerate;
    }
    public void DefaultBulletData() {
        flt_CurrentFirerate = playerData.flt_Firerate;
        flt_CurrentDamage = playerData.flt_Damage;
        flt_CurrentBulletForce = playerData.flt_Force;
    }

    private void FireBullet() {
        if (LevelManager.instance.list_AllEnemyInActiveInLevel.Count == 0) {
            isEnemyAcive = false;
            return;
        }

        isEnemyAcive = true;

        FindTarget();

        flt_CurrentTimeForFireRate += Time.deltaTime;
        if (flt_CurrentTimeForFireRate > flt_CurrentFirerate) {
            flt_CurrentTimeForFireRate = 0;
            SpawnBullet();
        }
    }

    private void FindTarget() {

      
        
       
      
        for (int i = 0; i < LevelManager.instance.list_AllEnemyInActiveInLevel.Count; i++) {

            if (LevelManager.instance.list_AllEnemyInActiveInLevel[i] == null) {
                continue;
            }


            float distance = MathF.Abs(Vector3.Distance(transform.position,
                LevelManager.instance.list_AllEnemyInActiveInLevel[i].transform.position));
            if (target == null) {
                target = LevelManager.instance.list_AllEnemyInActiveInLevel[i];
                MinDistnce = distance;
            }
            else {
                if (distance < MinDistnce) {
                    target = LevelManager.instance.list_AllEnemyInActiveInLevel[i];
                    MinDistnce = distance;
                }
            }
        }

        if (target == null) {
            return;
        }
       
        Vector3 dirction = (target.transform.position - transform.position).normalized;
         targetAngle = Mathf.Atan2(dirction.x,dirction.z) * Mathf.Rad2Deg;

        Quaternion Qua_Target = Quaternion.Euler(0, targetAngle, 0);
        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, Qua_Target, flt_SpeedOfTarget * Time.deltaTime);
     
        spawnPosition_Bullet.LookAt(target.transform);
        

       

    }

    private void SpawnBullet() {

        GameObject spawnedBullet = Instantiate(obj_Bullet, spawnPosition_Bullet.position, spawnPosition_Bullet.rotation);


        spawnedBullet.GetComponent<PlayerBulletMotion>().
            SetBulletData(spawnPosition_Bullet.forward, flt_CurrentDamage, flt_CurrentBulletForce,target.transform);
    
        
        bulletMuzzle.Play();

        if (GameManager.instance.isMissilePowerUpActive) {
            SpwnMisssle();
        }
    }

    private void SpwnMisssle() {
        int Index = Random.Range(0, 100);
        if (Index < playerData.PersantageOfSpawn) {

            for (int i = 0; i < playerData.MissileCounter; i++) {

            

                GameObject spawnedBullet = Instantiate(obj_Missile, all_SpawnPostion[i].position,
                            spawnPosition_Bullet.rotation);


                spawnedBullet.GetComponent<MicroMissileMotion>().
                    SetBulletData(spawnPosition_Bullet.forward, playerData.flt_MisisleDamage);
            }
            
                              
        }
    }
}
