using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EivileMageShooting : MonoBehaviour {

    [Header("Camponant")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Animator animator;
    [SerializeField]private EvileMageMovement evileMageMovement;

    [Header("Bullet Instantiate")]
    [SerializeField] private EvileMageBulletMotion bullet;
    [SerializeField] private GameObject bullet_Muzzle;
    [SerializeField] private Transform transform_BulletPostion;
  

    [Header("Shooting Data")]
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private float flt_BulletFireRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private bool isvisible;

    // Id
    private const string ID_Idle = "Idle";
    private const string Id_Attack = "Attack";
    private const string Id_Run = "Run";


    private void OnEnable() {
        damage = enemyData.GetDamage();
        force = enemyData.GetKnockBackForce();
    }
    private void Update() {
        
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        if (!isvisible) {
            return;
        }
      
        FindTarget();

    }

  


    private void FindTarget() {



        Vector3 dirction = (PlayerManager.instance.Player.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.x,dirction.z) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

      

        FireBullet();
    }

    public void SetVisible() {
        isvisible = true;
        
    }

    public void SetInVisible() {
        isvisible = false;
    }

    private void FireBullet() {

        if (evileMageMovement.enemyState == EnemyState.isbulletSpawn) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_BulletFireRate) {
            flt_CurrentTime = 0;
            evileMageMovement.enemyState = EnemyState.isbulletSpawn;
            
            animator.SetTrigger(Id_Attack);
            StartCoroutine(DelayBullet());


        }
    }
   

    private IEnumerator DelayBullet() {

        yield return new WaitForSeconds(0.3333f);
        EvileMageBulletMotion gameObject = Instantiate(bullet, transform_BulletPostion.position, transform.rotation);
        if (PlayerManager.instance.Player != null) {
            Vector3 direction = (-transform.position + PlayerManager.instance.Player.transform.position).normalized;
            gameObject.SetBulletData(direction, damage, force);

            Instantiate(bullet_Muzzle, transform_BulletPostion.position, bullet_Muzzle.transform.rotation);
        }

        yield return new WaitForSeconds(0.6667f);

        evileMageMovement.enemyState = EnemyState.Run;
        if (evileMageMovement.isMove) {
            animator.SetTrigger(Id_Run);
        }
        else {
            animator.SetTrigger(ID_Idle);
        }
    }

   
    public  void SetAnimator(bool isRun) {
        if (isRun) {
            animator.SetTrigger(Id_Run);
        }
        else {
            animator.SetTrigger(ID_Idle);
        }
        
    }
}
