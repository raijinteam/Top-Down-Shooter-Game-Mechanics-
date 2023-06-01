using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealPowerUp : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeForThisPowerUp;
    [SerializeField] private bool ispowerUpStart;

    [SerializeField] private GameObject obj_Indicator;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetLifeStealPowerUp();
        }
        PowerUpHandler();
    }

    private void PowerUpHandler() {
        if (!ispowerUpStart) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForThisPowerUp) {
            ispowerUpStart = false;
            GameManager.instance.isLifeStealPowerUpActive = false;
            obj_Indicator.gameObject.SetActive(false);
        }
    }

    private void SetLifeStealPowerUp() {
        ispowerUpStart = true;
        flt_CurrentTime = 0;
        GameManager.instance.isLifeStealPowerUpActive = true;
        obj_Indicator.gameObject.SetActive(true);
    }
}
