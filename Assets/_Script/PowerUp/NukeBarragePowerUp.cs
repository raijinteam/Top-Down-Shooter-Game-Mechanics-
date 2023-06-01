using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NukeBarragePowerUp : MonoBehaviour
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
    [SerializeField] private GameObject Obj_Indicator;
    [SerializeField] private Transform transform_BulletIndeicater;
    [SerializeField] private NukeMissileMotion Obj_Bullet;

    public void SetNukePowerUp() {
        flt_CurrentTime = 0;
        bulletCounter = 0;
    }

    private void Start() {
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_Boundry_X = LevelManager.instance.flt_BoundryX;
        flt_Boundry_Z = LevelManager.instance.flt_BoundryZ;
    }

    private void Update() {
        PowerUpHandler();
    }

    private void PowerUpHandler() {
     
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            flt_CurrentTime = 0;
            bulletCounter++;
            if (bulletCounter > max_Bullet) {
              
            }
            else {
                SpawnBullet();
            }
            
        }
    }

    private void SpawnBullet() {

        Vector3 spawnPostion = new Vector3(Random.Range(flt_Boundry, flt_Boundry_X), flt_DownY_Postion,
                                            Random.Range(flt_Boundry, flt_Boundry_Z));
       GameObject current =  Instantiate(Obj_Indicator, spawnPostion,transform.rotation, transform_BulletIndeicater);
        StartCoroutine(SpawnNukeMissile(new Vector3(spawnPostion.x,20,spawnPostion.z),current));
    }

    private IEnumerator SpawnNukeMissile(Vector3 _spawnPostion , GameObject _Indicator) {

        yield return new WaitForSeconds(flt_DelayBullwetSpawn);
        NukeMissileMotion current = Instantiate(Obj_Bullet, _spawnPostion, Quaternion.identity);
        current.SetBulletData(flt_range, flt_force, flt_Damage, _Indicator);
    }
}
