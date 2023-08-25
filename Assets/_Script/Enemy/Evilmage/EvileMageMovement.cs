using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EvileMageMovement : MonoBehaviour
{  
    public EnemyState enemyState;
   
    [Header("Camponant")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Animator animator;
    [SerializeField] private EvileMageTrigger evileMageTrigger;
   
    

    [Header("MovementData")]
    [SerializeField] private float flt_MoveTime;
    public bool isMove;

    [SerializeField] private Vector3 movePostion;
    [SerializeField] private LayerMask obstackleInRange;

  
    [SerializeField] private float flt_MovementSpeed;
    private bool isGrounded;
    private float gravityForce = -0.75f;


    [SerializeField] private bool isVisible;
    private bool isHitByBlackhole = false;

    [Header("Shooting")]
    [SerializeField] private SinWaveBullet bullet;
    [SerializeField] private ParticleSystem bullet_Muzzle;
    [SerializeField] private Transform transform_BulletPostion;
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_currentTimePassedForFireRate = 0f;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;


    [Header("KnockBackMotion")]
    private float persantageOfBlock = 0;
    private float flt_KnockBackTime = 0.5f;
    private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;

    // Animation_ID
    private const string ID_Idle = "Idle";
    private const string Id_Attack = "Attack";
    private const string Id_Run = "Run";


    // Courotine
    private Coroutine coro_KnockBack;
    private float flt_TidalDamage;
   

    private void OnEnable() {
        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
            
            animator.SetBool(Id_Run, true);
            animator.SetBool(ID_Idle, false);
        }

        enemyState = EnemyState.Run;
        movePostion = transform.position;
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        if (!isGrounded) {
            currentGroundCheckTime += Time.deltaTime;
            if (currentGroundCheckTime >= groundCheckBufferTime) {
                enemyState = EnemyState.Not_Ground;
            }
        }

        LookTowardsPlayer();
        EvilMageShooting();
        EvilemageMotionAsperState();

       // EvileMageMotion();
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
        
    }

    private void OnCollisionStay(Collision collision) {

        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            //evileMageTrigger.StopHitTidalWave();
            //CheckIfGrounded();

            StartCoroutine(WaitAndLeaveTidalWave());
        }
        isGrounded = false;
        currentGroundCheckTime = 0f;
       // enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitAndLeaveTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            evileMageTrigger.StopHitTidalWave(flt_TidalDamage);
        }
    }

    public void SetInVisible() {
        isVisible = false;

        if (!isAttacking) {
            animator.SetBool(ID_Idle, true);
            animator.SetBool(Id_Run, false);
        }
        
    }

    public void SetVisible() {
        isVisible = true;

        if(enemyState == EnemyState.knockBack) {
            return;
        }

        animator.SetBool(ID_Idle, true);
        animator.SetBool(Id_Run, false);
    }

    public void HitByTidal(Transform transform , float Damage) {
        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        animator.SetBool(ID_Idle,true);
        animator.SetBool(Id_Run, false);
        animator.SetBool(Id_Attack,false);   
        isAttacking = false;
        flt_currentTimePassedForFireRate = 0f;

    }

    public void CheckIfGrounded() {

        if (isGrounded) {

            animator.SetBool(Id_Run, true);
            animator.SetBool(ID_Idle, false);
            enemyState = EnemyState.Run;
        }
    }


    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);
        StopAllCoroutines();
        animator.SetBool(ID_Idle, true);
        animator.SetBool(Id_Run, false);
        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        flt_currentTimePassedForFireRate = 0f;

        isHitByBlackhole = true;
    }


    private void EvilemageMotionAsperState() {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.knockBack) {
            EvileMageKnockBackMotion();
        }
        else if (enemyState == EnemyState.Not_Ground) {

            EvileMageKnockBackMotion();

        }  
        else if (enemyState == EnemyState.Run) {

            //if (isVisible) {
                AgentMove();
           // }
            
        }
    }

    private void LookTowardsPlayer() {


        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        Vector3 dirction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void EvilMageShooting() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        if (isAttacking) {
            return;
        }
       

        flt_currentTimePassedForFireRate += Time.deltaTime;

        if (flt_currentTimePassedForFireRate >= flt_FireRate) {

            isAttacking = true;
            flt_currentTimePassedForFireRate = 0f;
            animator.SetBool(Id_Run, false);
            animator.SetBool(ID_Idle, true);
            StartCoroutine(WaitAndShootBullet());

        }

    }

    private IEnumerator WaitAndShootBullet() {

        yield return new WaitForSeconds(0.3f);

        animator.SetBool(Id_Attack, true);
        animator.SetBool(ID_Idle, false);

        yield return new WaitForSeconds(0.3333f);
        SinWaveBullet gameObject = Instantiate(bullet, transform_BulletPostion.position, transform.rotation);
        if (GameManager.instance.Player != null) {
            Vector3 direction = (-transform.position + GameManager.instance.Player.transform.position).normalized;
            gameObject.SetBulletData(direction, damage, force);


            Instantiate(bullet_Muzzle, transform_BulletPostion.position, bullet_Muzzle.transform.rotation, GameManager.instance.enemySpanwble);
        }

        yield return new WaitForSeconds(0.6667f);


        if (isVisible) {
            animator.SetBool(Id_Run, true);
        }
        else {
            animator.SetBool(ID_Idle, true);
        }

        animator.SetBool(Id_Attack, false);

        flt_currentTimePassedForFireRate = 0f;
        isAttacking = false;
    }


    private void GetRandomPostion() {

         movePostion = new Vector3(Random.Range(LevelManager.instance.flt_BoundryX_, LevelManager.instance.flt_BoundryX),
           transform.position.y, Random.Range(LevelManager.instance.flt_BoundryZ_, LevelManager.instance.flt_BoundryZ));

       

    }

    private void AgentMove() {



        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        if(isAttacking) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, movePostion, flt_MovementSpeed * Time.deltaTime);

       float distance =  Mathf.Abs(Vector3.Distance(movePostion, transform.position));
        if (distance < 0.1f) {

            GetRandomPostion();
      
        }
    }

    private void EvileMageKnockBackMotion() {

     
        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }
  

    public void EveileKnockback(Vector3 _KnockBackDirection,float _KnockBackSpeed) {
        //ScaleAnimation();
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (_KnockBackSpeed*0.01f*persantageOfBlock);
        knockBackDirection = new Vector3(_KnockBackDirection.x, 0, _KnockBackDirection.z).normalized;
        if (coro_KnockBack != null) {
            StopCoroutine(coro_KnockBack);
        }

        coro_KnockBack = StartCoroutine(StopKnockbackOverTime());
    }

    private IEnumerator StopKnockbackOverTime() {

        float currentKnockbackTime = 0f;
        float maxTime = flt_KnockBackTime;

        float startForce = flt_KnockBackSpeed;
        float endForce = 0f;

        while (currentKnockbackTime < 1) {

            currentKnockbackTime += Time.deltaTime / maxTime;

            flt_KnockBackSpeed = Mathf.Lerp(startForce, endForce, currentKnockbackTime);
            yield return null;
        }

        if (isHitByBlackhole) {
            animator.SetBool(Id_Run, true);
            animator.SetBool(ID_Idle, false);
        }

        enemyState = EnemyState.Run;
        //animator.SetBool(Id_Run, true);
        //animator.SetBool(ID_Idle, false);

    }
}
