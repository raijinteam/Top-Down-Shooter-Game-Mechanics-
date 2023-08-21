using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigArrow : MonoBehaviour
{
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Speed;
    private bool isMove = true;


    [Header("Camponant")]
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Transform body;
    [SerializeField] private BulletSoundSystem bulletSound;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem explotion;


    private Vector3 direction;

    private string tag_Player = "Player";
    private float flt_DelayOfDestroy = 2;

    public void SetArrowData(float flt_Damage, float flt_Force, Vector3 direction) {

        this.direction = direction;
        this.flt_Damage = flt_Damage;
        this.flt_Force = flt_Force;
        isMove = true;
    }

    private void Update() {

        ArrowMotion();
    }

    private void ArrowMotion() {
        if (!isMove) {
            return;
        }
        transform.Translate(direction * flt_Speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other) {
      
        if (other.gameObject.CompareTag(tag_Player)) {

            if (other.TryGetComponent<CollisionHandling>(out CollisionHandling collisionHandling)) {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                collisionHandling.SetHitByNormalBullet(flt_Damage, flt_Force, new Vector3(direction.x, 0, direction.z).normalized);
                BulletDestroy();
            }

        }
        else {
            BulletDestroy();
        }

      

    }

    private void OnCollisionEnter(Collision collision) {
        BulletDestroy();
    }

    private void BulletDestroy() {

        bulletSound.Play_BulletDestroyed();
        body.gameObject.SetActive(false);
        thisCollider.enabled = false;
        explotion.gameObject.SetActive(true);
        explotion.Play();
        isMove = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(flt_DelayOfDestroy);

        if (gameObject != null) {
            Destroy(gameObject);
        }

    }
}
