using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalWave : MonoBehaviour
{
    [Header("componant")]
    [SerializeField] private Collider collider_Current;
    [SerializeField] private Transform body;

    
    [SerializeField] private float flt_Speed;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;

    private bool isMotion;

    private Vector3 bulletDirection;
    private float flt_ExpltionTime;

    [SerializeField] private List<EnemyTrigger> List_WaveAffectedEnemey = new List<EnemyTrigger>();


    // Vfx;

    [SerializeField] private ParticleSystem explotion;
   

    //tag


    public void setBulletData(Vector3 _BulletDirection, float _Force, float _damage,
        float _ExplotionTime) {

        this.bulletDirection = _BulletDirection;
        this.flt_Damage = _damage;
        this.flt_Force = _Force;
        this.flt_ExpltionTime = _ExplotionTime;
        isMotion = true;
        StartCoroutine(ExplotionBlackHole());

    }

    private IEnumerator ExplotionBlackHole() {
        yield return new WaitForSeconds(flt_ExpltionTime);

        if (List_WaveAffectedEnemey.Count == 0) {
            Destroy(this.gameObject);
        }
        else {
            body.gameObject.SetActive(false);
            collider_Current.enabled = false;
            isMotion = false;

            explotion.Play();
            StartCoroutine(DealyOfExpltion());
        }




    }

    private IEnumerator DealyOfExpltion() {
        yield return new WaitForSeconds(0.5f);
        All_EnemyGetBlackHoleVfx();
    }

    private void All_EnemyGetBlackHoleVfx() {

        for (int i = 0; i < List_WaveAffectedEnemey.Count; i++) {

            if (List_WaveAffectedEnemey[i] != null) {

                Vector3 direction = (List_WaveAffectedEnemey[i].transform.position -
                    transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                List_WaveAffectedEnemey[i].SethitByBullet(this.flt_Damage, this.flt_Force, direction);
                List_WaveAffectedEnemey[i].StopHitTidalWave();

            }

        }

        Destroy(this.gameObject);
    }

    private void Update() {

        if (!isMotion) {
            return;
        }

        transform.Translate(bulletDirection * flt_Speed * Time.deltaTime, Space.World);
        

    }

    private void OnTriggerEnter(Collider other) {


        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

            if (!List_WaveAffectedEnemey.Contains(enemyTrigger)) {
                List_WaveAffectedEnemey.Add(enemyTrigger);
               
                enemyTrigger.SetHitTidalWave(transform);
            }

        }

    }


}
