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

    private float gravityForce = -0.75f;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isHitByBlackhole = false;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;


    [SerializeField] private bool isVisible = true;



    [Header("KnockBackData")]
    [SerializeField] private float persantageOfBlock = 0;
    [SerializeField] private float flt_KnockBackTime = 0.5f;
    [SerializeField] private bool isKnockBackStart;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private Vector3 knockBackDirection;

    // Courotine
    private Coroutine coro_KnockBack;
    [SerializeField]private float flt_Offset = 2;
    private float flt_TidalDamage;

    // Tag & Id
    private const string Id_Idle = "Idle";
    private const string Id_Run = "Run";


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
            return;
        }

        if (!isGrounded) {
            currentGroundCheckTime += Time.deltaTime;
            if (currentGroundCheckTime >= groundCheckBufferTime) {
                enemyState = EnemyState.Not_Ground;
            }
        }

        if (isVisible) {
            OrcCharging();
            FindPlayerPostion();
        }


        OrcStateMotion();

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
            //oRCTrigger.StopHitTidalWave();
            StartCoroutine(WaitAndLeaveTidalWave());
        }

        currentGroundCheckTime = 0f;
        isGrounded = false;
        //enemyState = EnemyState.Not_Ground;
    }
    private IEnumerator WaitAndLeaveTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            oRCTrigger.StopHitTidalWave(flt_TidalDamage);
        }

    }

    public void CheckIfGrounded() {

        if (isGrounded) {
            enemyState = EnemyState.Idle;
        }
    }

    public void SetInVisible() {
        isVisible = false;
        lineRenderer.gameObject.SetActive(false);
        flt_CurrentTime = 0f;
    }

    public void SetVisible() {
        isVisible = true;   
    }
    public void SethitByTidal(Transform transform , float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        lineRenderer.gameObject.SetActive(false);
        this.transform.SetParent(transform);

        StopAllCoroutines();
        flt_CurrentTime = 0f;
        isAttacking = false;

        orc_Animator.SetTrigger(Id_Idle);
        orc_Weapon.SetAllColider(false);

    }


    public void orbitKnockBack( float flt_Force, Vector3 direction) {
       
      
        enemyState = EnemyState.knockBack;
        
        OrcKnockBack(direction, flt_Force);
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        lineRenderer.gameObject.SetActive(false);
        transform.SetParent(_Target);

        StopAllCoroutines();
        flt_CurrentTime = 0f;
        isAttacking = false;

        orc_Animator.SetTrigger(Id_Idle);
        orc_Weapon.SetAllColider(false);
        isHitByBlackhole = true;

    }

  

    private void OrcStateMotion() {

        if (enemyState == EnemyState.BlackHole) {
       
            return;
        }
        else if (enemyState == EnemyState.knockBack) {
         
            orcKnockBackMotion();
        }
        else if (enemyState == EnemyState.Not_Ground) {
   
            orcKnockBackMotion();
        }
        //else if (enemyState == EnemyState.charge) {

        //    if (isVisible) {
        //        orc_Animator.SetTrigger(Id_Idle);
        //        OrcCharging();
        //        FindPlayerPostion();
        //    }
           
           
        //}
        //else if (enemyState == EnemyState.Run) {

        //    if (isVisible) {
        //        orc_Animator.SetTrigger(Id_Run);
        //        ChargeTowardsPlayer();
        //    }
         
           
        //}
    }

    private void OrcCharging() {

        if (!isGrounded) {
            if (lineRenderer.gameObject.activeSelf) {
                lineRenderer.gameObject.SetActive(false);
            }
            return;
        }

        if (isHitByBlackhole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (isAttacking) {
            return;
        }

        if (!lineRenderer.gameObject.activeSelf) {
            lineRenderer.gameObject.SetActive(true);
        }

        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_ChargingTime) {
         
            flt_CurrentTime = 0;
            targetPostion = SetTargetPostion();
            lineRenderer.gameObject.SetActive(false);
            orc_Weapon.SetAllColider(true);
            isAttacking = true;

            StartCoroutine(ChargeOnPlayer());
        }
    }

    private Vector3 SetTargetPostion() {

        Vector3 Direction = ( GameManager.instance.Player.transform.position - transform.position).normalized;
        float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, GameManager.instance.Player.transform.position));

        Vector3 postion = transform.position + Direction*(flt_Distance - flt_Offset);

        return postion;

    }

    private IEnumerator ChargeOnPlayer() {

        Debug.Log("Cour   + Runniing");
        orc_Animator.SetTrigger(Id_Run);
        yield return new WaitForSeconds(0.2f);

        float currentTime = 0f;
        //float maxTimeToReachDestination = CalCulateMaxTime(targetPostion);
        float maxTimeToReachDestination = 1f;

        Vector3 startPosition = transform.position;

        while (currentTime < 1f) {
            currentTime += Time.deltaTime / maxTimeToReachDestination;

            transform.position = Vector3.Lerp(startPosition, targetPostion, currentTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        orc_Animator.SetTrigger(Id_Idle);

        orc_Weapon.SetAllColider(false);
        isAttacking = false;
       
    }

  

    private void FindPlayerPostion() {

        if (isHitByBlackhole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (isAttacking) {
            return;
        }

        Vector3 position = GameManager.instance.Player.transform.position;


        lineRenderer.SetPosition(0, new Vector3(transform.position.x, 1, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(position.x,0,position.z));

       // transform.LookAt(PlayerManager.instance.Player.transform);
        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float flt_TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float flt_RotationSpeed = 10f;


        Quaternion current = transform.rotation;
        Quaternion taregt = Quaternion.Euler(0, flt_TargetAngle, 0);

        transform.rotation = Quaternion.Slerp(current, taregt, flt_RotationSpeed*Time.deltaTime);
       
    }

  

    private void orcKnockBackMotion() {


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }

        lineRenderer.SetPosition(0, transform.position);
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void SethitByAura(Vector3 direction, float flt_Force) {
       
       
  
        enemyState = EnemyState.knockBack;
        OrcKnockBack(direction, flt_Force);
    }

    public void OrcKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

        //  ScaleAnimation();
        if (isAttacking) {
            return;
        }

        if (!lineRenderer.gameObject.activeSelf) {
            lineRenderer.gameObject.SetActive(true);
        }
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

        if (isHitByBlackhole) {
            isHitByBlackhole = false;
        }

    }

   
}
