using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerBulletMotion : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_BulletSpeed;
    [SerializeField] private float IncreasedHealth;
    [SerializeField] private Transform target;

    [Header("Camponanat")]
    [SerializeField] private GameObject body;
    private Collider thisCollider;

    [Header("Partcle VFX")]
    [SerializeField] private ParticleSystem explotion;

    private Vector3 bulletMotionDirection;
    private float flt_Dealy = 3;
    private bool isMove;

    [Header("Richoest PowerUp")]
    [SerializeField] private List<GameObject> list_Enemy;
    [SerializeField] private int MaxCounter;
    [SerializeField] private int currentCounter;
    [SerializeField] private int damagePersantage;


    [Header(" ----DamageBlow ----")]
    [SerializeField] private int persantageOfDelathBow;



    // tag
    private string tag_Enemy = "Enemy";
    private string tag_Obstackle = "Obstackle";
    

    public void SetBulletData(Vector3 direction, float damage, float force, Transform  target) {
        this.flt_Damage = damage;
        this.bulletMotionDirection = direction;
        this.flt_Force = force;
        this.target = target;
        currentCounter = 0;
    
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
      
        transform.Translate(bulletMotionDirection * flt_BulletSpeed * Time.deltaTime,Space.World);
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
            Vector3 direction = (other.gameObject.transform.position - transform.
                position).normalized;
            enemyTrigger.SethitByBullet(flt_Damage, flt_Force,
                new Vector3(direction.x, 0, direction.z).normalized);

            if (GameManager.instance.isRichoestPowerUp) {
                RechoestPowerUp();
            }
            else if (GameManager.instance.isDealthBlowPowerUpActivated) {
                int Inedex = Random.Range(0, 100);
                if (Inedex <= persantageOfDelathBow) {
                    enemyTrigger.SetDamageBlast();
                }
            }
            else {
                BulletDetrsoySetup();
                
            }
          



        }

       
    }

    private void RechoestPowerUp() {
        currentCounter++;
        if (currentCounter < MaxCounter) {

            flt_Damage = flt_Damage - ((flt_Damage * damagePersantage) / 100);
            flt_Damage = ((int)flt_Damage);
            Debug.Log("FindTarget");
            FindTarget();

        }
        else {
            BulletDetrsoySetup();
        }
    }

    private void FindTarget() {

        float MinDistnce = 0;
        if (target != null) {
            list_Enemy.Add(target.gameObject);
            target = null;
        }
     

        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            if (GameManager.instance.list_ActiveEnemies == null) {
                continue;
            }
            if (list_Enemy.Contains(GameManager.instance.list_ActiveEnemies[i].gameObject)) {
                continue;
            }


            float distance = MathF.Abs(Vector3.Distance(transform.position,
                GameManager.instance.list_ActiveEnemies[i].transform.position));
            if (target == null) {
                target = GameManager.instance.list_ActiveEnemies[i];
                MinDistnce = distance;
            }
            else {
                if (distance < MinDistnce) {
                    target = GameManager.instance.list_ActiveEnemies[i].transform;
                    MinDistnce = distance;
                }
            }
        }

        if (target == null) {
            BulletDetrsoySetup();
            return;
        }
        transform.LookAt(target.transform);
         bulletMotionDirection = transform.forward;
       
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
