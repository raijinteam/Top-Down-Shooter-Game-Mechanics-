using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public  class BossMovement : MonoBehaviour {

    public BossAnimationName bossAnimationName;
    [SerializeField] private BossState myBoss = new BossState();
  
    public Animator boss_Animator;
  
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_FireRate;
    public bool isAttack2Work;
    private bool isRotate;
    public delegate void StopRotate(bool isRotate);
    public StopRotate SetRotate;
    public Action IdleCallBack;
    public Action AttackCallBack;
    public Action Attack1CallBack;
    public Action Attack2CallBack;
    public float force;
    public float damage;

    private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;
    private Quaternion KnockBackRotation;
    private bool isKnockBackStart;
    [SerializeField] private float perasantageOfBlock;
    private Coroutine cour_KncokBack;
    private bool isGrounded = true;
    private float currentAffectedGravityForce = 1;
    private float gravityForce = -0.75f;
    private float flt_KnockBackTime = 0.5f;


    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
        myBoss = BossState.idle;
    }

    private void OnCollisionExit(Collision collision) {

      
        isGrounded = false;
        myBoss = BossState.Not_Ground;
    }
    private void OnEnable() {
        myBoss = BossState.idle;
        boss_Animator.SetBool(bossAnimationName.joom, true);
        this.isRotate = true;
        SetRotate += SetRotation;
        IdleCallBack += IdleCallback;
        AttackCallBack += AttackCallback;
    }

    private void SetRotation(bool isRotate) {

        this.isRotate = isRotate;
    }

    private void OnDisable() {
        IdleCallBack -= IdleCallback;
        AttackCallBack -= AttackCallback;
        SetRotate -= SetRotation;
    }

    private void AttackCallback() {

        boss_Animator.SetFloat(bossAnimationName.IdleBattel, 1, 0.01f, Time.deltaTime);
       Attack2CallBack?.Invoke();

        //int index = Random.Range(0, 100);
        //if (index < 50) {
        //    Attack1CallBack?.Invoke();
        //}
        //else {
        //    Attack2CallBack?.Invoke();
        //}
    }

    private void IdleCallback() {

        
        flt_CurrentTime = 0;
        boss_Animator.SetFloat(bossAnimationName.IdleBattel, 0, 0.01f, Time.deltaTime);
       
    }

    private void Update() {

        if (isRotate) {
            Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
             transform.eulerAngles = new Vector3(0, angle, 0);

        }

       
        BossMotion();
    }

    private void BossMotion() {

        switch (myBoss) {

            case BossState.idle:

                TimeCalculationForAttack();
                break;
            case BossState.attack:
                break;
            case BossState.knockBack:
                EnemyKnockBackMotion();
                break;

            default:
                break;
        }
    }


    private void TimeCalculationForAttack() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            ChangeState(BossState.attack);
        }
    }

    public void ChangeState(BossState CurrentState) {

        Debug.Log("Change State");
        myBoss = CurrentState;

        if (myBoss == BossState.idle) {

           
             IdleCallBack?.Invoke();

        }
        else if (myBoss == BossState.attack) {

          
            AttackCallBack?.Invoke();

        }
    }
    public void ChangeAnimation(string MyAnimation) {

      
        boss_Animator.SetTrigger(MyAnimation);
    }


    private void EnemyKnockBackMotion() {
        boss_Animator.SetFloat(bossAnimationName.IdleBattel,0, 0.01f, Time.deltaTime);


        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * currentAffectedGravityForce;
        }


        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
        transform.rotation = KnockBackRotation;
    }

    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

      
        isKnockBackStart = true;
        myBoss = BossState.knockBack;
        flt_KnockBackSpeed = knockBackSpeed - (knockBackSpeed * perasantageOfBlock / 100);
        knockBackDirection = dirction;
        KnockBackRotation = transform.rotation;
        if (cour_KncokBack != null) {
            StopCoroutine(cour_KncokBack);
        }

        cour_KncokBack = StartCoroutine(StopKnockbackOverTime());
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

        isKnockBackStart = false;
       

    }

    public void SetInVisible() {
       
    }

    public void SetVisible() {
        
    }



}

public enum BossState {
    idle,attack ,knockBack, Not_Ground
}

[System.Serializable]
public struct BossAnimationName {

    public string IdleBattel;
    public string Attack1;
    public string Attack2;
    public string Run;
    public string joom;
   
}
