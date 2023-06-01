using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitPowerUp : MonoBehaviour
{
    [Header("Bullet Data")]

    private bool isPowerUpStart;
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
    

    public void SetNukePowerUp() {
        isPowerUpStart = true;
        flt_CurrentTime = 0;
        bulletCounter = 0;
    }

    private void Start() {
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_Boundry_X = LevelManager.instance.flt_BoundryX;
        flt_Boundry_Z = LevelManager.instance.flt_BoundryZ;
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetNukePowerUp();
        }

        PowerUpHandler();
    }

    private void PowerUpHandler() {
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            flt_CurrentTime = 0;
            bulletCounter++;
            if (bulletCounter > max_Bullet) {
                isPowerUpStart = false;
            }
            else {
                SpawnBullet();
            }

        }
    }

    private void SpawnBullet() {

        Vector3 spawnPostion = new Vector3(Random.Range(flt_Boundry, flt_Boundry_X), flt_DownY_Postion,
                                            Random.Range(flt_Boundry, flt_Boundry_Z));
        OrbitBullet current = Instantiate(orbitBullet, spawnPostion, transform.rotation);
        current.SetBulletData(flt_range, flt_force, flt_Damage);
                           
       
    }

   
}
