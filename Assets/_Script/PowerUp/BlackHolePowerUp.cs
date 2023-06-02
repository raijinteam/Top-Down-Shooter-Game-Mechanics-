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
    private float flt_CurrentTime;
   
    [SerializeField] private Transform bulletSpawnPostion;
    private Transform target;
    

    [Header("vfx")]
    [SerializeField] private GameObject Obj_Muzzle;
    [SerializeField] private BlackHoleBulletMotion Obj_Bullet;

    private void OnEnable() {
        SetBulletBlackHole();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_FireRate * maxBullet);
    }
    public void SetBulletBlackHole() {
        bulletCounter = 0;
        flt_CurrentTime = 0;
        bulletCounter = 0;
        
    }

   

    private void Update() {

        PowerUpHandler();
       
    }

  
    private void PowerUpHandler() {
  
        flt_CurrentTime += Time.deltaTime;
      
        if (flt_CurrentTime > flt_FireRate) {
            
            flt_CurrentTime = 0;
            SpawnBlackHole();
            
        }
    }

    private void SpawnBlackHole() {

        Instantiate(Obj_Muzzle, bulletSpawnPostion.position, bulletSpawnPostion.rotation);
        BlackHoleBulletMotion current = Instantiate(Obj_Bullet, bulletSpawnPostion.position, 
                                            bulletSpawnPostion.rotation);
        current.setBulletData(bulletSpawnPostion.forward, this.flt_force, this.flt_Damage, this.
                                                            flt_BlackHoleExpltionTime) ;

        bulletCounter++;
        if (bulletCounter >= maxBullet) {

            this.gameObject.SetActive(false);
        }
    }

    
}
