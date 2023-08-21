using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GolemMovement : MonoBehaviour
{

   
    [Header("Componant")]
    [SerializeField] private EnemySoundManager enemySound;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Animator enemy_Animator;
    [SerializeField] private EnemyState enemyState;
    [SerializeField] private GolemTrigger golemTrigger;
  

    [Header("Enemy - Data")]
    [SerializeField] private float flt_EnemyChargeTime; // flt_EnemyChargingTimeForJump
    [SerializeField] private Vector3 targetPostion;
    [SerializeField]private float flt_Currenttime; // flt_CurrentTimePassedForCharging

    [SerializeField] private float flt_Offset;
    [SerializeField] private MMF_Player jumpStart;
    [SerializeField] private MMF_Player jumpEnd;

   

    [SerializeField]private float flt_MaxJumpTime;
    [SerializeField]private float flt_JumpAccerletion; // jump acceleration
    [SerializeField]private float flt_KnockBackForce;
    [SerializeField]private float flt_MinKnockBackForce;
    [SerializeField]private float flt_MaxKnockBackForce;


    [SerializeField] private Vector3 castPosition;
    [SerializeField]private float flt_RangeOfSpheareCast;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    private float gravityForce = -0.75f;
    public bool isGrounded = true;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;

    public GameObject Obj_current_Target;
    [SerializeField]private bool isVisible = true;
    private bool isAttacking = false;
    private bool isAttackInMotion = false;
  
    [Header("KnockBackData")]
    [SerializeField] private Vector3 knockBackDirection;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private float perasantageOfBlock; // flt_KnockBackBlockPercentage

   

    private float flt_KnockBackTime = 0.5f;
   

    [Header("Vfx")]
    [SerializeField] private GameObject obj_tagret;
    [SerializeField] private GameObject jump_Start;
   

    //Coroutine
    private Coroutine cour_KnockBack;
    private Coroutine Cour_Jump;


    // tag & id
    private string tag_Player = "Player";
    [SerializeField]private LayerMask layerMask;
    private float flt_TidalDamage;
    private const string Id_Idle = "Idle";
    private const string Id_Jump = "Jump";
    private const string Id_Charge = "Charge";



    private void OnEnable() {
        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
    }


    private void Start() {
        flt_Damage = enemyData.GetDamage();
        flt_MaxKnockBackForce = enemyData.GetKnockBackForce();
        flt_MinKnockBackForce = 0;
       

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
            
            ChargeJump();
            FindTarget();
        }


        GolemStateMotion();
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
        if (isAttackInMotion) {
            return;
        }

        if (enemyState == EnemyState.Wave) {
            StartCoroutine(WaitHitByTidalWave());
        }
       

        isGrounded = false;
        currentGroundCheckTime = 0f;
        //enemyState = EnemyState.Not_Ground;
    }

    public void checktargetActive() {
        if (Obj_current_Target != null) {
            Destroy(Obj_current_Target);
        }
    }

    private IEnumerator WaitHitByTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            golemTrigger.StopHitTidalWave(flt_TidalDamage);
        }
     
    }

    public void CheckIfGrounded() {
        if (isGrounded) {
            enemyState = EnemyState.Idle;
        }
    }


    public void SetInVisible() {
        isVisible = false;
        flt_Currenttime = 0f;

        if (!isAttacking) {
            enemy_Animator.SetBool(Id_Idle, true);
            enemy_Animator.SetBool(Id_Charge, false);
            enemy_Animator.SetBool(Id_Jump, false);
        }
       

    }

    public void SetVisible() {

        isVisible = true;
        
    }

    private void GolemStateMotion() {

        if (enemyState == EnemyState.BlackHole) {
           
            return;
           
        }
        else if (enemyState == EnemyState.knockBack) {
            GolemKnockBackMotion();
        }
        else if (enemyState == EnemyState.Not_Ground) {
         
            GolemKnockBackMotion();
        }
    }

    public void SethitByAura(Vector3 direction, float flt_Force) {
        if (Obj_current_Target != null) {
            Destroy(Obj_current_Target);
        }
        if (Cour_Jump != null) {
          
            StopCoroutine(Cour_Jump);
            enemy_Animator.SetBool(Id_Idle, true);
            enemy_Animator.SetBool(Id_Charge, false);
            enemy_Animator.SetBool(Id_Jump, false);
        }
        enemyState = EnemyState.knockBack;
        GolemKnockBack(direction, flt_Force);
    }
    public void HitByTidal(Transform transform , float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);
        if (Obj_current_Target != null) {
            Destroy(Obj_current_Target);
        }

        StopAllCoroutines();
        enemy_Animator.SetBool(Id_Idle, true);
        enemy_Animator.SetBool(Id_Charge, false);
        enemy_Animator.SetBool(Id_Jump, false);
        isAttacking = false;
        isAttackInMotion = false;
        flt_Currenttime = 0f;

    }
    public void OrbitKnockBack(float flt_Force, Vector3 direction) {
       
       
        if (Obj_current_Target != null) {
            Destroy(Obj_current_Target);
        }
        if (Cour_Jump != null) {
    
            StopCoroutine(Cour_Jump);
            enemy_Animator.SetTrigger(Id_Idle);
        }
        GolemKnockBack(direction, flt_Force);
    }

    public void HitByBlackHole(Transform _Target) {

        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);
        if (Obj_current_Target != null) {
            Destroy(Obj_current_Target);
        }

        StopAllCoroutines();
        enemy_Animator.SetBool(Id_Idle, true);
        enemy_Animator.SetBool(Id_Charge, false);
        enemy_Animator.SetBool(Id_Jump, false);
        isAttacking = false;
        isAttackInMotion = false;
        flt_Currenttime = 0f;
    }
   

    //private void GolemMotion() {

    //    if (!isGetDirection) {

    //        Transform player = GameManager.instance.Player.transform;
        
    //        Vector3 direction = (player.transform.position - transform.position).normalized;
    //        Vector3 endPosition = player.transform.position - direction * 2f;
         
    //        targetPostion = endPosition;
           
    //        Vector3 postion = new Vector3(player.position.x, -0.95f
    //            , player.position.z);
    //        castPosition = postion;
    //        Obj_current_Target =  Instantiate(obj_tagret, postion, obj_tagret.transform.rotation);
    //        Cour_Jump = StartCoroutine(GolemJump());
    //        isGetDirection = true;

    //    }      
    //}

  
    private void FindTarget() {
        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if(isAttacking) {
            return;
        }

        float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, GameManager.instance.Player.transform.
            position));
        if (flt_Distance<0.5f) {
            return;
        }

        Vector3 targetDirection = (new Vector3(GameManager.instance.Player.transform.position.x, 0,
                                            GameManager.instance.Player.transform.position.z) - transform.position).normalized;
        //transform.LookAt(targetDirection);

       
       float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion Qua_Target = Quaternion.Euler(0, targetAngle, 0);
        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, Qua_Target, 10 * Time.deltaTime);


    }

    private void SetTargetLocation() {
        Transform player = GameManager.instance.Player.transform;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 endPosition = player.transform.position - direction * 2f;

        targetPostion = endPosition;

        Vector3 postion = new Vector3(player.position.x, -0.95f
            , player.position.z);
        castPosition = postion;
        Obj_current_Target = Instantiate(obj_tagret, postion, obj_tagret.transform.rotation);
        Cour_Jump = StartCoroutine(JumpTowardsPlayer());
    }


    private IEnumerator JumpTowardsPlayer() {

        enemy_Animator.SetBool(Id_Idle, false);
        enemy_Animator.SetBool(Id_Charge, true);
        //enemy_Animator.SetBool(Id_Jump, false);
        Debug.Log("GolemJump");
        yield return new WaitForSeconds(1);

        isAttackInMotion = true;

        jumpStart.PlayFeedbacks();
        yield return new WaitForSeconds(0.25f);    
        enemy_Animator.SetBool(Id_Charge, false);
        enemy_Animator.SetBool(Id_Jump, true);
        Debug.Log("Trigger JUmp");
        Instantiate(jump_Start, transform.position, transform.rotation);
        enemySound.Play_Attack();
        Vector3 startPostion = transform.position;
        float jumpheight = flt_JumpAccerletion * flt_MaxJumpTime / (MathF.Sqrt(2 * Physics.gravity.magnitude));

       
        // Keep track of how much time has passed since the start of the jump
        float elapsedTime = 0f;
     
        while (elapsedTime < 1) {
           
            elapsedTime += Time.deltaTime/flt_MaxJumpTime;
            float height = Mathf.Sin(elapsedTime * Mathf.PI) * jumpheight;
           
           
            transform.position = Vector3.Lerp(startPostion, targetPostion, elapsedTime) + Vector3.up * height;

           
            yield return null;
          
        }



        transform.position = targetPostion;
       
        Sphercast();
        enemySound.Play_Attack();
        Destroy(Obj_current_Target);
        yield return new WaitForSeconds(1);

        enemy_Animator.SetBool(Id_Idle, true);
        enemy_Animator.SetBool(Id_Jump, false);

        isAttacking = false;
        isAttackInMotion = false;

    }

  
    private void Sphercast() {
        Collider[] all_Collider = Physics.OverlapSphere(castPosition, flt_RangeOfSpheareCast,layerMask);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].gameObject.CompareTag(tag_Player)) {

                if (all_Collider[i].TryGetComponent<CollisionHandling>(out CollisionHandling collision)) {

                    float distance = Mathf.Abs(Vector3.Distance(new Vector3(castPosition.x, 0, castPosition.z),
                                   new Vector3(all_Collider[i].transform.position.x, 0, all_Collider[i].transform.position.z)));
                    Debug.Log(distance + "Distance");
                    flt_KnockBackForce = ((flt_MinKnockBackForce - flt_MaxKnockBackForce) / flt_RangeOfSpheareCast) * distance +
                        flt_MaxKnockBackForce;
                    Vector3 knockBackDirection = (-new Vector3(castPosition.x, 0, castPosition.z) +
                           new Vector3(all_Collider[i].transform.position.x, 0, all_Collider[i].transform.position.z)).normalized;


                    if (distance <= 0.5f) {
                        Vector3 randomDirection = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)).normalized;
                        knockBackDirection = randomDirection;
                        Debug.Log("Random" + randomDirection);
                        flt_KnockBackForce = flt_MaxKnockBackForce;
                    }

                    Debug.Log("flt_KnockBackForce" +  flt_KnockBackForce);
                    Debug.Log("flt_Direction" + knockBackDirection);
                    collision.SetHitByNormalBullet(flt_Damage,flt_KnockBackForce, knockBackDirection);
                }
            }
        }
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(castPosition, flt_RangeOfSpheareCast);
    }

    private void GolemKnockBackMotion() {
       

        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }

        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime,Space.World);
        
    }

    private void ChargeJump() {

        if(enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (isAttacking) {
            return;
        }
      
        flt_Currenttime += Time.deltaTime;
        if (flt_Currenttime>flt_EnemyChargeTime) {
          
            flt_Currenttime = 0;
            // enemyState = EnemyState.Run;
            isAttacking = true;
            SetTargetLocation();
        }
    }

   

    public void GolemKnockBack(Vector3 _knockBackDirection ,float _flt_Force) {

        if (isAttackInMotion) {
            return;
        }

       
        //ScaleAnimation();
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _flt_Force - (_flt_Force * perasantageOfBlock / 100);
        knockBackDirection = _knockBackDirection;
       
        if (cour_KnockBack != null) {
            StopCoroutine(cour_KnockBack);
        }

        cour_KnockBack = StartCoroutine(StopKnockbackOverTime());      
    }

    public void HitByAura(Vector3 _knockBackDirection, float _flt_Force) {

        if (enemyState == EnemyState.Run) {

            // Reset run data
        }

        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _flt_Force - (_flt_Force * perasantageOfBlock / 100);
        knockBackDirection = _knockBackDirection;

        if (cour_KnockBack != null) {
            StopCoroutine(cour_KnockBack);
        }

        cour_KnockBack = StartCoroutine(StopKnockbackOverTime());
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

    }

   
}
