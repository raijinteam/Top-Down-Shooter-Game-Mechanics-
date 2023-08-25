using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SalaManderMotion : MonoBehaviour {

    public EnemyState enemyState;

    [Header("Camponant")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Animator animator;
    [SerializeField] private SalamaderTrigger triggerHandler;



    [Header("MovementData")]
    [SerializeField] private float flt_MoveTime;
    public bool isMove;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private Vector3 movePostion;


    [SerializeField] private float flt_MovementSpeed;
    private bool isGrounded;
    private float gravityForce = -0.75f;
  

    [SerializeField] private bool isVisible;

    [Header("Shooting")]
    [SerializeField] private EnemyNormalBullet bullet;
    [SerializeField] private ParticleSystem bullet_Muzzle;
    [SerializeField] private Transform transform_BulletPostion;
    [SerializeField] private Transform trasnform_Direction;
    [SerializeField] private float damage;
    [SerializeField] private float force;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isAttackInMotion = false;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_currentTimePassedForFireRate = 0f;
    [SerializeField] private float flt_IdleTimeBeforeAttack = 0.5f;
    [SerializeField] private float flt_currentTimePassedForIdle = 0f;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;


    [Header("KnockBackMotion")]
    private float persantageOfBlock;
    private float flt_KnockBackTime = 0.5f;
    private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;

    // Animation_ID
    private const string ID_Idle = "Idle";
    private const string Id_Attack = "Attck";
    private const string Id_Run = "Run";


    // Courotine
    private Coroutine coro_KnockBack;
    private Coroutine coro_Attack;

    private float flt_TidalDamage;
    private void OnEnable() {
        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
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

        if (isVisible) {
            LookTowardsPlayer();
            Shooting();
        }
       
        StateHandler();

       
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
            // triggerHandler.StopHitTidalWave();
            StartCoroutine(WaitHitByTidalWave());
        }
        currentGroundCheckTime = 0;
        isGrounded = false;
       // enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitHitByTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            triggerHandler.StopHitTidalWave(flt_TidalDamage);
        }
    }

    public void CheckIfGrounded() {
        if (isGrounded) {

            animator.SetBool(Id_Run, true);
            animator.SetBool(ID_Idle, false);
            enemyState = EnemyState.Run;
        }
    }

    public void SetInVisible() {
        isVisible = false;

        if (!isAttacking) {
            animator.SetBool(Id_Run, false);
            animator.SetBool(ID_Idle, true);
            animator.SetBool(Id_Attack, false);
        }
    }

    public void SetVisible() {
        isVisible = true;
   
        if (!isAttacking) {
            animator.SetBool(Id_Run, true);
            animator.SetBool(ID_Idle, false);
           
        }

    }

    public void HitByTidal(Transform transform,float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);
        animator.SetBool(Id_Run, false);
        animator.SetBool(ID_Idle, true);
        animator.SetBool(Id_Attack, false);
        StopAllCoroutines();
       
        isAttacking = false;
        isAttackInMotion = false;
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;
    }


    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);
        StopAllCoroutines();
        animator.SetBool(Id_Run, false);
        animator.SetBool(ID_Idle, true);
        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        isAttackInMotion = false;
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;
    }


    private void StateHandler() {
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

            if (isVisible) {
                AgentMove();
            }

        }
    }

    private void LookTowardsPlayer() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (isAttacking) {
            return;
        }
      
        Vector3 dirction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
    }

    private void Shooting() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }   
        else if (isAttackInMotion) {
            return;
        }


        flt_currentTimePassedForFireRate += Time.deltaTime;

        if (flt_currentTimePassedForFireRate >= flt_FireRate) {

            isAttacking = true;

            flt_currentTimePassedForIdle += Time.deltaTime;

            if (flt_currentTimePassedForIdle >= flt_IdleTimeBeforeAttack) {
                // attack
                isAttackInMotion = true;

                animator.SetBool(ID_Idle, true);
                animator.SetBool(Id_Run, false);

                if (coro_Attack != null) {
                    StopCoroutine(coro_Attack);
                }
                coro_Attack = StartCoroutine(WaitAndShootBullet());
            }
        }

    }

    private IEnumerator WaitAndShootBullet() {

        trasnform_Direction.position = transform_BulletPostion.position;
        yield return new WaitForSeconds(0.2f);
       
        animator.SetBool(ID_Idle, false);
        animator.SetBool(Id_Attack, true);
        Debug.Log("attack");
       
        float rotationValue = -30f;
       

        for (int i = 0; i < 3; i++) {

            yield return new WaitForSeconds(0.5f);

            EnemyNormalBullet current = Instantiate(bullet, transform_BulletPostion.position,
           Quaternion.identity, GameManager.instance.enemySpanwble);


            Instantiate(bullet_Muzzle, transform_BulletPostion.position,
              Quaternion.identity);


            Vector3 direction = Vector3.zero;
            direction = Quaternion.Euler(0, rotationValue, 0) * trasnform_Direction.forward;
            current.SetBulletData(direction, damage, force);

            rotationValue += 30;




        }

        yield return new WaitForSeconds(0.7f);

        coro_Attack = null;
        enemyState = EnemyState.Run;
        if (isVisible) {
            animator.SetBool(Id_Run, true);
        }
        else {
            animator.SetBool(ID_Idle, true);
        }
        animator.SetBool(Id_Attack, false);
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;
        isAttackInMotion = false;
        isAttacking = false;
    }



    private void AgentMove() {
        if (isAttackInMotion) {
            return;
        }


       // animator.SetTrigger(Id_Run);
        Debug.Log("run");


        transform.position = Vector3.MoveTowards(transform.position, movePostion, flt_MovementSpeed * Time.deltaTime);

        float distance = Mathf.Abs(Vector3.Distance(movePostion, transform.position));
        if (distance < 0.5f) {

            movePostion = LevelManager.instance.GetRandomPostion(transform.position.y);
            flt_CurrentTime = 0;
        }
    }

    private void EvileMageKnockBackMotion() {

       

        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }


    public void KnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {
       
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
        
        float startForce = flt_KnockBackSpeed;
        float endForce = 0f;

        while (currentKnockbackTime < 1) {

            currentKnockbackTime += Time.deltaTime / flt_KnockBackTime;

            flt_KnockBackSpeed = Mathf.Lerp(startForce, endForce, currentKnockbackTime);
            yield return null;
        }

        enemyState = EnemyState.Run;
         

    }
}
