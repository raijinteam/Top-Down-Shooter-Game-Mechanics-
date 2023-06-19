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
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_Boundry_X;
    [SerializeField] private float flt_Boundry_Z;

    [Header("vfx")]
    [SerializeField] private OrbitBullet orbitBullet;


    private void OnEnable() {

        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_FireRate*max_Bullet);
        flt_CurrentTime = 0;
        bulletCounter = 0;
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_Boundry_X = LevelManager.instance.flt_BoundryX;
        flt_Boundry_Z = LevelManager.instance.flt_BoundryZ;
    }
    

   
    private void Update() {

      

        PowerUpHandler();
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
        if (flt_CurrentTime > CoolDown) {
            flt_CurrentTime = 0;
            bulletCounter++;
            SpawnBullet();
          

        }
    }

    private void SpawnBullet() {

        Vector3 spawnPostion = new Vector3(Random.Range(flt_Boundry, flt_Boundry_X), flt_DownY_Postion,
                                            Random.Range(flt_Boundry, flt_Boundry_Z));
        OrbitBullet current = Instantiate(orbitBullet, spawnPostion, transform.rotation);
        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
        current.SetBulletData(flt_range, flt_force, Damage);

        bulletCounter++;
        if (bulletCounter >=  max_Bullet) {
            this.gameObject.SetActive(false);
        }
                           
       
    }

   
}
