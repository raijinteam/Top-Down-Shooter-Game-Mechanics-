using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissilePowerUp : MonoBehaviour
{
    [Header("Componant")]
    [Header("PowerUp - cantroller")]
    [SerializeField] private float flt_MaxPowerUpTime;
    [SerializeField] private float flt_CurrenTime;
    [SerializeField] private bool isPowerUpTime;

   


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetMicroMissilePowerUp();
        }

       
    }

   

    private void SetMicroMissilePowerUp() {
        isPowerUpTime = true;
        flt_CurrenTime = 0;
        GameManager.instance.isMissilePowerUpActive = true;
    }
}
