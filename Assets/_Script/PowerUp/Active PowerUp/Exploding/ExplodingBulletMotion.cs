using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBulletMotion : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_BulletSpeed;
    [SerializeField] private float flt_Area_Damage;
    [SerializeField] private float flt_Range;
    [SerializeField] private LayerMask layermask;

    [Header("Camponanat")]
    [SerializeField] private GameObject body;
    private Collider thisCollider;

    [Header("Partcle VFX")]
    [SerializeField] private ParticleSystem explotion;

    private Vector3 bulletMotionDirection;
    private float flt_Dealy = 3;
    private bool isMove;



    // tag
    private string tag_Enemy = "Enemy";
    private string tag_Obstackle = "Obstackle";




    public void SetBulletData(Vector3 direction, float damage, float force,
                                                                float area_Damage,float flt_Range) {
        this.flt_Damage = damage;
        this.bulletMotionDirection = direction;
        this.flt_Force = force;
        this.flt_Area_Damage = area_Damage;
        this.flt_Range = flt_Range;
      



    }
    private void Start() {
        thisCollider = GetComponent<Collider>();
        isMove = true;
        Destroy(gameObject, 5);
    }
    private void Update() {
        BulletMovement();
    }

    private void BulletMovement() {
        if (!isMove) {
            return;
        }

        transform.Translate(bulletMotionDirection * flt_BulletSpeed * Time.deltaTime, Space.World);
    }



    private void OnTriggerEnter(Collider other) {

        if (!GameManager.instance.isPlayerLive) {
            return;
        }
       


            EnemyyHandler(other);

       
        if (other.gameObject.CompareTag(tag_Obstackle)) {
            BulletDetrsoySetup();
        }
    }


    private void EnemyyHandler(Collider other) {
        Debug.Log("PlayerTrigger With Enemy");
        if (other.gameObject.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            Vector3 direction = (other.gameObject.transform.position - transform.
                position).normalized;
            Debug.Log(enemyTrigger.name + "Damage" + flt_Damage);
            enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                new Vector3(direction.x, 0, direction.z).normalized);


            Debug.Log("Spheracast");
            SetSphercast(other);
           

        }


    }

    private void SetSphercast(Collider other) {

        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_Range, layermask);

        Debug.Log("Loop Start   ");

        for (int i = 0; i < all_Collider.Length; i++) {

            if (all_Collider[i] == other) {
                continue;
            }

           

                Vector3 direction = (all_Collider[i].transform.position - transform.
                                                    position).normalized;
                all_Collider[i].GetComponent<EnemyTrigger>().SethitByBullet(flt_Area_Damage, flt_Force,
                    new Vector3(direction.x, 0, direction.z).normalized);

            
        }
        Debug.Log("diable");
        BulletDetrsoySetup();
    }

    private void BulletDetrsoySetup() {
        body.gameObject.SetActive(false);
       
        thisCollider.enabled = false;
        explotion.gameObject.SetActive(true);
        explotion.Play();
        isMove = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(flt_Dealy);

        if (gameObject != null) {
            Destroy(gameObject);
        }

    }

}
