using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealthBlow : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Maxtime;
    [SerializeField] private bool isPoweupStart;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetDealthBlowPowerUpHandler();
        }
        PowerUpHandler();
    }

    private void PowerUpHandler() {
        if (!isPoweupStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_Maxtime) {
            isPoweupStart = false;
            GameManager.instance.isDealthBlowPowerUpActivated = false;
        }
    }

    private void SetDealthBlowPowerUpHandler() {
        flt_CurrentTime = 0;
        isPoweupStart = true;
        GameManager.instance.isDealthBlowPowerUpActivated = true;
    }
}
