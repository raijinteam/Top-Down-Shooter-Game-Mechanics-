using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpHandler : MonoBehaviour
{

    [Header("all_PowerUp")]
    [SerializeField] private GameObject[] all_PowerUp;
    [SerializeField] private int SelectivePowerUpIndex;


    [Header("Leval")]

    [SerializeField] private float currentPlayerPoint; // flt_CurrentPointsForLevel
    [SerializeField] private float currrentPointToNextLevel; // flt_RequiredPointsForLevelUp
    [SerializeField] private float requiredLevelToIncreased; // flt_ScoreIncreasePercentage

    [Header("Ultimate")]
    [SerializeField] private float requiredPointToUnlockUltimate; //flt_PointsRequiredforUltimate
    public bool isPowerUpStart;
   

    [SerializeField] private float currenPointToForUltimate; // flt_CurrentPointsForUltimate


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            IncreasingPlayerPoint(10);
        }
    }

    public void setPowerUpIndex(int index) {
        SelectivePowerUpIndex = index;
        UIManager.instance.uIGamePlayScreen.txt_PowerUpName.text = all_PowerUp[index].name;
    }

    public void StartUltimate() {

        all_PowerUp[SelectivePowerUpIndex].gameObject.SetActive(true);

    }


    public void UltimateCompleted() {
        currenPointToForUltimate = 0;
        isPowerUpStart = false;

    }

    public void IncreasingPlayerPoint(int point) {
        currentPlayerPoint += point;

      

        if (currentPlayerPoint >= currrentPointToNextLevel) {

           
            GameManager.instance.SetLevel();

            currentPlayerPoint = currrentPointToNextLevel - currentPlayerPoint;
            currrentPointToNextLevel = currrentPointToNextLevel +
                         (currrentPointToNextLevel * requiredLevelToIncreased * 0.01f);
        }

        if (!isPowerUpStart) {
            currenPointToForUltimate += point;
            float fillAmmmount = (currenPointToForUltimate / requiredPointToUnlockUltimate);
           
            UIManager.instance.uIGamePlayScreen.SetUltimateScore(fillAmmmount);
        }


       

    }
}
