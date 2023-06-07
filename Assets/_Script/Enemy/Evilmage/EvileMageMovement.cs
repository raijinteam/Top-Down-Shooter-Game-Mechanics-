using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EvileMageMovement : MonoBehaviour
{
    [Header("Camponant")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private EivileMageShooting eivileMageShooting;
    public EnemyState enemyState;
    [SerializeField] private EvileMageTrigger evileMageTrigger;
    

    [Header("MovementData")]
    [SerializeField] private float flt_MoveTime;
    public bool isMove;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private Vector3 movePostion;
    [SerializeField] private LayerMask obstackleInRange;

  
    [SerializeField] private float flt_MovementSpeed;
    private bool isGrounded;
    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;
    private float flt_ScaleAnimationTime = 0.2f;
    private float flt_Reducescale = 0.3f;

    [SerializeField] private bool isVisible;


    [Header("KnockBackMotion")]
    private float flt_KnockBackTime = 0.5f;
    private bool isKnockBackStart;

  
    private float flt_KnockBackSpeed;

   

    private Vector3 knockBackDirection;

    // Courotine
    private Coroutine coro_KnockBack;

    private void OnEnable() {
        movePostion = transform.position;
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        EvilemageMotionAsperState();

       // EvileMageMotion();
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
            evileMageTrigger.StopHitTidalWave();
        }
        isGrounded = false;
        enemyState = EnemyState.Not_Ground;
    }

    public void SetInVisible() {
        isVisible = false;
        
    }

    public void SetVisible() {
        isVisible = true;
    }

    public void HitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);
    }


    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

    }


    private void EvilemageMotionAsperState() {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.knockBack) {
            EvileMageKnockBackMotion();
        }
        else if (enemyState == EnemyState.Not_Ground) {

            currentAffectedGravityForce = gravityForce;
            EvileMageKnockBackMotion();

        }
       
        else if (enemyState == EnemyState.Run) {

            if (isVisible) {
                AgentMove();
            }
            
        }
    }

   
   
    private void TimeCalaculation() {
        eivileMageShooting.SetAnimator(false);
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>flt_MoveTime) {
           
            GetRandomPostion();
            enemyState = EnemyState.Run;
        }
    }

    private void GetRandomPostion() {

         movePostion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryX),
           transform.position.y, Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));

       

    }

    private void AgentMove() {
        eivileMageShooting.SetAnimator(true);

        transform.position = Vector3.MoveTowards(transform.position, movePostion, flt_MovementSpeed * Time.deltaTime);

       float distance =  Mathf.Abs(Vector3.Distance(movePostion, transform.position));
        if (distance<0.5f) {

            GetRandomPostion();
            flt_CurrentTime = 0;
        }
    }

    private void EvileMageKnockBackMotion() {

        eivileMageShooting.SetAnimator(false); // idle Animation

        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }
    private void ScaleAnimation() {
        float flt_CurrntScale = transform.localScale.x;
        float flt_AnimateScale = flt_CurrntScale - flt_Reducescale;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(flt_AnimateScale, flt_ScaleAnimationTime).SetEase(Ease.InSine)).
            Append(transform.DOScaleX(flt_CurrntScale, flt_ScaleAnimationTime).SetEase(Ease.OutSine));
    }

    public void EveileKnockback(Vector3 _KnockBackDirection,float _KnockBackSpeed) {
        //ScaleAnimation();
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed;
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

        enemyState = EnemyState.Run;

    }
}
