using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHandler : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("Componant")]
   [SerializeField] private Animator animator;
    [SerializeField] private EnemyData enemyData;


    [Header("Bat - data")]
    [SerializeField] private Vector3 targetedPostion;
    [SerializeField] private float flt_MovementSpeed = 1.5f;

    [Header("Bat Shooting")]
    [SerializeField] private EnemyNormalBullet bullet; // object
    [SerializeField] private Transform transform_SpawnPostion; // spawn position
    [SerializeField] private ParticleSystem ps_Bullet_Muzzle; // bullet muzzle
    [SerializeField] private Transform transform_Direction;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_currentTimePassedForFireRate = 0f;
   
    //Id
   
    private const string Id_Attack = "Attack";

    // KnockBackData
    [SerializeField]private float persantageofBlock = 0;
    private float flt_KnockBackTime = 0.5f;
    [SerializeField] private float flt_KnockBackSpeed;
    [SerializeField] private Vector3 knockBackDirection;

    // Courotine
    private Coroutine coro_KnockBack;
   

    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            setInvisible();
        }
        else {
            SetVisible();
        }
        targetedPostion = transform.position;
    }

    private void Update() {

        if (!GameManager.instance.isPlayerLive) {

            return;
        }

        DragonShooting();
        LookAtPlayer();

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (enemyState == EnemyState.knockBack) {

            KnockBackMotion();

        }
        else if (enemyState == EnemyState.Run) {

            MoveDragon();
        }
    }

    private void DragonShooting() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        if (isAttacking) {
            return;
        }

        flt_currentTimePassedForFireRate += Time.deltaTime;

        if (flt_currentTimePassedForFireRate >= flt_FireRate) {

            flt_currentTimePassedForFireRate = 0f;
            isAttacking = true;
            animator.SetBool(Id_Attack, true);
            StartCoroutine(WaitAndShootBullet());
                   
        }
    }

    private IEnumerator WaitAndShootBullet() {

        transform_Direction.position = transform_SpawnPostion.position;
        yield return new WaitForSeconds(1f);

        float rotationValue = 30f;

        for (int i = 0; i < 3; i++) {

           

            EnemyNormalBullet current = Instantiate(bullet, transform_SpawnPostion.position,
            Quaternion.identity, GameManager.instance.enemySpanwble);


            ps_Bullet_Muzzle.Play();



            Vector3 direction = Vector3.zero;
            direction = Quaternion.Euler(0, rotationValue, 0) * transform_Direction.forward;

            current.SetBulletData(direction, flt_Damage, flt_Force);
            rotationValue -= 30;

            //if (i == 0) {

            //    direction = Quaternion.Euler(0, rotationValue, 0) * transform_Direction.forward;
            //}
            //else if (i == 1) {
            //    direction = transform.forward;
            //}
            //else if (i == 2) {

            //    direction = Quaternion.Euler(0, -30, 0) * transform_Direction.forward;
            //}


            if (i == 0) {

                yield return new WaitForSeconds(0.6667f);
            }
            else if (i == 1) {
                yield return new WaitForSeconds(0.5f);
            }
         
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        animator.SetBool(Id_Attack, false);
    }

    internal void setInvisible() {
        
    }

    internal void SetVisible() {
       
    }

    private void KnockBackMotion() {

        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);

    }

    public void BatKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

        //ScaleAnimation();
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (_KnockBackSpeed * persantageofBlock*0.01f);
        Debug.Log("Perameter" + _KnockBackSpeed);
        Debug.Log("Varible" + flt_KnockBackSpeed);
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

        enemyState = EnemyState.Run;

    }


    public void HitByBlackHole(Transform _target) {

        enemyState = EnemyState.BlackHole;
        transform.SetParent(_target);

        StopAllCoroutines();
        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        flt_currentTimePassedForFireRate = 0f;
    
    }

    public void HitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        flt_currentTimePassedForFireRate = 0f;
    }

    public void StopTidalEffect() {
        enemyState = EnemyState.Run;
    }


    private void MoveDragon() {

        if (isAttacking) {
            return;
        }

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetedPostion, flt_MovementSpeed
            * Time.deltaTime);
        float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, targetedPostion));

        if (flt_Distance < 0.2f) {

            targetedPostion = LevelManager.instance.GetRandomPostion(transform.position.y);

        }
    }

    private void LookAtPlayer() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            return;
        }

        transform.LookAt(GameManager.instance.Player.transform);
    }
}
