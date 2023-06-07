using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterPlantShootine : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private Animator enemy_Animator;
    [SerializeField]private EnemyState enemyState;
    [SerializeField] private PlantTrigger plantTrigger;

  
    [Header("Bullet Data")]
    [SerializeField] private GameObject obj_Bullet;
    [SerializeField] private Transform transform_Spawnpostion;
    [SerializeField] private float flt_BulletFireRate;
    [SerializeField] private float flt_BulletDamage;

  

    [SerializeField] private float flt_KnockbackForce;
    [SerializeField]private float flt_CurrentTime;
    [SerializeField] private float flt_offsetTargetpostion;
    [SerializeField] private int noofBullet;
    private float flt_ScaleAnimationTime = 0.2f;

    [SerializeField]private bool isvisible;
    private float flt_Reducescale = 0.3f;

   

    //[Header("Vfx")]
    ////[SerializeField] private ParticleSystem bullet_Muzzle;

    // Tag & Id
    private const string ID_Idle = "Idle";
    private const string Id_Attack = "Attack";

    // Coroutine
    private Coroutine coro_KnockBack;

    private Vector3 knockBackDirection;
    private bool isKnockBackStart = false;
    private float flt_KnockBackSpeed;
    private float flt_KnockBackTime = 0.2f;
    private bool isGrounded;
    private float currentAffectedGravityForce;
    private float gravityForce = -0.75f;


    private void OnEnable() {
        flt_BulletDamage = enemyData.GetDamage();
        flt_KnockbackForce = enemyData.GetKnockBackForce();
    }

    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
        enemyState = EnemyState.Idle;
    }
   
    private void OnCollisionExit(Collision collision) {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.Wave) {
            plantTrigger.StopHitTidalWave();
        }

        isGrounded = false;
        enemyState = EnemyState.Not_Ground;
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            enemy_Animator.SetTrigger(ID_Idle);
            return;
        }

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Not_Ground) {

            currentAffectedGravityForce = gravityForce;
            PlantKnockBackMotion();
        }
        else if (enemyState == EnemyState.knockBack) {
            PlantKnockBackMotion();
        }
        else if (enemyState == EnemyState.Idle) {

            if (isvisible) {
                FireBullet();
                LookAtPLayer();
            }
            
        }
       
      
       
    }

    public void SetInVisible() {
        isvisible = false;
    }

    public void SetVisible() {
        isvisible = true;
    }

    public void SetTidal(Transform transform) {
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);
    }
    public void SetBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);
    }

    private void LookAtPLayer() {

        Vector3 direction = (PlayerManager.instance.Player.transform.position - transform.position).normalized;
        float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }

    private void FireBullet() {

        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_BulletFireRate) {

            flt_CurrentTime = 0;

            StartCoroutine(SpawnBullet());
        }
    }

    private IEnumerator SpawnBullet() {

       
        for (int i = 0; i < noofBullet; i++) {
            enemy_Animator.SetTrigger(Id_Attack);
            enemy_Animator.SetTrigger(ID_Idle);
            GameObject currentBullet = Instantiate(obj_Bullet, transform_Spawnpostion.position,
                        transform_Spawnpostion.rotation);
            Vector3 playerPostion = PlayerManager.instance.Player.transform.position;
            Vector3 shootingPostion = new Vector3(Random.Range(playerPostion.x - flt_offsetTargetpostion,
                playerPostion.x + flt_offsetTargetpostion), playerPostion.y, Random.Range(playerPostion.z - flt_offsetTargetpostion,
                                playerPostion.z + flt_offsetTargetpostion));
          
            currentBullet.GetComponent<MonsterPlantBulletMotion>().SetBulletData(shootingPostion, flt_BulletDamage,
                flt_KnockbackForce);
            yield return new WaitForSeconds(0.5f);
        }

       
    }

    private void PlantKnockBackMotion() {

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

    public void PlantKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

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

        enemyState = EnemyState.Idle;
       

    }
}
