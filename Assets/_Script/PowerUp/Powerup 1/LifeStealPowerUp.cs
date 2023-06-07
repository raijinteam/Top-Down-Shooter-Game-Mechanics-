using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealPowerUp : MonoBehaviour
{
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeForThisPowerUp;
   

    [SerializeField] private GameObject obj_Indicator;

    private void OnEnable() {
        SetLifeStealPowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeForThisPowerUp);
    }

    private void Update() {
        
        PowerUpHandler();
    }

    private void PowerUpHandler() {
       

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForThisPowerUp) {
            this.gameObject.SetActive(false);
            GameManager.instance.isLifeStealPowerUpActive = false;
            obj_Indicator.gameObject.SetActive(false);
        }
    }

    private void SetLifeStealPowerUp() {
      
        flt_CurrentTime = 0;
        GameManager.instance.isLifeStealPowerUpActive = true;
        obj_Indicator.gameObject.SetActive(true);
    }
}
