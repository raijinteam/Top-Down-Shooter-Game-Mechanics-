using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beeAttack : MonoBehaviour
{
    [SerializeField] private Wepon wepon;
    private BeeMotion enemyMovement;
    private float flt_DealyBetweenTwoAttack = 1.5f;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private bool isattack;
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
        enemyMovement = GetComponent<BeeMotion>();
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return; // player dead
        }

        if (!IsVisible) {
            return; // player invisible powerup.
        }
        AttackBySword();
        HandlingAttack();
    }

    private void AttackBySword() {

        if (isAttckinInRange) {

            enemyMovement.enemyState = EnemyState.isbulletSpawn;
            if (!isattack) {

                enemyMovement.SetAttackAnimation(true);
                isattack = true;
                wepon.Sword.enabled = true;
                flt_CurrentTime = 0;
            }

        }
        else {

            if (enemyMovement.enemyState == EnemyState.isbulletSpawn) {
                enemyMovement.enemyState = EnemyState.Run;
            }


            enemyMovement.SetAttackAnimation(false);
        }
    }
    private void HandlingAttack() {
        if (!isattack) {
            return;
        }
        enemyMovement.SetAttackAnimation(false);
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_DealyBetweenTwoAttack) {
            isattack = false;
            wepon.Sword.enabled = false;
        }
    }
}