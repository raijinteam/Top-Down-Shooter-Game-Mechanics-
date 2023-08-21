using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombMotion : MonoBehaviour
{
  
    [SerializeField] private Transform[] all_SpawnPostion;
    [SerializeField] private ClusterBulletMotion clusterBullet;
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private float flt_RotationSpeed;
    [SerializeField] private float flt_Firerate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private int counter;
    private Vector3 Direction;
    [SerializeField] private float flt_DetroyTime;
    [SerializeField] private float flt_Currenttime;
    [SerializeField] private GameObject body;
    [SerializeField] private Collider thisCollider;
    [SerializeField] private ParticleSystem explotion;
    private bool isMove;
    [SerializeField] private float flt_Dealy;

    [Header("VFX")]
    [SerializeField] private ParticleSystem[] all_BulletParticleSystem;
   

    public void SetBombData(Vector3 _Direction ,float _FireRate,float _flt_Damage,float _Force,int _Counter) {

        this.Direction = _Direction;
        this.flt_Damage = _flt_Damage;
        this.flt_Firerate = _FireRate;
        this.flt_Force = _Force;
        this.counter = _Counter;
        flt_Currenttime = 0;
        Destroy(this.gameObject, flt_DetroyTime);

    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        BulletSpawnHandler();
        BulletMovenet();




    }

    private void BulletMovenet() {
        transform.Translate(Direction * flt_MovementSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * flt_RotationSpeed * Time.deltaTime);
    }

    private void BulletSpawnHandler() {


        flt_Currenttime += Time.deltaTime;
        if (flt_Currenttime > flt_Firerate) {
            Spawnbullet();
            flt_Currenttime = 0;
        }
    }

    private void Spawnbullet() {

        for (int i = 0; i < counter; i++) {

            all_BulletParticleSystem[i].Play();

            ClusterBulletMotion currentClusterBullet = Instantiate(clusterBullet,
                    all_SpawnPostion[i].transform.position, all_SpawnPostion[i].rotation);
            currentClusterBullet.SetBulletData(all_SpawnPostion[i].forward,flt_Damage,flt_Force);

        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!GameManager.instance.isPlayerLive) {
            return;
        }
     

         EnemyyHandler(other);

       
       
    }


    private void EnemyyHandler(Collider other) {

        if (other.gameObject.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            Vector3 direction = (other.gameObject.transform.position - GameManager.instance.Player.transform.
                position).normalized;
            enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                new Vector3(direction.x, 0, direction.z).normalized);

            explotion.Play();
           

        }

       
    }

  
}
