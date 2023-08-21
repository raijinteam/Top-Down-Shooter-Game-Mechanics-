using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonMovement : MonoBehaviour {

    [Header("Components")]

    [SerializeField] private Animator enemy_Animator;
    [SerializeField] private SkeletonCollisionHandler skeletonCollisionHandler;
    private EnemyState enemyState;
    private bool IsVisible;

    [Header("Enemy Data")]
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private int perasantageOfBlock;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private float flt_Range;
    public bool isGrounded = true;


    [Header("Shooting")]
    [SerializeField] private Wepon wepon;
    private float flt_DealyBetweenTwoAttack = 0.5f;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private bool isAttacking;
    public bool isAttckinInRange;



   

    // KnockBack Data
    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;
    private float flt_KnockBackTime = 0.5f;
    private bool isKnockBackStart;
    private Vector3 knockBackDirection;

    private Vector3 dirction;
    private Quaternion KnockBackRotation;

   

    // EnemyAnimator ID
    private string id_Idle = "Idle";
    private string iD_Running = "Run";
    private string id_Attack = "Attack";

    //Coroutine
    private Coroutine cour_KncokBack;
    private Coroutine cour_Attack;
    private float tidalDamage;

    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
    }


    private void Update() {
        if (!GameManager.instance.isPlayerLive) {

            enemyState = EnemyState.Idle;
            return;
        }
        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }


        EnemyAsperStateMotion();
        Shooting();

        // EnemyMotion();
    }

    private void Shooting() {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            return;
        }

        HandlingAttack();
    }

    private void HandlingAttack() {

        if (isAttckinInRange) {

            if (isAttacking) {
                return;
            }
            flt_CurrentTime += Time.deltaTime;

            if (flt_CurrentTime > flt_DealyBetweenTwoAttack) {

                if (cour_Attack != null) {
                    StopCoroutine(cour_Attack);
                }
                  cour_Attack =  StartCoroutine(AttckBySword());
            }


        }



    }

    private IEnumerator AttckBySword() {


        enemy_Animator.SetBool(id_Attack, true);
        isAttacking = true;
        wepon.Sword.enabled = true;

        yield return new WaitForSeconds(0.75f);
        isAttacking = false;
        flt_CurrentTime = 0;
        wepon.Sword.enabled = false;

        if (isAttckinInRange) {

            enemy_Animator.SetBool(id_Idle, true);
        }
        else {

            enemy_Animator.SetBool(iD_Running, true);
        }
    }



    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
        enemyState = EnemyState.Run;
    }

    private void OnCollisionExit(Collision collision) {



        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            skeletonCollisionHandler.StopHitTidalWave(tidalDamage);

        }
        isGrounded = false;
        enemyState = EnemyState.Not_Ground;
    }


    public void SetInVisible() {
        IsVisible = false;
        enemy_Animator.SetBool(id_Idle, true);

        StopAllCoroutines();
        enemy_Animator.SetBool(id_Idle, true);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTime = 0;
    }

    public void SetVisible() {
        IsVisible = true;
        enemy_Animator.SetBool(iD_Running, true);
    }

    private void EnemyAsperStateMotion() {

        if (enemyState == EnemyState.BlackHole) {

            return;
        }


        else if (enemyState == EnemyState.Not_Ground) {
            EnemyNot_GroundMotion();
        }
        else if (enemyState == EnemyState.knockBack) {
            EnemyKnockBackMotion();
        }
        else if (enemyState == EnemyState.Run) {

            if (IsVisible) {
                EnemyNormalMotion();
            }

        }
        else if (enemyState == EnemyState.Idle) {
            EnemyIdleMotion();
        }
    }





    private void EnemyNot_GroundMotion() {
        currentAffectedGravityForce = gravityForce;
        EnemyKnockBackMotion();
    }

    private void EnemyIdleMotion() {
        enemy_Animator.SetTrigger(id_Idle);
    }

    private void EnemyNormalMotion() {

        if (isAttckinInRange) {
            return;
        }

   
        // NORMAL MOTION
        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float currentAngle = MathF.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        dirction = new Vector3(direction.x, transform.position.y, direction.z);

        transform.Translate(direction * flt_MovementSpeed * Time.deltaTime, Space.World);




        //NAVMESH AGENT MoTION

        // navMeshAgent.SetDestination(PlayerManager.instance.Player.transform.position);


    }

    private void EnemyKnockBackMotion() {
        enemy_Animator.SetBool(id_Idle, true);


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }


        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
        transform.rotation = KnockBackRotation;
    }

  

    public void HitByTidal(Transform transform , float Damage) {
        tidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        enemy_Animator.SetBool(id_Idle, true);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTime = 0;
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

        StopAllCoroutines();
        enemy_Animator.SetBool(id_Idle, true);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTime = 0;

    }


    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

        enemy_Animator.SetBool(id_Idle, true);
        isKnockBackStart = true;
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = knockBackSpeed - (knockBackSpeed * perasantageOfBlock / 100);
        knockBackDirection = dirction;
        KnockBackRotation = transform.rotation;
        if (cour_KncokBack != null) {
            StopCoroutine(cour_KncokBack);
        }

        cour_KncokBack = StartCoroutine(StopKnockbackOverTime());
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

        isKnockBackStart = false;
        enemyState = EnemyState.Run;
     

    }
}
