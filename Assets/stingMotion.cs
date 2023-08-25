using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stingMotion : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("Componant")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyData enemyData;


    [Header("Bat - data")]
    [SerializeField] private Vector3 targetedPostion;
    [SerializeField] private float flt_MovementSpeed = 1.5f;
    [SerializeField] private bool isVisible = true;

    [Header("Sting Shooting")]
    [SerializeField] private SinWaveBullet bullet; // object
    [SerializeField] private Transform transform_SpawnPostion; // spawn position
    [SerializeField] private ParticleSystem ps_Bullet_Muzzle; // bullet muzzle
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isAttackInMotion = false;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_currentTimePassedForFireRate = 0f;
    [SerializeField] private float flt_IdleTimeBeforeAttack = 0.5f;
    [SerializeField] private float flt_currentTimePassedForIdle = 0f;

    //Id
    private const string Id_Attack = "Attack";

    // KnockBackData
    private float persanatgeOfBlock = 0;
    private float flt_KnockBackTime = 0.5f;
    [SerializeField] private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;

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


    public void setInvisible() {
        isVisible = false;
    }


    public void SetVisible() {
        isVisible = true;
    }

    private void Update() {

        if (!GameManager.instance.isPlayerLive) {

            return;
        }

        BatShooting();
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

            BatNormalMotion();
        }
    }

    private void BatShooting() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }
        else if (!isVisible) {
            return;
        }
        else if(isAttackInMotion) {
            return;
        }

        flt_currentTimePassedForFireRate += Time.deltaTime;

        if (flt_currentTimePassedForFireRate >= flt_FireRate) {

            isAttacking = true;

            flt_currentTimePassedForIdle += Time.deltaTime;

            if (flt_currentTimePassedForIdle >= flt_IdleTimeBeforeAttack) {
                // attack
                isAttackInMotion = true;

                animator.SetBool(Id_Attack, true);
                StartCoroutine(WaitAndShootBullet());
            }
        }
    }

    private IEnumerator WaitAndShootBullet() {

        for (int i = 0; i < 3; i++) {

            if (i == 0) {
                yield return new WaitForSeconds(0.75f);
            }
            else {
                yield return new WaitForSeconds(0.34f);
            }

            transform_SpawnPostion.LookAt(GameManager.instance.Player.transform);
            SinWaveBullet current = Instantiate(bullet, transform_SpawnPostion.position,
               transform_SpawnPostion.rotation);

            ps_Bullet_Muzzle.Play();


            current.SetBulletData(transform_SpawnPostion.forward, flt_Damage, flt_Force);
        }

        yield return new WaitForSeconds(0.9f);

        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        isAttackInMotion = false;
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;

    }

    private void KnockBackMotion() {

        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);

    }

    public void BatKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

        //ScaleAnimation();
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (_KnockBackSpeed*0.01f*persanatgeOfBlock);
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
        isAttackInMotion = false;
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;
    }

    public void HitByTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        animator.SetBool(Id_Attack, false);
        isAttacking = false;
        isAttackInMotion = false;
        flt_currentTimePassedForFireRate = 0f;
        flt_currentTimePassedForIdle = 0f;
    }


    private void BatNormalMotion() {
      
        if (!isVisible) {
            return;
        }
        else if (isAttacking) {
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

        if (!isVisible) {
            return;
        }

        transform.LookAt(GameManager.instance.Player.transform);
    }
}