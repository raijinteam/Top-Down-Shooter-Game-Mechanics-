using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AstrlArrowPowerUp : MonoBehaviour {
   

    private float flt_BoundryX_;
    private float flt_BoundrX;
    private float flt_BoundryZ;
    private float flt_BoundryZ_;


    [SerializeField] private int No_OfArrow_Spawn;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_FireRate;

    [SerializeField] private float flt_CurrentTimeForFireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Maxtime;
   
    [SerializeField] private Arrow arrow;
    [SerializeField] private DamageIncreasedPowerUp DamageIncresed;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;


    private void OnEnable() {
        SpawnArrow();
        flt_BoundryX_ = LevelManager.instance.flt_BoundryX_;
        flt_BoundrX = LevelManager.instance.flt_BoundryX;
        flt_BoundryZ = LevelManager.instance.flt_BoundryZ;
        flt_BoundryZ_ = LevelManager.instance.flt_BoundryZ_;

        flt_CurrentTime = 0;
        flt_CurrentTimeForFireRate = 0;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_Maxtime);
        DamageIncresed.setDamageIncreased += SetDamage;
        coolDown.SetCoolDown += SetCoolDown;

    }
    private void OnDisable() {

        DamageIncresed.setDamageIncreased -= SetDamage;
        coolDown.SetCoolDown -= SetCoolDown;
    }

    private void SetDamage() {

        flt_Damage += flt_Damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    private void SetCoolDown() {

        flt_FireRate -= flt_FireRate * 0.01f * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
    }

    private void Update() {
        BulletHandler();
        PowerUpHandler();
    }

    private void PowerUpHandler() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_Maxtime) {
            this.gameObject.SetActive(false);
        }
    }

    private void BulletHandler() {
        flt_CurrentTimeForFireRate += Time.deltaTime;
     
        if (flt_CurrentTimeForFireRate >  flt_FireRate) {
            SpawnArrow();
            flt_CurrentTimeForFireRate = 0;
        }
    }

    private void SpawnArrow() {
        for (int i = 0; i < No_OfArrow_Spawn; i++) {
            Arrow current = Instantiate(arrow, GetRandomPostion(), arrow.transform.rotation);
           
            arrow.SetBulletData(flt_Damage);
        }
        
    }

    private Vector3 GetRandomPostion() {
        float flt_YPostion = 20;
        float x = Random.Range(flt_BoundryX_, flt_BoundrX);
        float z = Random.Range(flt_BoundryZ_, flt_BoundryZ);

        return new Vector3(x, flt_YPostion, z);
    }
}
