using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeMovement : MonoBehaviour
{
   

    [Header("Components")]

    [SerializeField] private Animator enemy_Animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private SlimeTrigger slimeTrigger;
    public EnemyState enemyState;
    public bool IsVisible;

    [Header("Enemy Data")]
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private int perasantageOfBlock;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private float flt_Range;

   

    public bool isGrounded = true;


    // BlackHoleData

    private Transform blackHoleTarget;
    private float flt_BlackHoleSpeed = 10;


    // KnockBack Data
    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;
    private float flt_KnockBackTime = 0.5f;
    private bool isKnockBackStart;
    private Vector3 knockBackDirection;
    private float targetAngle;
    private Vector3 dirction;
    private Quaternion KnockBackRotation;

    // Animation 

    private float flt_ScaleAnimationTime = 0.2f;
    private float flt_Reducescale = 0.3f;





    // EnemyAnimator ID
    private string id_Idle = "Idle";
    private string iD_Running = "Run";
    private string id_Attack = "Attack";

    //Coroutine
    private Coroutine cour_KncokBack;


   
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {

            enemyState = EnemyState.Idle;
            return;
        }

        EnemyAsperStateMotion();

        // EnemyMotion();
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            slimeTrigger.StopHitTidalWave();
        }
        isGrounded = false;
        enemyState = EnemyState.Not_Ground;
    }


    public void SetInVisible() {
        IsVisible = false;
        enemy_Animator.SetTrigger(id_Idle);
    }

    public void SetVisible() {
        IsVisible = true;
        enemy_Animator.SetTrigger(iD_Running);
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

    //private void EnemyMotion() {

    //    if (!isGrounded) {

    //        currentAffectedGravityForce = gravityForce;
    //        EnemyKnockBackMotion();
    //    }
    //    else {
    //        if (isKnockBackStart) {
    //            EnemyKnockBackMotion();
    //        }
    //        else {

    //            EnemyNormalMotion();

    //        }
    //    }



    //}




    private void EnemyIdleMotion() {
        enemy_Animator.SetTrigger(id_Idle);
    }

    private void EnemyNormalMotion() {


        // NORMAL MOTION
        Vector3 direction = (PlayerManager.instance.Player.transform.position - transform.position).normalized;
        float currentAngle = MathF.Atan2(dirction.x, dirction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        float flt_Distance = MathF.Abs(Vector3.Distance(transform.position, PlayerManager.instance.Player.transform.position));

        Debug.Log(flt_Distance);
        if (flt_Distance < flt_Range) {


            return;
        }
        dirction = new Vector3(direction.x, transform.position.y, direction.z);

        transform.Translate(direction * flt_MovementSpeed * Time.deltaTime, Space.World);




        //NAVMESH AGENT MoTION

        // navMeshAgent.SetDestination(PlayerManager.instance.Player.transform.position);


    }

    private void EnemyKnockBackMotion() {
        enemy_Animator.SetTrigger(id_Idle);


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }


        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
        transform.rotation = KnockBackRotation;
    }

    private void ScaleAnimation() {
        float flt_CurrntScale = transform.localScale.x;
        float flt_AnimateScale = flt_CurrntScale - flt_Reducescale;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(flt_AnimateScale, flt_ScaleAnimationTime).SetEase(Ease.InSine)).
            Append(transform.DOScaleX(flt_CurrntScale, flt_ScaleAnimationTime).SetEase(Ease.OutSine));
    }

    public void SetHitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);
    }


    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

    }


    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

        //ScaleAnimation();
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




    public void SetAttackAnimation(bool value) {
        if (value) {
            enemy_Animator.SetTrigger(id_Attack);
        }
        else {
            enemy_Animator.SetTrigger(iD_Running);
        }
    }



}
