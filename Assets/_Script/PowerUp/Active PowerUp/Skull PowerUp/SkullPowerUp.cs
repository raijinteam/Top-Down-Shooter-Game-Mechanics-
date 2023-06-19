using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkullPowerUp : MonoBehaviour
{



    [Header("BulletData")]
    [SerializeField] private SkullData skullData;
    [SerializeField] private float flt_CurrentTime;

    [Header("Bullet")]
    [SerializeField] private SkullMissileMotion Obj_Skull;
    [SerializeField] private Transform spawnPostion;
    
    

    private void Update() {

       
        BulletHandler();
       
    }

    public void SetSkullPowerUp() {
        
        flt_CurrentTime = 0;
       
    }

   

    private void BulletHandler() {

        
        flt_CurrentTime += Time.deltaTime;
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(skullData.Firerate);
        if (flt_CurrentTime > CoolDown) {
            flt_CurrentTime = 0;

            SpawnBullet();
           

        }
    }

    private void SpawnBullet() {

        
        if (GameManager.instance.list_ActiveEnemies.Count==0) {
            return;
        }

        SkullMissileMotion current = Instantiate(Obj_Skull, spawnPostion.position, spawnPostion.rotation);

        int Index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);
        Transform Target = GameManager.instance.list_ActiveEnemies[Index];
        spawnPostion.LookAt(Target);
        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(skullData.Damage);
        current.SetBulletData(spawnPostion.forward, Damage);
    }
}
