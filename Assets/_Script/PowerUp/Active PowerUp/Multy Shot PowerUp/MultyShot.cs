using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyShot : MonoBehaviour
{


    [Header("Bullet Data")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private MultyShotData multyShotData;
   

    [Header("Bullet")]
    [SerializeField] private MultyBulletMotion Obj_Bullet;
    [SerializeField] private Transform[] spawnPostion;
    [SerializeField] private ParticleSystem[] all_Muzzle;
   

    private void Update() {

       
        BulletHandler();
       

    }

    private void SetMolotovPowerUp() {
       
        flt_CurrentTime = 0;
       
    }

  
    private void BulletHandler() {
        
        flt_CurrentTime += Time.deltaTime;
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(multyShotData.FireRate);
        if (flt_CurrentTime > CoolDown) {
            flt_CurrentTime = 0;
            SpawnBullet();

        }
    }

    private void SpawnBullet() {

        for (int i = 0; i < multyShotData.Max_Counter; i++) {
            MultyBulletMotion current = Instantiate(Obj_Bullet, spawnPostion[i].position, spawnPostion[i].rotation);

            all_Muzzle[i].Play();

            float Damage = PlayerManager.instance.Player.GetIncreasedDamage(multyShotData.Damage);
            current.GetComponent<MultyBulletMotion>().SetBulletData(spawnPostion[i].forward, Damage,
                            multyShotData.Force);
        }
       
    }
}
