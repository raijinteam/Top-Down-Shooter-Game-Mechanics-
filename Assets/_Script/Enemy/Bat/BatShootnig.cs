using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatShootnig : MonoBehaviour
{
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private Transform transform_SpawnPostion;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private GameObject bullet;
    private bool isvisible = true;

  

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        if (!isvisible) {
            return;
        }
       
        if (batMovement.enemyState == EnemyState.Run) {
            return;
        }
        FindTarget();
        BulletShoot();
    }

    private void BulletShoot() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>flt_FireRate) {
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
        GameObject current = Instantiate(bullet, transform_SpawnPostion.position,
           transform_SpawnPostion.rotation);
        current.GetComponent<EvileMageBulletMotion>().SetBulletData(transform_SpawnPostion.forward, 0);
        batMovement.DefaultAnimator();
    }

    private void FindTarget() {
        transform.LookAt(PlayerManager.instance.Player.transform);
    }
}
