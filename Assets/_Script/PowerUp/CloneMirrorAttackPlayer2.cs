using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneMirrorAttackPlayer2 : MonoBehaviour
{
  
    private float boundry;
    private float boundryX;
    private float boundryZ;

    [Header("Bullet Instantiate")]
    [SerializeField] private GameObject obj_Bullet;
    [SerializeField] private Transform spawnPosition_Bullet;
    [SerializeField] private Animator animator;
    GameObject target = null;

    [Header("MovementData")]
    [SerializeField] private bool isMovePlayer;
    [SerializeField] private float flt_CurrentTimeForCharging;
    [SerializeField] private bool isPlayerCharged;
    [SerializeField] private float flt_ChargingTime;
    [SerializeField] private Vector3 targetPostion;
    [SerializeField]private float flt_MovementSpeed;




    [Header("RunTimeValue")]
    [SerializeField] private float flt_CurrentBulletForce;
    [SerializeField] private float flt_CurrentFirerate;
    [SerializeField] private float flt_CurrentDamage;
    [SerializeField] private float flt_CurrentTimeForFireRate;

    [Header("VFX")]
    [SerializeField] private ParticleSystem bulletMuzzle;

    [Header("Enemy Handling")]
    private bool isEnemyAcive = true;
    [SerializeField] private float targetAngle;
    [SerializeField] private float currentAngle;
    [SerializeField] private float flt_SpeedOfTarget = 250;
   

    private void Start() {
        boundry = LevelManager.instance.flt_Boundry;
        boundryX = LevelManager.instance.flt_BoundryX;
        boundryZ = LevelManager.instance.flt_BoundryZ;
    }


    public void SetMirrorPlayerData(float flt_DestroyesedTime, float damage, float flt_Force, float flt_Firerate) {
        Destroy(this.gameObject, flt_DestroyesedTime);

        flt_CurrentBulletForce = flt_Force;
        flt_CurrentFirerate = flt_Firerate;
        flt_CurrentDamage = damage;


    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            Destroy(gameObject);
            return;
        }
        if (isMovePlayer) {
            PlayerMotion();
        }
       
        FireBullet();
    }

    private void PlayerMotion() {

        if (isPlayerCharged) {
            animator.SetTrigger("Run");
            PlayerMoveTowordTarget();
        }
        else {
            animator.SetTrigger("idle");
            ChargPlayer();
        }
    }

    private void PlayerMoveTowordTarget() {
        if (!isPlayerCharged) {
            return;
        }

       
        transform.position = Vector3.MoveTowards(transform.position, targetPostion, 
                                    flt_MovementSpeed * Time.deltaTime);
        transform.LookAt(targetPostion);
        float flt_Distance = Math.Abs(Vector3.Distance(transform.position, targetPostion));
        if (flt_Distance<0.5f) {
            isPlayerCharged = false;
        }
    }

    private void ChargPlayer() {
        if (isPlayerCharged) {
            return;
        }
        flt_CurrentTimeForCharging += Time.deltaTime;

        if (flt_CurrentTimeForCharging>flt_ChargingTime) {
            flt_CurrentTimeForCharging = 0;
            isPlayerCharged = true;
            targetPostion = GetRandomTargetPostion();
        }
    }

    private Vector3 GetRandomTargetPostion() {
        Vector3 postion = new Vector3(Random.Range(boundry, boundryX), transform.position.y,
                                               Random.Range(boundry, boundryZ));

        return postion;
    }

    private void FireBullet() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            isEnemyAcive = false;
            return;
        }


        FindTarget();

        flt_CurrentTimeForFireRate += Time.deltaTime;
        if (flt_CurrentTimeForFireRate > flt_CurrentFirerate) {
            flt_CurrentTimeForFireRate = 0;
            SpawnBullet();
        }
    }

    private void FindTarget() {

        float MinDistnce = 0;
      


        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            if (GameManager.instance.list_ActiveEnemies[i] == null) {
                continue;
            }
            float distance = MathF.Abs(Vector3.Distance(transform.position,
               GameManager.instance.list_ActiveEnemies[i].transform.position));
            if (target == null) {
                target = GameManager.instance.list_ActiveEnemies[i].gameObject;
                MinDistnce = distance;
            }
            else {
                if (distance < MinDistnce) {
                    target = GameManager.instance.list_ActiveEnemies[i].gameObject;
                    MinDistnce = distance;
                }
            }
        }

        if (target == null) {
            return;
        }

        Vector3 dirction = (target.transform.position - transform.position).normalized;
        targetAngle = Mathf.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, flt_SpeedOfTarget * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, targetAngle, 0);
        spawnPosition_Bullet.LookAt(target.transform);




    }

    private void SpawnBullet() {

        GameObject spawnedBullet = Instantiate(obj_Bullet, spawnPosition_Bullet.position, spawnPosition_Bullet.rotation);


        spawnedBullet.GetComponent<PlayerBulletMotion>().
            SetBulletData(spawnPosition_Bullet.forward, flt_CurrentDamage, flt_CurrentBulletForce,target.transform);


        bulletMuzzle.Play();
    }


}
