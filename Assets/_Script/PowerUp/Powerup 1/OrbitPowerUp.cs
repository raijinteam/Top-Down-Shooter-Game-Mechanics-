using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPowerUp : MonoBehaviour
{
    [Header("Bullet Data")]

    [SerializeField] private float flt_range;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;
    [SerializeField] private int bulletCounter;
    [SerializeField] private int max_Bullet;
    [SerializeField] private float flt_FireRate;
    private float flt_CurrentTime;
    [SerializeField] private float flt_DelayBullwetSpawn;

    [Header("Script Data")]
    [SerializeField] private float flt_DownY_Postion;
    [SerializeField] private float flt_BoundryX_;
    [SerializeField] private float flt_Boundry_X;
    [SerializeField] private float flt_Boundry_Z;
    [SerializeField] private float flt_BoundryZ_;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;
 
    [Header("vfx")]
    [SerializeField] private OrbitBullet orbitBullet;


    private void OnEnable() {

        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_FireRate*max_Bullet);
        flt_CurrentTime = 0;
        bulletCounter = 0;
        flt_BoundryX_ = LevelManager.instance.flt_BoundryX_;
        flt_Boundry_X = LevelManager.instance.flt_BoundryX;
        flt_Boundry_Z = LevelManager.instance.flt_BoundryZ;
        flt_BoundryZ_ = LevelManager.instance.flt_BoundryZ_;
        damageIncreased.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;
    }

    private void OnDisable() {

        damageIncreased.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }

    private void SetCoolDown() {

        flt_FireRate -= 0.01f * flt_FireRate * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
    }

    private void SetDamage() {
        flt_Damage += flt_Damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void Update() {

      

        PowerUpHandler();
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        
        if (flt_CurrentTime > flt_FireRate) {
            flt_CurrentTime = 0;
            bulletCounter++;
            SpawnBullet();
          

        }
    }

    private void SpawnBullet() {

        Vector3 spawnPostion = new Vector3(Random.Range(flt_BoundryX_, flt_Boundry_X), flt_DownY_Postion,
                                            Random.Range(flt_BoundryZ_, flt_Boundry_Z));
        OrbitBullet current = Instantiate(orbitBullet, spawnPostion, transform.rotation);
      
        current.SetBulletData(flt_range, flt_force, flt_Damage);

        bulletCounter++;
        if (bulletCounter >=  max_Bullet) {
            this.gameObject.SetActive(false);
        }
                           
       
    }

   
}
