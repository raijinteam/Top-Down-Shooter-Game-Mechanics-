using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkullPowerUp : MonoBehaviour
{
   
   
    [SerializeField] private bool isPowerUpStart;
    [Header("BulletData")]
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTime;

    [Header("Bullet")]
    [SerializeField] private SkullMissileMotion Obj_Skull;
    [SerializeField] private Transform spawnPostion;
    
    

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetSkullPowerUp();
        }
        BulletHandler();
       
    }

    public void SetSkullPowerUp() {
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

        
        if (LevelManager.instance.list_AllEnemyInActiveInLevel.Count==0) {
            return;
        }

        SkullMissileMotion current = Instantiate(Obj_Skull, spawnPostion.position, spawnPostion.rotation);

        int Index = Random.Range(0, LevelManager.instance.list_AllEnemyInActiveInLevel.Count);
        Transform Target = LevelManager.instance.list_AllEnemyInActiveInLevel[Index].transform;
        spawnPostion.LookAt(Target);
        current.SetBulletData(spawnPostion.forward, flt_Damage);
    }
}
