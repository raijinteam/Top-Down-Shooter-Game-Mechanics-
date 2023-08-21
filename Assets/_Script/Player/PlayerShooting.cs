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
   
    [SerializeField] private PlayerBulletMotion obj_Bullet;
    [SerializeField] private Transform spawnPosition_Bullet;
    [Header("MicroMissile")]
    [SerializeField] private MicroMissileMotion obj_MicroMissile;
    [SerializeField] private Transform[] all_SpawnPostion;
    [Header("MultyShot")]
    [SerializeField] private MultyBulletMotion obj_MultyShot;
    [SerializeField] private Transform[] all_MultySpawnPostion;
    [SerializeField] private ParticleSystem[] all_MultyShotVFX;
    public GameObject target;
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
    [SerializeField] private FireRateDecreasedPowerUp fireRateDecreased;
    [SerializeField] private bool isRagePowerUpStart;

    private void OnEnable() {
        DefaultBulletData();
        fireRateDecreased.setFireRate += SetFireRate;
    }

    private void SetFireRate() {
        playerData.flt_Firerate -= playerData.flt_Firerate * 0.01f * PowerUpData.insatnce.fireRateDecreased.GetCurrentFirerate;
        if (!isRagePowerUpStart) {
            flt_CurrentFirerate = playerData.flt_Firerate;
        }
    }

    private void Start() {
        
    }


    private void Update() {

        FireBullet();
    }

    public void SetPowerupTime(float flt_Bulletdamage, float flt_BulletForce, float flt_BulletFirerate) {
        isRagePowerUpStart = true;
        flt_CurrentDamage = flt_Bulletdamage;
        flt_CurrentBulletForce = flt_BulletForce;
        flt_CurrentFirerate = flt_BulletFirerate;
    }
    public void DefaultBulletData() {
        isRagePowerUpStart = false;
        flt_CurrentFirerate = playerData.flt_Firerate;
        flt_CurrentDamage = playerData.flt_Damage;
        flt_CurrentBulletForce = playerData.flt_Force;
    }

    private void FireBullet() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            isEnemyAcive = false;
            return;
        }

        isEnemyAcive = true;

        FindTarget();

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SpawnBullet();
        }

        //flt_CurrentTimeForFireRate += Time.deltaTime;
        //if (flt_CurrentTimeForFireRate > flt_CurrentFirerate) {
        //    flt_CurrentTimeForFireRate = 0;

        //    SpawnBullet();
        //}
    }

    private void FindTarget() {
        MinDistnce = 0;

        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            if (GameManager.instance.list_ActiveEnemies[i] == null) {
                continue;
            }

           
            float distance = MathF.Abs(Vector3.Distance(transform.position,
                GameManager.instance.list_ActiveEnemies[i].transform.position));
            if (MinDistnce == 0) {
                target = GameManager.instance.list_ActiveEnemies[i].gameObject;
                MinDistnce = distance;
            }
            else {
                if (distance < MinDistnce) {
                    target = GameManager.instance.list_ActiveEnemies[i].gameObject;
                    MinDistnce = distance;
                }
            }
        }

       
        Vector3 dirction = (target.transform.position - transform.position).normalized;
         targetAngle = Mathf.Atan2(dirction.x,dirction.z) * Mathf.Rad2Deg;

        Quaternion Qua_Target = Quaternion.Euler(0, targetAngle, 0);
        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, Qua_Target, flt_SpeedOfTarget * Time.deltaTime);
     
        spawnPosition_Bullet.LookAt(target.transform);
        

       

    }

    private void SpawnBullet() {

        PlayerBulletMotion spawnedBullet = Instantiate(obj_Bullet, spawnPosition_Bullet.position, spawnPosition_Bullet.rotation);


       
        spawnedBullet.SetBulletData(spawnPosition_Bullet.forward, playerData.flt_Damage, flt_CurrentBulletForce, target.transform

            , playerData.RechoestCounter,playerData.Rechoest_damagePersantage,playerData.persantageOfDelathBow);
    
        
        bulletMuzzle.Play();

        if (GameManager.instance.IsMicroMissile) {
            SpwnMisssle();
        }
        if (GameManager.instance.IsMultyShot) {
            SpawnMultyShot();
        }
    }

    private void SpawnMultyShot() {
        for (int i = 0; i < all_MultySpawnPostion.Length; i++) {

            MultyBulletMotion currentMulty = Instantiate(obj_MultyShot, all_MultySpawnPostion[i].position, all_MultySpawnPostion[i].rotation);
            currentMulty.SetBulletData(all_MultySpawnPostion[i].forward, playerData.multyShotDamage, playerData.flt_Force);

            all_MultyShotVFX[i].Play();
        }
    }

    private void SpwnMisssle() {

        for (int i = 0; i < playerData.MissileCounter; i++) {



            MicroMissileMotion spawnedBullet = Instantiate(obj_MicroMissile, all_SpawnPostion[i].position,
                        spawnPosition_Bullet.rotation);



            spawnedBullet.SetBulletData(spawnPosition_Bullet.forward, playerData.flt_MissileDamage);

        }
        
    }
}
