using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichochestPowerUp : MonoBehaviour
{

  
    [SerializeField] private bool ispowerUpStart;

    [SerializeField] private GameObject obj_Indicator;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetLifeStealPowerUp();
        }
       
    }

    private void PowerUpHandler() {
        if (!ispowerUpStart) {
            return;
        }

       
    }

    private void SetLifeStealPowerUp() {
        ispowerUpStart = true;
       
        GameManager.instance.isRichoestPowerUp = true;
        obj_Indicator.gameObject.SetActive(true);
    }
}
