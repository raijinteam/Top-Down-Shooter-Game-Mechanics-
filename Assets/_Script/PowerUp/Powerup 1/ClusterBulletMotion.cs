using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBulletMotion : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_BulletSpeed;

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




    public void SetBulletData(Vector3 direction, float damage, float force) {
        this.flt_Damage = damage;
        this.bulletMotionDirection = direction;
        this.flt_Force = force;


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
        if (other.gameObject.CompareTag(tag_Enemy)) {

            EnemyyHandler(other);

        }
        if (other.gameObject.CompareTag(tag_Obstackle)) {
            BulletDetrsoySetup();
        }
    }


    private void EnemyyHandler(Collider other) {

        if (other.gameObject.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            Vector3 direction = (other.gameObject.transform.position - PlayerManager.instance.Player.transform.
                position).normalized;
            enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                new Vector3(direction.x, 0, direction.z).normalized);

            BulletDetrsoySetup();

        }

     
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
