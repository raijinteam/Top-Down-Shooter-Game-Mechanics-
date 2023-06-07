using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMissileMotion : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_BulletSpeed;


   

    [Header("Partcle VFX")]
    [SerializeField] private ParticleSystem explotion;

    private Vector3 bulletMotionDirection;
    private float flt_Dealy = 5;
   




    // tag
    private string tag_Enemy = "Enemy";
  




    public void SetBulletData(Vector3 direction,float damage) {
        this.flt_Damage = damage;
        this.bulletMotionDirection = direction;
   
    }
    private void Start() {
      
       
        Destroy(gameObject, flt_Dealy);
    }
    private void Update() {
        BulletMovement();
    }

    private void BulletMovement() {
       

        transform.Translate(bulletMotionDirection * flt_BulletSpeed * Time.deltaTime, Space.World);
    }



    private void OnTriggerEnter(Collider other) {

        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        if (other.gameObject.CompareTag(tag_Enemy)) {

            EnemyyHandler(other);

        }
        
    }


    private void EnemyyHandler(Collider other) {

        if (other.gameObject.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {
            Vector3 direction = (other.gameObject.transform.position - transform.
                position).normalized;
            enemyTrigger.SethitByBullet(flt_Damage, 0,
                Vector3.zero);

            explotion.gameObject.SetActive(true);
            explotion.Play();


        }


    }


   
}
