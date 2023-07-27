using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBulletMotion : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private BulletSoundSystem bulletSound;
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
    private string tag_Player = "Player";
    private string tag_Obstackle = "Obstackle";




    public void SetBulletData(Vector3 direction, float damage ,float flt_Force) {
        this.flt_Damage = damage;
        this.bulletMotionDirection = direction;
        this.flt_Force = flt_Force;
        bulletSound.Play_BulletSpawn();

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

        if (other.gameObject.CompareTag("Enemy")) {
            return;
        }
       
        if (other.gameObject.CompareTag(tag_Player)) {
            

            if (other.TryGetComponent<CollisionHandling>(out CollisionHandling collisionHandling)) {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                collisionHandling.SetHitByNormalBullet(flt_Damage, flt_Force,direction );
                BulletDetrsoySetup();

            }

        }
        
        if (other.gameObject.CompareTag(tag_Obstackle)) {
            BulletDetrsoySetup();
        }
        BulletDetrsoySetup();
    }


   

    private void BulletDetrsoySetup() {

        bulletSound.Play_BulletDestroyed();
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
