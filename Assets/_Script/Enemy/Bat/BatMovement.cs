using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BatMovement : MonoBehaviour
{
    [Header("Componant")]
    [SerializeField] private Animator animator;

    public EnemyState enemyState;

    [Header("Bat - data")]
    
   [SerializeField]private bool isEnemyCharged = true;
   [SerializeField]private float flt_MaxChargeTime = 3;
   [SerializeField]private float flt_CurrentTime = 0;
   [SerializeField] private Vector3 targetPostion;
   private float flt_MovementSpeed = 1.5f;
   [SerializeField]private bool isVisible = true;

    


    //Id
    private const string Id_Idle = "Idle";

   

    private const string Id_Attack = "Attack";


   

    private void Update() {

        if (!GameManager.instance.isPlayerLive) {

            animator.SetTrigger(Id_Idle);
            return;
        }

        if (enemyState == EnemyState.BlackHole) {
            return;
        }
        if (enemyState == EnemyState.charge) {

            if (isVisible) {
                ChargingTime();
            }
           
        }
        else if (enemyState == EnemyState.Run) {

            if (isVisible) {
                BatNormalMotion();
            }
           
        }

    
      
    }

    public void setInvisible() {
        isVisible = false;
       
    }


    public void SetVisible() {
        isVisible = true;
    }
    public void HitByBlackHole(Transform _target) {

        enemyState = EnemyState.BlackHole;
        transform.SetParent(_target);
    }
    private void BatNormalMotion() {

        transform.position = Vector3.MoveTowards(transform.position, targetPostion, flt_MovementSpeed
            * Time.deltaTime);
        float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, targetPostion));
        if (flt_Distance<0.2f) {

            enemyState = EnemyState.charge;
        
        }
    }

    private void ChargingTime() {
       
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>flt_MaxChargeTime) {

            enemyState = EnemyState.Run;
            GetRandomPostionToBatMove();
            flt_CurrentTime = 0;
           
        }
    }

    private void GetRandomPostionToBatMove() {
        targetPostion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry,
                                                    LevelManager.instance.flt_BoundryX), transform.position.y,
                                    Random.Range(LevelManager.instance.flt_Boundry,
                                                  LevelManager.instance.flt_BoundryZ));
    }

   
    public void SetAttck() {

        animator.SetTrigger(Id_Attack);
    }
    public void DefaultAnimator() {
       
         animator.SetTrigger(Id_Idle);
        
    }

   
}
