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
    [SerializeField] private GameObject obj_Muzzle;
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

    private float groundCheckBufferTime = 0.2f;
    private float currentGroundCheckTime = 0f;



    // Tag & Id
    private const string ID_Idle = "Idle";
    private const string Id_Attack = "Attack";

    // Coroutine
    private Coroutine coro_KnockBack;

    private float persantage_OfBlock = 0;
    private Vector3 knockBackDirection;
    private float flt_KnockBackSpeed;
    private float flt_KnockBackTime = 0.2f;
    private bool isGrounded;
    private float gravityForce = -0.75f;

    private bool isBulletSpawn;
    private float flt_TidalDamage;

    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            SetInVisible();
        }
        else {
            SetVisible();
        }
        flt_BulletDamage = enemyData.GetDamage();
        flt_KnockbackForce = enemyData.GetKnockBackForce();
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
            //plantTrigger.StopHitTidalWave();
            StartCoroutine(WaitHitByTidalWave());
        }

        isGrounded = false;
        currentGroundCheckTime = 0;
        //enemyState = EnemyState.Not_Ground;
    }

    private IEnumerator WaitHitByTidalWave() {
        yield return new WaitForSeconds(0.3f);
        if (!isGrounded) {
            plantTrigger.StopHitTidalWave(flt_TidalDamage);
        }
    }

    public void CheckIfGrounded() {
        if (isGrounded) {
            enemyState = EnemyState.Idle;
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

        if (isvisible) {
            FireBullet();
            LookAtPLayer();
        }

        if (enemyState == EnemyState.Not_Ground) {

            PlantKnockBackMotion();
        }
        else if (enemyState == EnemyState.knockBack) {
            PlantKnockBackMotion();
        }

    }

    public void SetInVisible() {
        isvisible = false;
    }

    public void SetVisible() {
        isvisible = true;
    }

    public void SetTidal(Transform transform , float Damage) {

        flt_TidalDamage = Damage;
        enemyState = EnemyState.Wave;
        this.transform.SetParent(transform);

        StopAllCoroutines();
        isBulletSpawn = false;
        flt_CurrentTime = 0f;
        enemy_Animator.SetTrigger(ID_Idle);
    }

    public void SetBlackHole(Transform _Target) {
        enemyState = EnemyState.BlackHole;
        transform.SetParent(_Target);

        StopAllCoroutines();
        isBulletSpawn = false;
        flt_CurrentTime = 0f;
        enemy_Animator.SetTrigger(ID_Idle);
    }

    private void LookAtPLayer() {

        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, targetAngle, 0);
    }

    private void FireBullet() {

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        else if (enemyState == EnemyState.Wave) {
            return;
        }

        if (isBulletSpawn) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_BulletFireRate) {

            isBulletSpawn = true;
            flt_CurrentTime = 0;

            StartCoroutine(SpawnBullet());
        }
    }

    private IEnumerator SpawnBullet() {


        for (int i = 0; i < noofBullet; i++) {

            enemy_Animator.SetTrigger(Id_Attack);

            yield return new WaitForSeconds(0.1f * 0.833f);


            GameObject currentBullet = Instantiate(obj_Bullet, transform_Spawnpostion.position,
                        transform_Spawnpostion.rotation, GameManager.instance.enemySpanwble);

            Instantiate(obj_Muzzle, transform_Spawnpostion.position, transform_Spawnpostion.rotation);
            Vector3 playerPostion = GameManager.instance.Player.transform.position;
            Vector3 shootingPostion = new Vector3(Random.Range(playerPostion.x - flt_offsetTargetpostion,
                playerPostion.x + flt_offsetTargetpostion), -0.95f, Random.Range(playerPostion.z - flt_offsetTargetpostion,
                                playerPostion.z + flt_offsetTargetpostion));
          
            currentBullet.GetComponent<MonsterPlantBulletMotion>().SetBulletData(shootingPostion, flt_BulletDamage,
                flt_KnockbackForce);
            yield return new WaitForSeconds( 0.75f);

            enemy_Animator.SetTrigger(ID_Idle);


        }

      
        isBulletSpawn = false;
        flt_CurrentTime = 0;
       

    }

    private void PlantKnockBackMotion() {

        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }
        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void PlantKnockBack(Vector3 _KnockBackDirection, float _KnockBackSpeed) {

      
        enemyState = EnemyState.knockBack;
        flt_KnockBackSpeed = _KnockBackSpeed - (flt_KnockBackSpeed*0.01f*persantage_OfBlock);
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

        enemyState = EnemyState.Idle;
       

    }
}
