using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mini_PlayerCantroller : MonoBehaviour {

    [SerializeField] private float flt_Boundry;
    [SerializeField] private int flt_MovementSpeed;
    [SerializeField] private int flt_RotationYSpeed;
    [SerializeField] private float flt_RotationYAngle;
    [SerializeField] private int flt_RotationZSpeed;
    [SerializeField] private float flt_RotationZAngle;
    private Vector3 targetRotation;
    
     
    private float flt_Input;

 


    private void Update() {


        if (!Mini_GameManager.instance.IsPlayerLive) {
            return;
        }

        PlayerInput();
        PlayerMovement();
    }

    private void PlayerMovement() {

        transform.Translate(Vector3.right * flt_Input * flt_MovementSpeed * Time.deltaTime);

      
        float Y = transform.eulerAngles.y;
        Y = Mathf.LerpAngle(Y, targetRotation.y, flt_RotationYSpeed*Time.deltaTime);
        float Z = transform.eulerAngles.z;
        Z = Mathf.LerpAngle(Z, targetRotation.z, flt_RotationZSpeed * Time.deltaTime);


        transform.eulerAngles = new Vector3(0, Y, Z);

        float postion = transform.position.x;
        postion = Mathf.Clamp(postion, -flt_Boundry, flt_Boundry);
        transform.position = new Vector3(postion, 0, 0);

    }

    private void PlayerInput() {

        flt_Input = Input.GetAxis("Horizontal");
        if (flt_Input > 0) {

            targetRotation = new Vector3(0, flt_RotationYAngle, -flt_RotationZAngle);
        }
        else if (flt_Input < 0) {

            targetRotation = new Vector3(0, -flt_RotationYAngle, flt_RotationZAngle);
        }
        else {

            targetRotation = Vector3.zero;
        }

    }
}

