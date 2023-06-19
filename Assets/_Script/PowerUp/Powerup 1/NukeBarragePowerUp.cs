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
    private bool isspawn;

    [Header("Script Data")]
    [SerializeField] private float flt_DownY_Postion;
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_Boundry_X;
    [SerializeField] private float flt_Boundry_Z;

    [Header("vfx")]
    [SerializeField] private GameObject Obj_Indicator;
    [SerializeField] private Transform transform_BulletIndeicater;
    [SerializeField] private NukeMissileMotion Obj_Bullet;

    GameObject current_Indicator;
    NukeMissileMotion current_Bullet;
    Coroutine my_Coroutine;

    public void SetNukePowerUp() {
        flt_CurrentTime = 0;
        bulletCounter = 0;
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
    }

    private void OnEnable() {
        SetNukePowerUp();
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_Boundry_X = LevelManager.instance.flt_BoundryX;
        flt_Boundry_Z = LevelManager.instance.flt_BoundryZ;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_FireRate*max_Bullet*flt_DelayBullwetSpawn);
    }

    

    private void Update() {
        PowerUpHandler();
    }

    private void PowerUpHandler() {
        if (isspawn ) {
            return;
        }
     
        flt_CurrentTime += Time.deltaTime;
        float cooldown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
        if (flt_CurrentTime > cooldown) {
           
            isspawn = true;
            SpawnBullet();



        }
    }

    private void SpawnBullet() {

        Vector3 spawnPostion = new Vector3(Random.Range(flt_Boundry, flt_Boundry_X), flt_DownY_Postion,
                                            Random.Range(flt_Boundry, flt_Boundry_Z));
        current_Indicator =  Instantiate(Obj_Indicator, spawnPostion,transform.rotation, transform_BulletIndeicater);
      my_Coroutine = StartCoroutine(SpawnNukeMissile(new Vector3(spawnPostion.x,20,spawnPostion.z),
            current_Indicator));
    }

    private IEnumerator SpawnNukeMissile(Vector3 _spawnPostion , GameObject _Indicator) {

        yield return new WaitForSeconds(flt_DelayBullwetSpawn);
         current_Bullet = Instantiate(Obj_Bullet, _spawnPostion, Quaternion.identity);

        float damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
        current_Bullet.SetBulletData(flt_range, flt_force, damage, _Indicator);
        flt_CurrentTime = 0;
        isspawn = false;
        bulletCounter++;
        if (bulletCounter >= max_Bullet) {
            this.gameObject.SetActive(false);
        }
    }
}
