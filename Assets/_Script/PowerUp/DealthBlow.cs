using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealthBlow : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Maxtime;

    private void OnEnable() {
        SetDealthBlowPowerUpHandler();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_Maxtime);
    }
    private void Update() {
       
        PowerUpHandler();
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_Maxtime) {
            this.gameObject.SetActive(false);
            GameManager.instance.isDealthBlowPowerUpActivated = false;
        }
    }

    private void SetDealthBlowPowerUpHandler() {
        flt_CurrentTime = 0;
       
        GameManager.instance.isDealthBlowPowerUpActivated = true;
    }
}
