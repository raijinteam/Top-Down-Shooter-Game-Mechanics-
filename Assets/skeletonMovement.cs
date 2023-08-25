using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonMovement : MonoBehaviour {

    [Header("Components")]

    [SerializeField] private Animator enemy_Animator;
    [SerializeField] private SkeletonCollisionHandler skeletonCollisionHandler;
    [SerializeField] private EnemyState enemyState;
    private bool IsVisible;

    [Header("Enemy Data")]
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private int perasantageOfBlock;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private float flt_Range;
    public bool isGrounded = true;

    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;


    [Header("Shooting")]
    [SerializeField] private Wepon wepon;
    private float flt_DealyBetweenTwoAttack = 0.5f;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private bool isAttacking;
    public bool isAttckinInRange;


    // KnockBack Data
    private float gravityForce = -0.75f;
    private float flt_KnockBackTime = 0.5f;
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

        if (!isGrounded) {
            currentGroundCheckTime += Time.deltaTime;
            if (currentGroundCheckTime >= groundCheckBufferTime) {
                enemyState = EnemyState.Not_Ground;
            }
        }

        EnemyAsperStateMotion();
        Shooting();

        // EnemyMotion();
    }

    private void Shooting() {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (!IsVisible) {
            return;
        }

        HandlingAttack();
    }

    private void HandlingAttack() {

        if (isAttacking) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;

        if (isAttckinInRange) {

            enemy_Animator.SetBool(id_Idle, true);
            enemy_Animator.SetBool(iD_Running, false);
        
            if (flt_CurrentTime > flt_DealyBetweenTwoAttack) {
                isAttacking = true;       
                 cour_Attack =  StartCoroutine(AttckBySword());
            }
        }
    }

    private IEnumerator AttckBySword() {


        enemy_Animator.SetBool(id_Idle, false);
        enemy_Animator.SetBool(id_Attack, true);
        wepon.Sword.enabled = true;

        yield return new WaitForSeconds(0.75f);
        isAttacking = false;
        flt_CurrentTime = 0;
        wepon.Sword.enabled = false;

        if (IsVisible) {
            if (isAttckinInRange) {

                enemy_Animator.SetBool(id_Idle, true);
                enemy_Animator.SetBool(id_Attack, false);

            }
            else {

                enemy_Animator.SetBool(iD_Running, true);
                enemy_Animator.SetBool(id_Attack, false);
            }
        }
        else {

            enemy_Animator.SetBool(id_Idle, true);
            enemy_Animator.SetBool(id_Attack, false);
        }
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
           // skeletonCollisionHandler.StopHitTidalWave(tidalDamage);
            StartCoroutine(WaitHitByTidalWave());

        }
        isGrounded = false;
        currentGroundCheckTime = 0f;
        //enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitHitByTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            skeletonCollisionHandler.StopHitTidalWave(tidalDamage);
        }
    }

    public void CheckIfGrounded() {
        if (isGrounded) {

            enemy_Animator.SetBool(iD_Running, true);
            enemy_Animator.SetBool(id_Idle, false);
            enemyState = EnemyState.Run;
        }
    }


    public void SetInVisible() {
        IsVisible = false;

        if (!isAttacking) {
            enemy_Animator.SetBool(id_Idle, true);
            enemy_Animator.SetBool(id_Attack, false);
            enemy_Animator.SetBool(iD_Running, false);
        }

    }

    public void SetVisible() {
        IsVisible = true;

        enemy_Animator.SetBool(id_Idle, false);
        enemy_Animator.SetBool(id_Attack, false);
        enemy_Animator.SetBool(iD_Running, true);
    }

    private void EnemyAsperStateMotion() {

        if (enemyState == EnemyState.BlackHole) {

            return;
        }
        else if(enemyState == EnemyState.Wave) {

            return;
        }
        else if (enemyState == EnemyState.Not_Ground) {
            EnemyKnockBackMotion();
        }
        else if (enemyState == EnemyState.knockBack) {
            EnemyKnockBackMotion();
        }
        else if (enemyState == EnemyState.Run) {

            if (IsVisible) {
                EnemyNormalMotion();
            }

        }
       
    }

    private void EnemyNormalMotion() {

        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
     
        dirction = new Vector3(direction.x, transform.position.y, direction.z);
        float currentAngle = MathF.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        if (isAttacking) {
            return;
        }
        else if (isAttckinInRange) {                  
            return;
        }

        enemy_Animator.SetBool(id_Idle, false);
        enemy_Animator.SetBool(iD_Running, true);

        // NORMAL MOTION
        //transform.eulerAngles = new Vector3(0, currentAngle, 0);

      

        transform.Translate(direction * flt_MovementSpeed * Time.deltaTime, Space.World);

        //NAVMESH AGENT MoTION

        // navMeshAgent.SetDestination(PlayerManager.instance.Player.transform.position);
    }

    private void EnemyKnockBackMotion() {
        
        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
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
        enemy_Animator.SetBool(id_Attack, false);
        enemy_Animator.SetBool(iD_Running, false);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTime = 0;
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

        StopAllCoroutines();
        enemy_Animator.SetBool(id_Idle, true);
        enemy_Animator.SetBool(id_Attack, false);
        enemy_Animator.SetBool(iD_Running, false);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTime = 0;

    }


    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

       
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
