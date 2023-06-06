using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.AI;


public class LevelManager : MonoBehaviour {
    public static LevelManager instance;
 

    [Header("Enemy Spawn Data")]
    private int enemyListIndexToChooseFrom = 0;
   

    [Header("LevelIncresingData")]
    [SerializeField] private int baseWaveValue = 2;
    [SerializeField] private int waveIncreaseCount = 6; // total waves in stage will increase by 1 after every 6 levels
    [SerializeField] private int minimumEnemyCountInWave = 3; // increase minimum enemy to spawn inside 1 wave after every 3 levels
    [SerializeField] private int maximumEnemyCountInWave = 1; // increase maximum enemy to spawn inside 1 wave after every 3 levels
    [SerializeField] EnemyAndItsPresantage[] all_EnemyAndItsPersantage;

    [Header("This Wave Random Enemy Count")]
    [SerializeField] private int baseMinEnemyCount;
    [SerializeField] private int baseMaxEnemyCount;

    [Header("Ground - Boundry")]
    public float flt_Boundry;
    public float flt_BoundryX;
    public float flt_BoundryZ;
    
    public bool isStartWave;


    private void Awake() {
        instance = this;
    }


    public void StartLevel() {
        for (int i = 0; i < all_EnemyAndItsPersantage.Length; i++) {

            if (all_EnemyAndItsPersantage[i].ChageLevelValue >= GameManager.instance.currentStageIndex) {
                
                GameManager.instance.SetCurrentStageData(all_EnemyAndItsPersantage[i], baseWaveValue,
                    GetEnemiesInCurrentWave());
                break;
            }

        }
    }


   

   

   
    public void SetNewStage() {

        GameManager.instance.currentStageIndex++;

        if (GameManager.instance.currentStageIndex % waveIncreaseCount == 0) {
            baseWaveValue += 1;
        }
        if (GameManager.instance.currentStageIndex % minimumEnemyCountInWave == 0) {
            baseMinEnemyCount += 1;
        }
        if (GameManager.instance.currentStageIndex % maximumEnemyCountInWave == 0) {
            baseMaxEnemyCount += 1;
        }

        GameManager.instance.totalNumberOfWavesInThisStage = baseWaveValue;

     
        


    }

    public int GetEnemiesInCurrentWave() {
        int enemyCount = Random.Range(baseMinEnemyCount, baseMaxEnemyCount);
        return enemyCount;
    }


}








