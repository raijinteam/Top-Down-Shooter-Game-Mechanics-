using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CyclopsMotion : MonoBehaviour
{
    [SerializeField] private cyclopsTrigger cyclopsTrigger;
    [SerializeField] private Animator myanimator;
    [SerializeField] private cyclopsAnimationName animationName;
    [SerializeField]private EnemyState enemyState;
   

    [SerializeField] private BigArrow smallArrow;
    [SerializeField] private BigArrow bigArrow;
    [SerializeField] private Transform bigArrowPostion;
    [SerializeField] private Transform[] all_SmallArrowPostion;


    [SerializeField]private Vector3 targetDirection;
    [SerializeField]private float flt_MovementSpeed;
    [SerializeField] private float flt_RotationSpeed;
    [SerializeField]private float flt_Damage;
    [SerializeField]private float flt_Force;
    [SerializeField] private float flt_FirerateTime;
   

    private Coroutine cour_Attack2;
    private Coroutine cour_Attack1;
    private Coroutine coro_KnockBack;
    private float flt_KnockBackSpeed;
    [SerializeField] private Vector3 knockBackDirection;
    [SerializeField] private bool isGrounded;
    private float flt_KnockBackTime = 0.5f;
    [SerializeField]private float perasantageOfBlock = 0;
    [SerializeField]private bool isVisible = true;
   
    private float gravityForce = -0.75f;
    private float targetAngle = 0;
    private float currentAngle = 0;

    private bool isHitByBlackhole = false;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;

    [Header("Cyclops Attack")]
    [SerializeField] private float flt_FireRate;
    private float flt_CurrentTimePassedForFireRate = 0f;
    private bool isAttacking = false;
    private float flt_TidalDamage;
    

    private void OnCollisionEnter(Collision collision) {

        isGrounded = true;
        //if (!isLanded) {
        //    //MoveCallBack();
        //    isLanded = true;
        //}
    }

    private void OnCollisionStay(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        if (enemyState == EnemyState.BlackHole) {
            isGrounded = false;
            return;
        }
        if (enemyState == EnemyState.Wave) {
            StartCoroutine(WaitHitByTidalWave());
          
        }

        isGrounded = false;

        currentGroundCheckTime = 0f;
       // enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitHitByTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            cyclopsTrigger.StopHitTidalWave(flt_TidalDamage);
        }
        
        //CheckIfGrounded();
    }

    public void CheckIfGrounded() {

        if (isGrounded) {
            myanimator.SetBool(animationName.idle, false);
            MoveCallBack();
           
            enemyState = EnemyState.Run;
        }
    }

    public void SetInVisible() {

        isVisible = false;

        if (!isAttacking) {
            myanimator.SetBool(animationName.idle, true);
            myanimator.SetBool(animationName.run, false);
        }
       

       // StopAllCoroutines();
      
    }

    public void SetVisible() {

        isVisible = true;
        

        if (enemyState == EnemyState.knockBack) {
            return;
        }

        enemyState = EnemyState.Run;
        myanimator.SetBool(animationName.run, true);
        myanimator.SetBool(animationName.idle, false);
        MoveCallBack();
        
    }

    public void HitByTidal(Transform transform , float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        myanimator.SetBool(animationName.idle, true);
        myanimator.SetBool(animationName.run, false);
        myanimator.SetBool(animationName.attack1, false);
        myanimator.SetBool(animationName.attack2, false);
        isAttacking = false;
        flt_CurrentTimePassedForFireRate = 0f;
    }


    public void HitByBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

        StopAllCoroutines();
        myanimator.SetBool(animationName.idle, true);
        myanimator.SetBool(animationName.run, false);
        myanimator.SetBool(animationName.attack1, false);
        myanimator.SetBool(animationName.attack2, false);
        isAttacking = false;
        flt_CurrentTimePassedForFireRate = 0f;

        isHitByBlackhole = true;
    }

    private void OnEnable() {
        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
       
      
    }

    private void Start() {

        myanimator.SetBool(animationName.idle, false);
        MoveCallBack();
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

        CyclopsMovement();
    }

    private void CyclopsMovement() {


        if (isVisible) {

            LookTowardsPlayer();
            AttackHandler();
        }
       
        
        
        switch (enemyState) {

            case EnemyState.Run:
                MoveMotion();
                break;
            case EnemyState.knockBack:
                KnockBackMotion();
                break;
            case EnemyState.Not_Ground:
               
                KnockBackMotion();
                break;

        }

   
    }

    private void AttackHandler() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }

        if (isAttacking) {
          
            return;
        }

        flt_CurrentTimePassedForFireRate += Time.deltaTime;
    

        if(flt_CurrentTimePassedForFireRate >= flt_FireRate) {

            flt_CurrentTimePassedForFireRate = 0f;
            isAttacking = true;
            ChooseRandomAttack();
        }

    }
    private void KnockBackMotion() {


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }
      
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    private void ChooseRandomAttack() {

        int index = Random.Range(0, 100);
        if (index < 50) {
            if (cour_Attack2 != null) {
                StopCoroutine(cour_Attack2);
            }
            cour_Attack2 = StartCoroutine(Attack2Spawn());
        }
        else {
            if (cour_Attack1 != null) {
                StopCoroutine(cour_Attack1);
            }
            cour_Attack1 = StartCoroutine(Attack1Spawn());
        }
    }


    private IEnumerator Attack1Spawn() {
        
        transform.LookAt(GameManager.instance.Player.transform);
            
        myanimator.SetBool(animationName.attack1, true);
        myanimator.SetBool(animationName.run, false);
      

        for (int i = 0; i < all_SmallArrowPostion.Length; i++) {

            if (i==0) {
                yield return new WaitForSeconds(0.66f);
            }
            else {
                yield return new WaitForSeconds(0.26f);
            }

            if (!GameManager.instance.IsInVisblePowerUpActive) {
                all_SmallArrowPostion[i].LookAt(GameManager.instance.Player.transform.position);
            }
            BigArrow current = Instantiate(smallArrow, all_SmallArrowPostion[i].position, all_SmallArrowPostion[i].rotation);

           
            current.SetArrowData(flt_Damage, flt_Force, all_SmallArrowPostion[i].forward);
        }

        yield return new WaitForSeconds(0.7f);
        cour_Attack1 = null;
        
        isAttacking = false;
        MoveCallBack();
    }

    private IEnumerator Attack2Spawn() {
        
     
        transform.LookAt(GameManager.instance.Player.transform);
        
        
        myanimator.SetBool(animationName.attack2, true);
        myanimator.SetBool(animationName.run, false);
        yield return new WaitForSeconds(1.3667f);

        if (!GameManager.instance.IsInVisblePowerUpActive) {
            bigArrowPostion.LookAt(GameManager.instance.Player.transform.position);
        }

        BigArrow current = Instantiate(bigArrow, bigArrowPostion.position, bigArrowPostion.rotation);

       
        current.SetArrowData(flt_Damage, flt_Force, bigArrowPostion.forward);

        yield return new WaitForSeconds(0.7f);
        cour_Attack2 = null;

        isAttacking = false;

       
        MoveCallBack();

    }

   

    private void MoveCallBack() {

        if (GameManager.instance.IsInVisblePowerUpActive) {

            myanimator.SetBool(animationName.idle, true);
            myanimator.SetBool(animationName.attack1, false);
            myanimator.SetBool(animationName.attack2, false);
            return;
        }

        myanimator.SetBool(animationName.run, true);
        myanimator.SetBool(animationName.attack1, false);
        myanimator.SetBool(animationName.attack2, false);

        targetDirection = LevelManager.instance.GetRandomPostion(transform.position.y);
        enemyState = EnemyState.Run;
     
    }
    private void MoveMotion() {
        Debug.Log("MoveMotion");
        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        if (isAttacking) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetDirection, flt_MovementSpeed * Time.deltaTime);

        float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, targetDirection));

        if (flt_Distance < 0.2f) {

            targetDirection = LevelManager.instance.GetRandomPostion(transform.position.y);

        }
    }

    public void Knockback(Vector3 _KnockBackDirection, float _KnockBackSpeed) {
        //ScaleAnimation();
        Debug.Log("CyclopsKNoackBack");
        enemyState  = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (_KnockBackSpeed * perasantageOfBlock / 100);
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

        if (isHitByBlackhole) {

            myanimator.SetBool(animationName.idle, false);
            MoveCallBack();
            isHitByBlackhole = false;
        }

        enemyState = EnemyState.Run;

        if (!isAttacking) {
            MoveCallBack();
        }
      
    }

    private void LookTowardsPlayer() {
        Debug.Log("LookTowardsPlayer");


        Vector3 direction = (-transform.position + GameManager.instance.Player.transform.position).normalized;
         targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, flt_RotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0,currentAngle,0);
         


    }
}



[System.Serializable]
public struct cyclopsAnimationName {
    public string idle;
    public string run;
    public string attack1;
    public string attack2;
}
