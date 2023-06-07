using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Components")]
    private PlayerShooting playerShooting;
    public Animator player_Animator;
    [SerializeField] private VariableJoystick joystick;
    

    [Header("Player properties")]
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private float flt_JumpForce;
    [SerializeField] private float flt_RotateSpeed;
    private float targetAngle;
    
    private float flt_SpeedOfTarget = 250;



    private float flt_KnockBackTime = 0.5f;
    private bool isKnockBackStart;
   [SerializeField] private float flt_KnockBackSpeed;
    private Vector3 knockBackDirection;
   

    //inputs

    private float flt_VerticalInput;
    private float flt_HorizontalInput;
    private bool isJump;
    private int jumpCount;
    

    // tag & Id
  
    private string tag_Ground = "Ground";
    private string id_Idle = "idle";
    private string id_Running = "Run";

    //Coroutine

    private Coroutine coro_KnockBack;
   

    private void Start() {
                   
        playerShooting = GetComponent<PlayerShooting>();
    }
    private void Update() {

        if (!GameManager.instance.isPlayerLive) {
            return;
        }

      
        GetInput();
        PlayerMotion();
      
        //PlayerJump();
    }

    private void RotatePlayer() {
        if (playerShooting.isEnemyAcive) {
            return;
        }

        targetAngle = Mathf.Atan2(flt_HorizontalInput, flt_VerticalInput) * Mathf.Rad2Deg;

        Quaternion Qua_Target = Quaternion.Euler(0, targetAngle, 0);
        Quaternion current = transform.rotation;

        transform.rotation = Quaternion.Slerp(current, Qua_Target, flt_SpeedOfTarget * Time.deltaTime);
    }

    private void GetInput() {

        //flt_HorizontalInput = Input.GetAxis("Horizontal");
        //flt_VerticalInput = Input.GetAxis("Vertical");

        flt_HorizontalInput = joystick.Horizontal;
        flt_VerticalInput = joystick.Vertical;

        //if (Input.GetMouseButtonDown(0)) {
        //    isJump = true;
        //}
        //else {
        //    isJump = false;
        //}

    }
    private void PlayerJump() {

        //if ( isJump && jumpCount<2) {
        //    if (!isKnocBackStart) {
        //        playerRb.velocity = Vector3.zero;
        //    }
        //    playerRb.AddForce(Vector3.up * flt_JumpForce, ForceMode.Impulse);
        //    jumpCount++;
        //}
   
    }


   

    private void PlayerMotion() {

        if (isKnockBackStart) {
            PlayerKnockbackMotion();
        }
        else {
            PlayerNormalMotion();
        }
    
    }

    private void PlayerNormalMotion() {

        if (flt_HorizontalInput == 0 && flt_VerticalInput == 0) {

            player_Animator.SetTrigger(id_Idle);
            return;
        }

        RotatePlayer();
        player_Animator.SetTrigger(id_Running);

        Vector3 inputVector = new Vector3(flt_HorizontalInput, 0, flt_VerticalInput);
        transform.Translate(inputVector * flt_MovementSpeed * Time.deltaTime,Space.World);
       

    }
    private void PlayerKnockbackMotion() {

        player_Animator.SetTrigger(id_Idle);
        transform.Translate( knockBackDirection* flt_KnockBackSpeed * Time.deltaTime, Space.World);
    }

    public void SetBlinq() {
        if (coro_KnockBack != null) {
            StopCoroutine(coro_KnockBack);
        }
    }

    public void KnockBack(Vector3 dirction,float knockBackSpeed) {

        
        isKnockBackStart = true;
        flt_KnockBackSpeed = knockBackSpeed;
        knockBackDirection = new Vector3(dirction.x,0,dirction.z).normalized;
        if (coro_KnockBack != null) {
            StopCoroutine(coro_KnockBack);
        }
      
        coro_KnockBack =   StartCoroutine(StopKnockbackOverTime());
    }

    private IEnumerator StopKnockbackOverTime() {
       
        float currentKnockbackTime = 0f;
        float maxTime = flt_KnockBackTime;

        float startForce = flt_KnockBackSpeed;
        float endForce = 0f;

        while (currentKnockbackTime < 1) {

            currentKnockbackTime += Time.deltaTime/ maxTime;

            flt_KnockBackSpeed = Mathf.Lerp(startForce, endForce, currentKnockbackTime);
            yield return null;
        }

        isKnockBackStart = false;
        
    }



   

   
}







