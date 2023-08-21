using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    [SerializeField]private Wepon wepon;
    private EnemyMovement enemyMovement;
    private float flt_DealyBetweenTwoAttack = 1;
    [SerializeField]private float flt_CurrentTime;
    [SerializeField]private bool isattack;
    public bool isAttckinInRange;
 
    public bool IsVisible;


    private void OnEnable() {

        if (GameManager.instance.IsInVisblePowerUpActive) {
            IsVisible = false; 
        }
        else {
            IsVisible = true;
        }
    }

    private void Start() {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return; // player dead
        }

        if (!IsVisible) {
            return; // player invisible powerup.
        }
       
    }

    private void AttackBySword() {

      
    }
    private void HandlingAttack() {
      
    }
   
}
