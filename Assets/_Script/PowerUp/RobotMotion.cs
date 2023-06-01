using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RobotMotion : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;

    [SerializeField] private PlayerBulletMotion playerBulletMotion;
    [SerializeField] private Transform spawnpostion;

    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] private Transform player;
    [SerializeField] private float flt_min;
    [SerializeField] private float flt_Max;
    [SerializeField] private float flt_RotationSpeed;
    [SerializeField] private float flt_ShipRoataion;
    [SerializeField] private float flt_Radius;
    [SerializeField] private float flt_Time = 0;

    private void Start() {
        player = PlayerManager.instance.Player.transform;

    }
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        FindTarget();
        BulletHandler();
        RobotMovement();
    }

    internal void SetRobotData(float flt_Damage, float flt_Force, float flt_Range,float flt_Destroy) {
        this.flt_Damage = flt_Damage;
        this.flt_Radius = flt_Range;
        this.flt_Force = flt_Force;
        flt_Time = Random.Range(0, 2f * Mathf.PI);
        flt_RotationSpeed = Random.Range(flt_min, flt_Max);
        Destroy(this.gameObject, flt_Destroy);
    }

    private void RobotMovement() {
        float x = Mathf.Cos(flt_Time) * flt_Radius;
        float y = Mathf.Sin(flt_Time) * flt_Radius;
        Vector3 newPosition = player.position + new Vector3(x, 0, y);

        transform.position = newPosition;

       
        flt_Time += flt_RotationSpeed * Time.deltaTime;

        
        if (flt_Time >= 2f * Mathf.PI) {
            flt_Time -= 2f * Mathf.PI;
        }



        
    }

    private void BulletHandler() {
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_FireRate) {
            SpawnBullet();
            flt_CurrentTime = 0;
        }
    }

    private void SpawnBullet() {
        PlayerBulletMotion bullet = Instantiate(playerBulletMotion, spawnpostion.position,
            spawnpostion.rotation);

        bullet.SetBulletData(spawnpostion.forward, flt_Damage, flt_Force,null);
    }

    private void FindTarget() {
        if (LevelManager.instance.list_AllEnemyInActiveInLevel.Count == 0) {
            return;
        }
        Transform target = null;
        float flt_MinDistance = 0;
        for (int i = 0; i < LevelManager.instance.list_AllEnemyInActiveInLevel.Count; i++) {

            Transform cururrenTransform = LevelManager.instance.list_AllEnemyInActiveInLevel[i].
                transform;
            float flt_Distance = MathF.Abs(Vector3.Distance(transform.position,
                                                cururrenTransform.position));

            if (target == null) {
                target = cururrenTransform;
                flt_MinDistance = flt_Distance;

            }
            if (flt_Distance < flt_MinDistance) {
                target = cururrenTransform;
                flt_MinDistance = flt_Distance;
            }

        }

        Vector3 direction = (target.position - transform.position).normalized;
       float flt_Angle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion target_Rotation = Quaternion.Euler(0, flt_Angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target_Rotation,
                    flt_ShipRoataion * Time.deltaTime);

        //transform.LookAt(target);
    }
}
