using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatShootnig : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private Transform transform_SpawnPostion;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private EvileMageBulletMotion bullet;
    private bool isvisible = true;
   


    private void OnEnable() {
        flt_Damage = enemyData.GetDamage();
        flt_Force = enemyData.GetKnockBackForce();
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        if (!isvisible) {
            return;
        }
       
     
        FindTarget();
        BulletShoot();
    }

    private void BulletShoot() {
        if (batMovement.enemyState == EnemyState.isbulletSpawn) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>flt_FireRate) {
            batMovement.enemyState = EnemyState.isbulletSpawn;
            SpawnBullet();
            flt_CurrentTime = 0;
        }
    }

    public void SetVisible() {
        isvisible = true;
    }

    public void setInvisible() {
        isvisible = false;
    }

    private void SpawnBullet() {

        batMovement.SetAttck();
        StartCoroutine(Delay_SpwnBullet());
      
    }

    private IEnumerator Delay_SpwnBullet() {

        yield return new WaitForSeconds(0.5f);
        EvileMageBulletMotion current = Instantiate(bullet, transform_SpawnPostion.position,
           transform_SpawnPostion.rotation);
        current.SetBulletData(transform_SpawnPostion.forward,flt_Damage,flt_Force);
        batMovement.DefaultAnimator();
        batMovement.enemyState = EnemyState.Run;


    }

    private void FindTarget() {
        transform.LookAt(PlayerManager.instance.Player.transform);
    }
}
