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

    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    
   
    [SerializeField] private float flt_ShipRoataion;
   
  

    private void Start() {
        player = PlayerManager.instance.Player.transform;
        offset = new Vector3(player.position.x - transform.position.x, player.position.y -
                transform.position.y, player.position.z - transform.position.z);
    }
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        FindTarget();
        BulletHandler();
        RobotMovement();
    }

    internal void SetRobotData(float flt_Damage, float flt_Force, float flt_Destroy) {
        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
      
        Destroy(this.gameObject, flt_Destroy);
    }

    private void RobotMovement() {

        transform.position = player.position + offset;

        
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
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
        Transform target = null;
        float flt_MinDistance = 0;
        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            Transform cururrenTransform = GameManager.instance.list_ActiveEnemies[i].
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
