using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackHoleBulletMotion : MonoBehaviour {

    [Header("componant")]
    [SerializeField] private Collider collider_Current;
    [SerializeField] private Transform body;

    [SerializeField] private float flt_RotationSpeed;
    [SerializeField] private float flt_Speed;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;

    private bool isMotion;
        
    private Vector3 bulletDirection;
    private float flt_ExpltionTime;

    [SerializeField]private List<EnemyTrigger> List_balckHoleAffectedEnemey = new List<EnemyTrigger>();
    

    // Vfx;
  
    [SerializeField] private ParticleSystem explotion;
    private string tag_Enemy;

    //tag


    public void setBulletData(Vector3 _BulletDirection,float _Force,float _damage,
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

        if (List_balckHoleAffectedEnemey.Count == 0) {
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

        for (int i = 0; i < List_balckHoleAffectedEnemey.Count; i++) {

            if (List_balckHoleAffectedEnemey!= null) {
               
                Vector3 direction = (List_balckHoleAffectedEnemey[i].transform.position -
                    transform.position).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
               
                List_balckHoleAffectedEnemey[i].BlackHoleBlast(this.flt_Force, direction);
                
            }
           
        }

        Destroy(this.gameObject);
    }

    private void Update() {

        if (!isMotion) {
            return;
        }

        transform.Translate(bulletDirection * flt_Speed * Time.deltaTime,Space.World);
        transform.Rotate(Vector3.up*flt_RotationSpeed*Time.deltaTime);
        
    }

    private void OnTriggerEnter(Collider other) {
      

            if (other.TryGetComponent<EnemyTrigger>( out EnemyTrigger enemyTrigger)) {

                
                if (!List_balckHoleAffectedEnemey.Contains(enemyTrigger)) {

                    List_balckHoleAffectedEnemey.Add(enemyTrigger);
                    enemyTrigger.HitByBlackHole(transform);
                }
                
            }
       
    }
}
