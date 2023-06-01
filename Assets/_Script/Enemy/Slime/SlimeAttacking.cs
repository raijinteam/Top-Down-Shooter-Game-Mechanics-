using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttacking : MonoBehaviour {


    [SerializeField] private Wepon wepon;
    private SlimeMovement slimeMovement;
    private float flt_DealyBetweenTwoAttack = 1;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private bool isattack;
    public bool isAttckinInRange;
    public bool isvisible;

   

    private void Start() {
        slimeMovement = GetComponent<SlimeMovement>();
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        if (!isvisible) {
            return;
        }

        AttackSlime();
        HandlingAttack();
    }

    private void AttackSlime() {

        if (isAttckinInRange) {

            if (!isattack) {

                slimeMovement.SetAttackAnimation(true);
                isattack = true;
                wepon.Sword.enabled = true;
                flt_CurrentTime = 0;
            }

        }
        else {
            slimeMovement.SetAttackAnimation(false);
        }
    }
    private void HandlingAttack() {
        if (!isattack) {
            return;
        }
        slimeMovement.SetAttackAnimation(false);
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_DealyBetweenTwoAttack) {
            isattack = false;
            wepon.Sword.enabled = false;
        }
    }
   
}
