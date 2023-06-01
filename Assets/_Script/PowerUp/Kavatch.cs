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
    [SerializeField] private bool isPowerUpStart;


    [SerializeField] private GameObject obj_Kavach;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetkavachPowerUp();
        }
        PowerUpHandler();
    }

    private void PowerUpHandler() {
        if (!isPowerUpStart) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxTimeForThisPowerUp) {
            isPowerUpStart = false;
            obj_Kavach.gameObject.SetActive(false);
            collisionHandling.StopKavach();
        }
    }

    private void SetkavachPowerUp() {
        isPowerUpStart = true;
        flt_CurrentTime = 0f;
        obj_Kavach.gameObject.SetActive(true);
        collisionHandling.SetKavachActive();

    }
}
