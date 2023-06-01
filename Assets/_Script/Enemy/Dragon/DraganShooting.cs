using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraganShooting : MonoBehaviour
{
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private Transform transform_SpawnPostion;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private GameObject bullet;

    private bool isVisible;

    private void OnEnable() {
        isVisible = true;
    }
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        if (!isVisible) {
            return;
        }
        
        if (batMovement.enemyState == EnemyState.Run) {
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
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
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
        GameObject current = Instantiate(bullet, transform_SpawnPostion.position,
           transform_SpawnPostion.rotation);
        current.GetComponent<BatBulletMotion>().SetBulletData(transform_SpawnPostion.forward, 0);
        batMovement.DefaultAnimator();
    }

    private void FindTarget() {
        transform.LookAt(PlayerManager.instance.Player.transform);
    }
}
