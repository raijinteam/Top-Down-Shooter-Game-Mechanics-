using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class OrcEnemyMovement : MonoBehaviour {
    [Header("Camponant")]
  
    [SerializeField] private Animator orc_Animator;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private OrcWepon orc_Weapon;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private ORCTrigger oRCTrigger;


    [Header("OrcEnemyData")]
   
    [SerializeField] private float flt_ChargingTime;
    [SerializeField] private Vector3 targetPostion;

   

    [SerializeField] private float flt_SpeedOfOrc;
    [SerializeField] private float flt_Distance;

   

    [SerializeField] private float flt_CurrentTime;
    
    [SerializeField] private float flt_MovemMentSpeed;

   

    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;

    private bool isGrounded;
    private float flt_ScaleAnimationTime = 0.2f;
    private float flt_Reducescale = 0.3f;

    

   [SerializeField]private bool isSetTarget = false;

   [SerializeField] private bool isVisible = true;

   

    [Header("KnockBackData")]
    [SerializeField] private float flt_KnockBackTime = 0.5f;
    [SerializeField] private bool isKnockBackStart;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private Vector3 knockBackDirection;

    // Courotine
    private Coroutine coro_KnockBack;
    private Coroutine coro_tagetmove;
    private Rigidbody rb;

    // Tag & Id


    private const string Id_Idle = "Idle";
    private const string Id_Run = "Run";


    private void OnEnable() {
        enemyState = EnemyState.charge;
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {

           
            return;
        }
        OrcStateMotion();

    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Run) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            oRCTrigger.StopHitTidalWave();
        }
        isGrounded = false;
        enemyState = EnemyState.Not_Ground;
    }

    public void SetInVisible() {
        isVisible = false;
        lineRenderer.gameObject.SetActive(false);
    }

    public void SetVisible() {
        isVisible = true;
        lineRenderer.gameObject.SetActive(true);
    }
    public void SethitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        lineRenderer.gameObject.SetActive(false);
        this.transform.SetParent(transform);
        if (coro_tagetmove != null) {
            isSetTarget = false;
            StopCoroutine(coro_tagetmove);

        }
    }


    public void orbitKnockBack( float flt_Force, Vector3 direction) {
       
       
        if (coro_tagetmove != null) {
            isSetTarget = false;
            StopCoroutine(coro_tagetmove);

        }
    
        OrcKnockBack(direction, flt_Force);
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        lineRenderer.gameObject.SetActive(false);
        transform.SetParent(_Target);
        if (coro_tagetmove != null) {
            isSetTarget = false;
            StopCoroutine(coro_tagetmove);
            
        }

       
    }

  

    private void OrcStateMotion() {

        if (enemyState == EnemyState.BlackHole) {
            orc_Animator.SetTrigger(Id_Idle);
            return;
        }
        else if (enemyState == EnemyState.knockBack) {

            orc_Animator.SetTrigger(Id_Idle);
            orcKnockBackMotion();
        }
        else if (enemyState == EnemyState.Not_Ground) {

            orc_Animator.SetTrigger(Id_Idle);
            currentAffectedGravityForce = gravityForce;
            orcKnockBackMotion();
        }
        else if (enemyState == EnemyState.charge) {

            if (isVisible) {
                orc_Animator.SetTrigger(Id_Idle);
                OrcCharging();
                FindPlayerPostion();
            }
           
           
        }
        else if (enemyState == EnemyState.Run) {

            if (isVisible) {
                orc_Animator.SetTrigger(Id_Run);
                ChargeTowardsPlayer();
            }
         
           
        }
    }

    private void OrcCharging() {

       
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_ChargingTime) {

           
            flt_CurrentTime = 0;
            targetPostion = new Vector3(PlayerManager.instance.Player.transform.position.x, 
                1.5f, PlayerManager.instance
                .Player.transform.position.z);
            lineRenderer.gameObject.SetActive(false);
            orc_Weapon.SetAllColider(true);
            enemyState = EnemyState.Run;
            isSetTarget = false;
           

        }
    }


    private void ChargeTowardsPlayer() {

        if (!isSetTarget) {
           coro_tagetmove =  StartCoroutine(ChargeOnPlayer());
            isSetTarget = true;
        }
        
    }

    private IEnumerator ChargeOnPlayer() {

        Debug.Log("Cour   + Runniing");

        yield return new WaitForSeconds(0.2f);

        float currentTime = 0f;
        float maxTimeToReachDestination = 1.2f;

        Vector3 startPosition = transform.position;

        while (currentTime < 1f) {
            currentTime += Time.deltaTime / maxTimeToReachDestination;

            transform.position = Vector3.Lerp(startPosition, targetPostion, currentTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);


       
        lineRenderer.gameObject.SetActive(true);
        orc_Weapon.SetAllColider(false);
        enemyState = EnemyState.charge;
        isSetTarget = false;
       

    }

    private void FindPlayerPostion() {

       
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 1, transform.position.z));
        lineRenderer.SetPosition(1, PlayerManager.instance.Player.transform.position);

       // transform.LookAt(PlayerManager.instance.Player.transform);
        Vector3 direction = (PlayerManager.instance.Player.transform.position - transform.position).normalized;
        float flt_TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float flt_RotationSpeed = 10f;


        Quaternion current = transform.rotation;
        Quaternion taregt = Quaternion.Euler(0, flt_TargetAngle, 0);

        transform.rotation = Quaternion.Slerp(current, taregt, flt_RotationSpeed*Time.deltaTime);
       
    }

  

    private void orcKnockBackMotion() {


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }

        lineRenderer.SetPosition(0, transform.position);
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void SethitByAura(Vector3 direction, float flt_Force) {
       
       
        if (coro_tagetmove != null) {
            isSetTarget = false;
            StopCoroutine(coro_tagetmove);

        }
        enemyState = EnemyState.knockBack;
        OrcKnockBack(direction, flt_Force);
    }

    public void OrcKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

        //  ScaleAnimation();
        if (enemyState == EnemyState.Run) {
            return;
        }

        if (!lineRenderer.gameObject.activeSelf) {
            lineRenderer.gameObject.SetActive(true);
        }
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed;
        knockBackDirection = new Vector3(_KnockBackDirection.x, 0, _KnockBackDirection.z).normalized;
       
        if (coro_KnockBack != null) {
            StopCoroutine(coro_KnockBack);
        }

        coro_KnockBack = StartCoroutine(StopKnockbackOverTime());
    }

    private void ScaleAnimation() {
       float flt_CurrntScale = transform.localScale.x;
        float flt_AnimateScale = flt_CurrntScale - flt_Reducescale;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScaleX(flt_AnimateScale, flt_ScaleAnimationTime).SetEase(Ease.InSine)).
            Append(transform.DOScaleX(flt_CurrntScale, flt_ScaleAnimationTime).SetEase(Ease.OutSine));
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

        enemyState = EnemyState.charge;

    }

   
}
