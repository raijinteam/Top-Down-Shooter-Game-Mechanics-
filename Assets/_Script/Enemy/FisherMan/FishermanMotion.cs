using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishermanMotion : MonoBehaviour
{
    [Header("Camponant")]

    [SerializeField] private Animator myanimater;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Wepon weapon;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private fisherManTrigger triggerHandler;


    [Header("FisherManData")]
    [SerializeField]private float flt_Offset;
    [SerializeField] private float flt_ChargingTime;
    [SerializeField] private Vector3 targetPostion;
    [SerializeField] private float flt_SpeedOfOrc;
    [SerializeField] private float flt_Distance;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MovemMentSpeed;
    private bool isAttacking = false;
    private bool isHitByBlackhole = false;

    private float gravityForce = -0.75f;
    private bool isGrounded;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;
    private float maxDistanceToPlayer = 28f;
    private float maxTimeToReachPlayer = 1.75f;

    [SerializeField] private bool isVisible = true;

    [Header("KnockBackData")]
    [SerializeField] private float persantageOfBlock = 0;
    [SerializeField] private float flt_KnockBackTime = 0.5f;
    [SerializeField] private bool isKnockBackStart;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private Vector3 knockBackDirection;

    // Courotine
    private Coroutine coro_KnockBack;
    private Coroutine coro_tagetmove;
    private float flt_TidalDamage;


    // Tag & Id


    private const string Id_Idle = "Idle";
    private const string Id_Run = "Run";
    private const string Id_Attack = "Attack";


    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
       //enemyState = EnemyState.charge;
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {

            myanimater.SetBool(Id_Idle, true);
            myanimater.SetBool(Id_Attack, false);
            myanimater.SetBool(Id_Run, false);

            return;
        }

        if (!isGrounded) {
            currentGroundCheckTime += Time.deltaTime;
            if(currentGroundCheckTime >= groundCheckBufferTime) {
                enemyState = EnemyState.Not_Ground;
            }
        }

        if (isVisible) {

            ChargeAttack();
            FindPlayerPostion();
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
            StartCoroutine(WaitAndLeaveTidalWave());
            //triggerHandler.StopHitTidalWave();

        }

        isGrounded = false;
        currentGroundCheckTime = 0f;
        //enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitAndLeaveTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            triggerHandler.StopHitTidalWave(flt_TidalDamage);
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
        //if (enemyState == EnemyState.charge) {
        //    lineRenderer.gameObject.SetActive(true);
        //}
   
    }


    public void SethitByTidal(Transform transform , float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        lineRenderer.gameObject.SetActive(false);
        this.transform.SetParent(transform);    

        StopAllCoroutines();
        flt_CurrentTime = 0f;
        isAttacking = false;

        myanimater.SetBool(Id_Idle, true);
        myanimater.SetBool(Id_Attack, false);
        myanimater.SetBool(Id_Run, false);
        weapon.Sword.enabled = false;
        weapon.isSwordSide = false;
    }




    public void orbitKnockBack(float flt_Force, Vector3 direction) {
        OrcKnockBack(direction, flt_Force);
    }

    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        lineRenderer.gameObject.SetActive(false);
        transform.SetParent(_Target);
      

        StopAllCoroutines();
        flt_CurrentTime = 0f;
        isAttacking = false;

        myanimater.SetBool(Id_Idle, true);
        myanimater.SetBool(Id_Attack, false);
        myanimater.SetBool(Id_Run, false);
        weapon.Sword.enabled = false;
        weapon.isSwordSide = false;
        isHitByBlackhole = true;
    }



    private void StateHandler() {

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
                 
        //        OrcCharging();
        //        FindPlayerPostion();
        //    }


        //}
        //else if (enemyState == EnemyState.Run) {        
        //    ChargeTowardsPlayer();
        //}
    }

    private void ChargeAttack() {

        if (!isGrounded) {


            if (lineRenderer.gameObject.activeSelf) {
                lineRenderer.gameObject.SetActive(false);
            }

            return;
        }

        if (isHitByBlackhole) {
            return;
        }
        else if(enemyState == EnemyState.Wave) {
            return;
        }
        else if(isAttacking) {  
            return;
        }

        if (!lineRenderer.gameObject.activeSelf) {
            lineRenderer.gameObject.SetActive(true);
        }


        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_ChargingTime) {


            flt_CurrentTime = 0;

            Vector3 PlayerPostion = new Vector3(GameManager.instance.Player.transform.position.x, transform.position.y, GameManager.instance.Player.transform.position.z);
            Vector3 targetDirection = (PlayerPostion - transform.position).normalized;

            float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, PlayerPostion));

            targetPostion = transform.position + targetDirection * (flt_Distance - flt_Offset);
            
            lineRenderer.gameObject.SetActive(false);

            //enemyState = EnemyState.Run;
            //isSetTarget = false;

            isAttacking = true;
            coro_tagetmove = StartCoroutine(ChargeOnPlayer());
        }
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
        lineRenderer.SetPosition(1, new Vector3(position.x, 0, position.z));

        // transform.LookAt(PlayerManager.instance.Player.transform);
        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float flt_TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float flt_RotationSpeed = 10f;


        Quaternion current = transform.rotation;
        Quaternion taregt = Quaternion.Euler(0, flt_TargetAngle, 0);

        transform.rotation = Quaternion.Slerp(current, taregt, flt_RotationSpeed * Time.deltaTime);

    }


    private IEnumerator ChargeOnPlayer() {

        myanimater.SetBool(Id_Idle, false);
        myanimater.SetBool(Id_Attack, false);
        myanimater.SetBool(Id_Run, true);

        yield return new WaitForSeconds(0.2f);

        float currentTime = 0f;
        float maxTimeToReachDestination = CalCulateMaxTime(targetPostion);

        Vector3 startPosition = transform.position;

        while (currentTime < 1f) {
            currentTime += Time.deltaTime / maxTimeToReachDestination;

            transform.position = Vector3.Lerp(startPosition, targetPostion, currentTime);
            yield return null;
        }


        myanimater.SetBool(Id_Idle, false);
        myanimater.SetBool(Id_Attack, true);
        myanimater.SetBool(Id_Run, false);

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Here");
        weapon.isSwordSide = true;
        weapon.Sword.enabled = true;
        yield return new WaitForSeconds(0.5f);
        weapon.Sword.enabled = false;
        weapon.isSwordSide = false;


        yield return new WaitForSeconds(0.5f);


       // lineRenderer.gameObject.SetActive(true);

        isAttacking = false;
        myanimater.SetBool(Id_Idle, true);
        myanimater.SetBool(Id_Attack, false);
    }

    private float CalCulateMaxTime(Vector3 targetPostion) {

        float distanceToTarget = Vector3.Distance(transform.position, targetPostion);

        if (distanceToTarget > maxDistanceToPlayer) {

            distanceToTarget = maxDistanceToPlayer;
        }

        return ((distanceToTarget * maxTimeToReachPlayer) / maxDistanceToPlayer);
    }

    private void orcKnockBackMotion() {


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }

        lineRenderer.SetPosition(0, transform.position);
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void SethitByAura(Vector3 direction, float flt_Force) {


        //if (coro_tagetmove != null) {
        //    isSetTarget = false;
        //    StopCoroutine(coro_tagetmove);

        //}
        enemyState = EnemyState.knockBack;
        OrcKnockBack(direction, flt_Force);
    }

    public void OrcKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

        //  ScaleAnimation();

        if (isAttacking) {
            return;
        }

        //if (!lineRenderer.gameObject.activeSelf) {
        //    lineRenderer.gameObject.SetActive(true);
        //}

        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (_KnockBackSpeed * persantageOfBlock*0.01f);
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

[System.Serializable]
public  struct FisherAnimationName {

    public string run;
    public string attack1;
    public string attack2;
    

}
