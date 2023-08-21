using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CobraMotion : MonoBehaviour
{
    [SerializeField] private CobraTrigger cobraTrigger;
    [SerializeField] private LineRenderer line;
    
    [SerializeField] private FisherAnimationName myAnimation;
    [SerializeField] private EnemyState enemyState;
    private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;
    [SerializeField] private Animator animator;
    [SerializeField] private float flt_ChargingTime;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_CurrentTimeForFireRate;
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private float flt_RotationSpeed;
    [SerializeField] private float flt_CurrentChargeTime;
    [SerializeField] private Vector3 targetPostion;
    private float flt_CurrentAngle;
    private float flt_TargetAngle;
    [SerializeField] private bool isGrounded;
    private float flt_KnockBackTime = 0.5f;
    [SerializeField]private bool isVisible;
    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;
    [SerializeField] private float flt_RunningOffset;
    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;

    [Header("Laser")]
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool shouldRunLaser = false;
    [SerializeField] private bool hasAlreadyDoneDamage = false;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Transform laserPostion;
    [SerializeField] private GameObject startLaser;
    [SerializeField] private LineRenderer laser_Line;
    [SerializeField] private GameObject laserExpltion;
    [SerializeField] private LayerMask player_Layer;

    public Transform rayPoint;

    private bool isHitByBlackhole = false;
    
    private Coroutine attack;
   
    [SerializeField]private float laserRange;
    [SerializeField] private float flt_CurrentRange;
    private Coroutine coro_KnockBack;
    private float flt_MaxTime = 2;
    private float flt_TidalDamage;

    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
        ChargeCallBack();
    } 

    private void Update() {

        if (!isGrounded) {
            currentGroundCheckTime += Time.deltaTime;
            if (currentGroundCheckTime >= groundCheckBufferTime) {
                enemyState = EnemyState.Not_Ground;
            }
        }
        FisherMovement();
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
        if (enemyState == EnemyState.Run) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            Debug.Log("EXIT");
            StartCoroutine(WaitAndLeaveTidalWave());
            
        }
        isGrounded = false;
        currentGroundCheckTime = 0f;
       // enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitAndLeaveTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            cobraTrigger.StopHitTidalWave(flt_TidalDamage);
        }
    }

    public void CheckIfGrounded() {

        if (isGrounded) {

            MoveCallBack();
            isHitByBlackhole = false;
        }
    }

    public void SetInVisible() {
        isVisible = false;
        line.gameObject.SetActive(false);

        //if(enemyState == EnemyState.Run) {

        //    animator.SetBool(myAnimation.run, false);
        //    animator.SetBool(myAnimation.attack1, false);
        //    enemyState = EnemyState.Idle;
        //}

        if (!isAttacking) {

            animator.SetBool(myAnimation.run, false);
            animator.SetBool(myAnimation.attack1, false);
            enemyState = EnemyState.Idle;
        }

        //startLaser.gameObject.SetActive(false);
        //laser_Line.gameObject.SetActive(false);
        //laserExpltion.gameObject.SetActive(false);
        //StopAllCoroutines();

    }

    public void SetVisible() {
        isVisible = true; 
        flt_CurrentChargeTime = 0f;

        if(enemyState == EnemyState.knockBack) {
            return;
        }

        enemyState = EnemyState.Run;
        if (!isAttacking) {
            animator.SetBool(myAnimation.run, true);
        }
        else {
            MoveCallBack();
        }
        //line.gameObject.SetActive(true);
    }

    public void SethitByTidal(Transform transform , float flt_Damage) {

        flt_TidalDamage = flt_Damage;
        enemyState = EnemyState.Wave;
        line.gameObject.SetActive(false);
        this.transform.SetParent(transform);
 

        animator.SetBool(myAnimation.run, false);
        animator.SetBool(myAnimation.attack1, false);

        StopAttackingIfEnemyIsAttacking();
        StopAllCoroutines();;
    }

  
    public void orbitKnockBack(float flt_Force, Vector3 direction) { 
        OrcKnockBack(direction, flt_Force);
    }

    public void HitByBlackHole(Transform _Target) {

        enemyState = EnemyState.BlackHole;

        isHitByBlackhole = true;

        animator.SetBool(myAnimation.run, false);
        animator.SetBool(myAnimation.attack1, false);

        StopAttackingIfEnemyIsAttacking();   
        transform.SetParent(_Target);
        StopAllCoroutines();

    }

    private void StopAttackingIfEnemyIsAttacking() {

        line.gameObject.SetActive(false);
        startLaser.gameObject.SetActive(false);
        laser_Line.gameObject.SetActive(false);
        laserExpltion.gameObject.SetActive(false);

        isAttacking = false;
        shouldRunLaser = false;
        flt_CurrentTimeForFireRate = 0;
        flt_CurrentChargeTime = 0f;
    }

    private void FisherMovement() {

       
        ChargeAttack();
        ShootLaserTowardsPlayer();
        //ShootLaserTowardsPlayer();

        switch (enemyState) {

          
            case EnemyState.Run:
                MovePlayer();
                break;
            case EnemyState.knockBack:
              
                orcKnockBackMotion();
                break;
            case EnemyState.Not_Ground:
                currentAffectedGravityForce = gravityForce;
                orcKnockBackMotion();
                break;
            case EnemyState.BlackHole:

                break;
            case EnemyState.Wave:
                break;
            
            default:
                break;
        }
    }

   



    private void ChargeCallBack() {

        animator.SetBool(myAnimation.run, false);
        flt_CurrentChargeTime = 0;
        line.gameObject.SetActive(true);
        flt_CurrentTimeForFireRate = flt_FireRate;
    }

    private void LaserShootingMechanics() {

        
        animator.SetBool(myAnimation.run, false);
        if (attack == null) {
        
            flt_CurrentRange = laserRange;
            StartCoroutine(WaitAndShootLaser());

        }

    }

    private IEnumerator WaitAndShootLaser() {

        animator.SetBool(myAnimation.attack1, true);
       
        yield return new WaitForSeconds(0.5f);
        shouldRunLaser = true;
        hasAlreadyDoneDamage = false;
        startLaser.gameObject.SetActive(true);
        laser_Line.gameObject.SetActive(true);
        
        laserExpltion.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);

        shouldRunLaser = false;
        startLaser.gameObject.SetActive(false);
        laser_Line.gameObject.SetActive(false);
        laserExpltion.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        attack = null;

        isAttacking = false;
        flt_CurrentTimeForFireRate = 0f;

        laser_Line.SetPosition(0, Vector3.zero);
        laser_Line.SetPosition(1, Vector3.zero);

        animator.SetBool(myAnimation.attack1, false);

        if (!GameManager.instance.IsInVisblePowerUpActive) {
            MoveCallBack();
        }
      
    }
    private void ChargeAttack() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        // Charge code

        if (isAttacking) {
            return;
        }

        flt_CurrentTimeForFireRate += Time.deltaTime;
        if(flt_CurrentTimeForFireRate >= flt_FireRate) {

            if (!line.gameObject.activeSelf) {
                animator.SetBool(myAnimation.run, false);
                line.gameObject.SetActive(true);
            }
           

            flt_CurrentChargeTime += Time.deltaTime;
            line.SetPosition(0, new Vector3(transform.position.x, 0, transform.position.z));
            line.SetPosition(1, new Vector3(GameManager.instance.Player.transform.position.x, 0, GameManager.instance.Player.transform.position.z));
            RotateAsPerTarget();

            if (flt_CurrentChargeTime > flt_ChargingTime) {

                flt_CurrentChargeTime = 0f;
                line.gameObject.SetActive(false);
                isAttacking = true;
                LaserShootingMechanics();             
            }
        }
    }

    private void ShootLaserTowardsPlayer() {

        if (!shouldRunLaser) {
            return;
        }

        startLaser.transform.position = laserPostion.position;
        startLaser.transform.rotation = laserPostion.rotation;

        Vector3 startPosition = laserPostion.position + rayPoint.forward;

        laser_Line.SetPosition(0, startPosition);
        laserExpltion.transform.position = laserPostion.forward * flt_CurrentRange;

        Vector3 endPosition = laserPostion.position + rayPoint.forward * flt_CurrentRange;

        laser_Line.SetPosition(1, endPosition);
        laserExpltion.transform.position = laserPostion.position + rayPoint.forward * flt_CurrentRange;


        Vector3 rayPos = laserPostion.position;
        rayPos.y = 0;
        rayPos += rayPoint.forward;



        if (!hasAlreadyDoneDamage) {

            Debug.Log("has already done damage");

            if (Physics.Raycast(rayPos, rayPoint.forward, out RaycastHit hit, flt_CurrentRange)) {

                flt_CurrentRange = MathF.Abs(Vector3.Distance(laserPostion.position, hit.point));
                if (hit.collider.TryGetComponent<CollisionHandling>(out CollisionHandling collision)) {

                    hasAlreadyDoneDamage = true;
                    collision.SetHitByNormalBullet(enemyData.GetDamage(), enemyData.GetKnockBackForce(), laserPostion.forward.normalized);
                }
            }
        }
       
    }


    private void MoveCallBack() {

        enemyState = EnemyState.Run;
        animator.SetBool(myAnimation.run, true);

        FindRandomPosition();
    }

    private void MovePlayer() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        if (flt_CurrentTimeForFireRate >= flt_FireRate) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPostion, flt_MovementSpeed * Time.deltaTime);
        RotateAsPerTarget();
        float flt_Distance = MathF.Abs(Vector3.Distance(transform.position, targetPostion));

        if(flt_Distance <= 1f) {
            FindRandomPosition();
        }
        
    }

    private void FindRandomPosition() {
       
        Vector3 RandomPOstion = LevelManager.instance.GetRandomPostion(transform.position.y);
        targetPostion = RandomPOstion;
    }
   

    private void RotateAsPerTarget() {

        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        flt_TargetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        flt_CurrentAngle = Mathf.LerpAngle(flt_CurrentAngle, flt_TargetAngle, flt_RotationSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, flt_CurrentAngle, 0);
    }

    private void orcKnockBackMotion() {


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }

        line.SetPosition(0, transform.position);
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void SethitByAura(Vector3 direction, float flt_Force) {

        enemyState = EnemyState.knockBack;
        OrcKnockBack(direction, flt_Force);
    }

    public void OrcKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {


      
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

        if (isHitByBlackhole) {
            MoveCallBack();
            isHitByBlackhole = false;
        }
    }

}
