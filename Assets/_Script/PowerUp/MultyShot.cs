using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyShot : MonoBehaviour
{
    [Header("PowerUpData")]
    [SerializeField] private bool isPowerUpStart;

    [Header("Bullet Data")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private int Counter = 2;
   

    [Header("Bullet")]
    [SerializeField] private MultyBulletMotion Obj_Bullet;
    [SerializeField] private Transform[] spawnPostion;
    [SerializeField] private ParticleSystem[] all_Muzzle;
   

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetMolotovPowerUp();
        }
        BulletHandler();
       

    }

    private void SetMolotovPowerUp() {
        isPowerUpStart = true;
        flt_CurrentTime = 0;
       
    }

  
    private void BulletHandler() {
        if (!isPowerUpStart) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            flt_CurrentTime = 0;
            SpawnBullet();

        }
    }

    private void SpawnBullet() {

        for (int i = 0; i < Counter; i++) {
            MultyBulletMotion current = Instantiate(Obj_Bullet, spawnPostion[i].position, spawnPostion[i].rotation);

            all_Muzzle[i].Play();

            current.GetComponent<MultyBulletMotion>().SetBulletData(spawnPostion[i].forward, flt_Damage,
                            flt_Force);
        }
       
    }
}
