using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kavatch : MonoBehaviour
{
    [Header("Componanat")]
    [SerializeField] private CollisionHandling collisionHandling;
    [Header("PowerUp - Cantroller")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_MaxTimeForThisPowerUp;
  


    [SerializeField] private GameObject obj_Kavach;

    private void OnEnable() {
        SetkavachPowerUp();
       UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxTimeForThisPowerUp);
    }
    private void Update() {
       
        PowerUpHandler();
    }

    private void PowerUpHandler() {
       
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForThisPowerUp) {
            this.gameObject.SetActive(false);
            obj_Kavach.gameObject.SetActive(false);
            collisionHandling.StopKavach();
        }
    }

    private void SetkavachPowerUp() {
       
        flt_CurrentTime = 0f;
        obj_Kavach.gameObject.SetActive(true);
        collisionHandling.SetKavachActive();

    }
}
