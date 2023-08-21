using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolePowerUp : MonoBehaviour
{
    [Header("Bullet Data")]
    [SerializeField] private float flt_BlackHoleExpltionTime;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;
    [SerializeField] private int bulletCounter;
    [SerializeField] private int maxBullet;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentCoolDown;
    private float flt_CurrentTime;
   
    [SerializeField] private Transform bulletSpawnPostion;
    private Transform target;
    [SerializeField] private DamageIncreasedPowerUp damageIncresed;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;
    

    [Header("vfx")]
    [SerializeField] private GameObject Obj_Muzzle;
    [SerializeField] private BlackHoleBulletMotion Obj_Bullet;

    private void OnEnable() {
        SetBulletBlackHole();
        flt_CurrentCoolDown = flt_FireRate;
        damageIncresed.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_FireRate * maxBullet);
       
    }

    private void OnDisable() {

        damageIncresed.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }

    private void SetCoolDown() {

        flt_CurrentCoolDown -= flt_CurrentCoolDown * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
    }

    private void SetDamage() {
        flt_Damage += flt_Damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    public void SetBulletBlackHole() {
        bulletCounter = 0;
        flt_CurrentTime = 0;
        bulletCounter = 0;
        
    }

   

    private void Update() {

        // PowerUpHandler();

        if (Input.GetKeyDown(KeyCode.X)) {
            SpawnBlackHole();
        }
    }

  
    private void PowerUpHandler() {
  
        flt_CurrentTime += Time.deltaTime;
       
        if (flt_CurrentTime > flt_CurrentCoolDown) {
            
            flt_CurrentTime = 0;
            SpawnBlackHole();
            
        }
    }

    private void SpawnBlackHole() {

        Instantiate(Obj_Muzzle, bulletSpawnPostion.position, bulletSpawnPostion.rotation);
        BlackHoleBulletMotion current = Instantiate(Obj_Bullet, bulletSpawnPostion.position, 
                                            bulletSpawnPostion.rotation);
       
        current.setBulletData(bulletSpawnPostion.forward, this.flt_force, flt_Damage, this.
                                                            flt_BlackHoleExpltionTime) ;

        bulletCounter++;
        if (bulletCounter >= maxBullet) {

            this.gameObject.SetActive(false);
        }
    }

    
}
