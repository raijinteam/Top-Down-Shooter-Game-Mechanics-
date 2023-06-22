using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraganShooting : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private Transform transform_SpawnPostion;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private BatBulletMotion bullet;
    [SerializeField] private GameObject bullet_Muzzle;

    private bool isVisible;

    private void OnEnable() {
        isVisible = true;

        flt_Damage = enemyData.GetDamage();
        flt_Force = enemyData.GetKnockBackForce();
        
    }
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        if (!isVisible) {
            return;
        }
        
        
        FindTarget();
        BulletShoot();
    }


    public void setInvisible() {
        isVisible = false;
    }

    public void SetVisible() {
        isVisible = true;
    }

    private void BulletShoot() {
        if (batMovement.enemyState == EnemyState.isbulletSpawn) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            batMovement.enemyState = EnemyState.isbulletSpawn;
            SpawnBullet();
            flt_CurrentTime = 0;
        }
    }

   

    private void SpawnBullet() {

        batMovement.SetAttck();
        StartCoroutine(Delay_SpwnBullet());

    }

    private IEnumerator Delay_SpwnBullet() {

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++) {

            BatBulletMotion current = Instantiate(bullet, transform_SpawnPostion.position,
            transform_SpawnPostion.rotation);
            Instantiate(bullet_Muzzle, transform_SpawnPostion.position,
               transform_SpawnPostion.rotation);
            current.SetBulletData(transform_SpawnPostion.forward, flt_Damage, flt_Force);

            if (i == 0) {

                yield return new WaitForSeconds(0.6667f);
            }
            else if (i == 1) {
                yield return new WaitForSeconds(0.3333f);
            }
            
        }
       
        
        batMovement.enemyState = EnemyState.Run;
    }

    private void FindTarget() {
        transform.LookAt(PlayerManager.instance.Player.transform);
    }
}
