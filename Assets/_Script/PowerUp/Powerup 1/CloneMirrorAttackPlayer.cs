using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMirrorAttackPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerToFollow;
    [SerializeField]private Vector3 offset;
    private float boundry;
    private float boundryX;
    private float boundryZ;

    [Header("Bullet Instantiate")]
    [SerializeField] private GameObject obj_Bullet;
    [SerializeField] private Transform spawnPosition_Bullet;
    GameObject target = null;



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


    public  void SetMirrorPlayerData(float flt_DestroyesedTime,float damage,float flt_Force, float flt_Firerate) {
        Destroy(this.gameObject, flt_DestroyesedTime);

        //Vector3 player = PlayerManager.instance.Player.transform.localPosition;
        //Vector3 mypostion = transform.localPosition;
       

        playerToFollow = PlayerManager.instance.Player.transform;

        Vector3 mypostion = transform.localPosition;

        offset = new Vector3(playerToFollow.localPosition.x - mypostion.x, playerToFollow.localPosition.y - mypostion.y, playerToFollow.localPosition.z - mypostion.z);
        flt_CurrentBulletForce = flt_Force;
        flt_CurrentFirerate = flt_Firerate;
        flt_CurrentDamage = damage;
       
        
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            Destroy(gameObject);
            return;
        }
        FireBullet();
    }

    private void FireBullet() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            isEnemyAcive = false;
            return;
        }

       
        FindTarget();

        float CoolDown = PlayerManager.instance.Player.DecreasedCoolDown(flt_CurrentFirerate);
        flt_CurrentTimeForFireRate += Time.deltaTime;
        if (flt_CurrentTimeForFireRate > CoolDown) {
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

        float Damage = PlayerManager.instance.Player.GetIncreasedDamage(flt_CurrentDamage);
        spawnedBullet.GetComponent<PlayerBulletMotion>().
            SetBulletData(spawnPosition_Bullet.forward, Damage, flt_CurrentBulletForce, target.transform,
            0,0,0);


        bulletMuzzle.Play();
    }


    private void LateUpdate() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        //Vector3 targetPostion  = PlayerManager.instance.Player.transform.position + offset;
        Vector3 targetPostion  = playerToFollow.position + offset;

        float x = Mathf.Clamp(targetPostion.x, boundryX, boundry);
        float z = Mathf.Clamp(targetPostion.z, boundryZ, boundry);

        transform.position = new Vector3(x, targetPostion.y, z);

        if (!isEnemyAcive) {
            transform.rotation = playerToFollow.rotation;
        }
       
    }





}
