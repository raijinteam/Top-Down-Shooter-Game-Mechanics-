using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AstrlArrowPowerUp : MonoBehaviour {
   

    private float flt_Boundry;
    private float flt_BoundrX;
    private float flt_BoundryZ;


    [SerializeField] private int No_OfArrow_Spawn;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_FireRate;

    [SerializeField] private float flt_CurrentTimeForFireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Maxtime;
   
    [SerializeField] private Arrow arrow;


    private void OnEnable() {
        SpawnArrow();
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_BoundrX = LevelManager.instance.flt_BoundryX;
        flt_BoundryZ = LevelManager.instance.flt_BoundryZ;
        flt_CurrentTime = 0;
        flt_CurrentTimeForFireRate = 0;
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_Maxtime);
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
        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_FireRate);
        if (flt_CurrentTimeForFireRate > CoolDown) {
            SpawnArrow();
            flt_CurrentTimeForFireRate = 0;
        }
    }

    private void SpawnArrow() {
        for (int i = 0; i < No_OfArrow_Spawn; i++) {
            Arrow current = Instantiate(arrow, GetRandomPostion(), arrow.transform.rotation);
            float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_Damage);
            arrow.SetBulletData(Damage);
        }
        
    }

    private Vector3 GetRandomPostion() {
        float flt_YPostion = 20;
        float x = Random.Range(flt_Boundry, flt_BoundrX);
        float z = Random.Range(flt_Boundry, flt_BoundryZ);

        return new Vector3(x, flt_YPostion, z);
    }
}
