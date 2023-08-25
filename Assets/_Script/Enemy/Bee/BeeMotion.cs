using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMotion : MonoBehaviour
{


  
    [Header("Components")]

    [SerializeField]private Animator enemy_Animator;
    [SerializeField] private BeeTriger skeletonCollisionHandler;
    public EnemyState enemyState;
   

    [Header("Enemy Data")]
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private int perasantageOfBlock;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private float flt_Range;






    // KnockBack Data
    private float flt_KnockBackTime = 0.5f;

    private Vector3 knockBackDirection;
    private float targetAngle;
    private Vector3 dirction;
    private Quaternion KnockBackRotation;


    [Header("Bee Attack")]
    [SerializeField] private Wepon wepon;
    private float flt_FireRate = 1.5f;
    [SerializeField] private float flt_CurrentTimePassedForAttack = 0f;
    public bool isPlayerInRangeOfAttack = false;
    public bool isAttacking = false;




    // EnemyAnimator ID

    private string iD_Running = "Run";
    private string id_Attack = "Attack";
    private string idIdle = "Idle";
    private Coroutine cour_KncokBack;
    private Coroutine cour_Attack;

    private void OnEnable() {
        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }

        enemyState = EnemyState.Run;
       
    }

    

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
         
            return;
        }

        if (!GameManager.instance.IsInVisblePowerUpActive) {

            AttackIfPlayerInRange();
        }

        EnemyAsperStateMotion();

        // EnemyMotion();
    }

 
    public void SetInVisible() {
       
        enemy_Animator.SetTrigger(idIdle);
       
    }

    public void SetVisible() {
       
        enemy_Animator.SetTrigger(iD_Running);
    }

    private void EnemyAsperStateMotion() {

        if (enemyState == EnemyState.BlackHole) {

            return;
        }
        
        else if (enemyState == EnemyState.knockBack) {
            EnemyKnockBackMotion();
        }
        else if (enemyState == EnemyState.Run) {

                EnemyNormalMotion();
           

        }
        else if (enemyState == EnemyState.Idle) {
            EnemyIdleMotion();
        }
    }


    private void EnemyIdleMotion() {
        enemy_Animator.SetTrigger(idIdle);
    }

    private void EnemyNormalMotion() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float currentAngle = MathF.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        if (isPlayerInRangeOfAttack) {
            return;
        }
        if (isAttacking) {
            return;
        }

        // NORMAL MOTION
        
        dirction = new Vector3(direction.x, transform.position.y, direction.z);

        transform.Translate(direction * flt_MovementSpeed * Time.deltaTime, Space.World);

        //NAVMESH AGENT MoTION
        // navMeshAgent.SetDestination(PlayerManager.instance.Player.transform.position);
    }

    private void AttackIfPlayerInRange() {

        //if (!isPlayerInRangeOfAttack) {

        //    return;
        //}

        if (isAttacking) {
            return;
        }

        flt_CurrentTimePassedForAttack += Time.deltaTime;
        if (flt_CurrentTimePassedForAttack >= flt_FireRate) {

            if (!isPlayerInRangeOfAttack) {

                return;
            }
            cour_Attack = StartCoroutine(BeeAttack());
        }

    }

    private IEnumerator BeeAttack() {

        isAttacking = true;
        SetAttackAnimation(true);
        wepon.Sword.enabled = true;
      

        yield return new WaitForSeconds(0.75f);

      
        isAttacking = false;
        flt_CurrentTimePassedForAttack = 0;
        SetAttackAnimation(false);
        wepon.Sword.enabled = false;
    }

   
    public void HitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        SetAttackAnimation(false);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTimePassedForAttack = 0;
    }

    public void TidalWaveEnded() {

        enemyState = EnemyState.Run;
        enemy_Animator.SetTrigger(iD_Running);
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

        StopAllCoroutines();
        SetAttackAnimation(false);
        isAttacking = false;
        wepon.Sword.enabled = false;
        flt_CurrentTimePassedForAttack = 0;
    }


    public void SetAttackAnimation(bool value) {
        if (value) {
            Debug.Log("Attack");
            enemy_Animator.SetTrigger(id_Attack);
        }
        else {
            enemy_Animator.SetTrigger(iD_Running);
        }
    }

    private void EnemyKnockBackMotion() {
       


        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
        transform.rotation = KnockBackRotation;
    }
    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

        //ScaleAnimation();
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
