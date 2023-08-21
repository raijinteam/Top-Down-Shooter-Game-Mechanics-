using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{
    
    [SerializeField] private BossMovement boss;
    [SerializeField] private float flt_Speed;
    
    [SerializeField] private Vector3 targetPostion;
    [SerializeField]private LineRenderer line;
    [SerializeField] private float flt_Offset;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_ChargingTime;
    [SerializeField] private BossWepon bossWepon;
   
  
    [SerializeField]private bool isRun;
    [SerializeField]private bool isCharge;

    private Coroutine cour_Attack2;
    
   
    

    private void OnEnable() {
        bossWepon.gameObject.SetActive(true);
        boss.Attack2CallBack += AttackStart;
        boss.isAttack2Work = true;
    }

   

    private void OnDisable() {

        boss.Attack2CallBack -= AttackStart;
    }


    private void AttackStart() {
        isCharge = true;
        flt_CurrentTime = 0;
        line.gameObject.SetActive(true);
    }

    private void Update() {

        
        if (isCharge) {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, GameManager.instance.Player.transform.position));
            Vector3 direction = (GameManager.instance.Player.transform.position - transform.position).normalized;
            Vector3 postion = direction * distance + boss.transform.position;
            line.SetPosition(0, new Vector3(boss.transform.position.x,0,boss.transform.position.z));
            line.SetPosition(1, new Vector3(postion.x,0,postion.z));
            targetPostion = direction * (distance - flt_Offset) + boss.transform.position;
           
            chargeBoss();
        }
        else if (isRun) {

            transform.position = Vector3.MoveTowards(transform.position, targetPostion, flt_Speed * Time.deltaTime);

            float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, targetPostion));
          

            if (flt_Distance < 1.5f) {
                isRun = false;
                boss.boss_Animator.SetBool(boss.bossAnimationName.Run, false);

                boss.SetRotate?.Invoke(true);
                if (cour_Attack2 == null) {
                    cour_Attack2 = StartCoroutine(AttackAnimation()); ;
                }
                
            }
        }     
    }

   

    private IEnumerator AttackAnimation() {
        yield return new WaitForSeconds(0.2f);

        boss.SetRotate?.Invoke(false);
        boss.ChangeAnimation(boss.bossAnimationName.Attack2);
        yield return new WaitForSeconds(0.3f);
        bossWepon.EnableCollider(true);
        yield return new WaitForSeconds(2);

        bossWepon.EnableCollider(false);

        boss.SetRotate?.Invoke(true);
        cour_Attack2 = null;
        boss.isAttack2Work = false;
        boss.ChangeState(BossState.idle);
    }

    private void chargeBoss() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_ChargingTime) {
            isCharge = false;
            isRun = true;
            line.gameObject.SetActive(false);
            boss.boss_Animator.SetBool(boss.bossAnimationName.Run, true);
            boss.SetRotate?.Invoke(false);
        }
    }
}
