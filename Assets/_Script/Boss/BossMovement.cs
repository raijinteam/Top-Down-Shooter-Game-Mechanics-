using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public  class BossMovement : MonoBehaviour {

    public BossAnimationName bossAnimationName;
   
  
    public Animator boss_Animator;
    [SerializeField] private SpikePooler spike;
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
    private float gravityForce = -0.75f;
    private float flt_KnockBackTime = 0.5f;
    private bool isAttacking = false;
    private bool isBossRotate;

    public bool isVisible;

    //private float groundCheckBufferTime = 0.2f;
    //private float currentGroundCheckTime = 0f;


    private void OnCollisionEnter(Collision collision) {
        isGrounded = true;
      
    }

    private void OnCollisionStay(Collision collision) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {

       // currentGroundCheckTime = 0f;
        isGrounded = false; 
    }

    private void OnEnable() {

        Instantiate(spike, transform.position, transform.rotation);
        boss_Animator.SetBool(bossAnimationName.joom, true);
        isBossRotate = true;
        SetRotate += RoationCalbback;
        isVisible = true;

    }
    private void OnDisable() {
        SetRotate -= RoationCalbback;
    }

    private void RoationCalbback(bool isRotate) {
        isBossRotate = isRotate;
    }

   



    private void Update() {

        //if (isRotate) {
        //    Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        //    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //     transform.eulerAngles = new Vector3(0, angle, 0);

        //}

        //if (!isGrounded) {
        //    currentGroundCheckTime += Time.deltaTime;
        //    if (currentGroundCheckTime >= groundCheckBufferTime) {
        //        enemyState = EnemyState.Not_Ground;
        //    }
        //}

        //BossMotion();

        AttackHandler();
        EnemyKnockBackMotion();

        BossRotation();
       
       

    }

    private void AttackHandler() {

        if (!isVisible) {
            return;
        }
        else if (isAttacking) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
            isAttacking = true;
            ChooseAndDoRandomAttack();
        }
    }

    private void ChooseAndDoRandomAttack() {

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

    private void BossRotation() {

        if (!isVisible) {
            return;
        }
        else if (!isBossRotate) {
            return;
        }

        Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, angle, 0);
    }



    public void SetIdleAnimation() {
        boss_Animator.SetBool(bossAnimationName.Attack1, false);
        //boss_Animator.SetFloat(bossAnimationName.IdleBattel, 0, 0.01f, Time.deltaTime);
        
    }

    public void AttackEnded() {
        isAttacking = false;
        flt_CurrentTime = 0;

        flt_CurrentTime = 0;
        boss_Animator.SetBool(bossAnimationName.Attack1, false);
        boss_Animator.SetBool(bossAnimationName.Attack2, false);
       

    }

    public void ChangeAnimation(string MyAnimation) {    
        boss_Animator.SetBool(MyAnimation,true);
    }

    


    private void EnemyKnockBackMotion() {

       

        if (!isGrounded) {
            knockBackDirection.y = MathF.Abs(transform.position.y) * gravityForce;
        }


        transform.Translate(knockBackDirection * flt_KnockBackSpeed * Time.deltaTime, Space.World);
       // transform.rotation = KnockBackRotation;
    }

    public void KnockBack(Vector3 dirction, float knockBackSpeed) {

        if(!isBossRotate) {
            return;
        }

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

    }

    public void SetInVisible() {

        isVisible = false;
    }

    public void SetVisible() {

        isVisible = true;
    }
}


[System.Serializable]
public struct BossAnimationName {

    public string IdleBattel;
    public string Attack1;
    public string Attack2;
    public string Run;
    public string joom;
   
}
