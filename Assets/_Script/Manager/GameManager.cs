using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public static GameManager instance;


    [SerializeField] private int GameLevel;
    [Header("PlayerData")]
    public bool isPlayerLive;

    [Header("PowerUp Handler")]
    public bool isRichoestPowerUp;
    internal bool isDealthBlowPowerUpActivated;
    public bool isLifeStealPowerUpActive;
    public bool isMissilePowerUpActive;
    [Header("killStreach Data")]
    public bool isKilltimeCalculation;

    [SerializeField] private KillStretch killStretch;
    [SerializeField] private float flt_MaxKillStreachTime;
    [SerializeField] private float flt_CurrentTime;

    [Header("Wave Handling")]
    public List<Transform> list_ActiveEnemies = new List<Transform>();
    public EnemyAndItsPresantage currentEnemyData;
    public int currentStageIndex;
    [SerializeField]private int currentWaveIndex = 0;
    public int totalNumberOfWavesInThisStage;
    public int numberOfEnemiesInCurrentWave;

    [Header("Boss handling")]
    [SerializeField] private bool IsBossActvated;
    [SerializeField] private EnemyHandler boss;
    [SerializeField] private int BossIndex = 5;
   

    private void Awake() {
        instance = this;
    }

    private void Start() {
        StartSpawningEnemyForCurrentWave();
    }


    public int GetLevel() {
        return GameLevel;
    }
    public void SetLevel() {
        GameLevel++;
    }

    private void Update() {
        KillstreachCalculation();
    }

    private void KillstreachCalculation() {
        if (!isKilltimeCalculation) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_MaxKillStreachTime) {
            killStretch.killTretchedIndex = 0;
            flt_CurrentTime = 0;
        }
    }
    public void IncreasingkillStreachIndex() {
        if (flt_CurrentTime < flt_MaxKillStreachTime) {
            killStretch.killTretchedIndex++;
        }
    }

    public void StartSpawningEnemyForCurrentWave() {

        LevelManager.instance.StartLevel();
        StartCoroutine(SpawnEnemyInIntervals());
    }

    public void EnemyKilled(Transform _enemy) {

        list_ActiveEnemies.Remove(_enemy);
        Destroy(_enemy.gameObject);

        if (list_ActiveEnemies.Count == 0) {

            GameManager.instance.isKilltimeCalculation = false;
            currentWaveIndex++;
            if (currentWaveIndex > totalNumberOfWavesInThisStage) {

                if (currentStageIndex % BossIndex == 0  && !IsBossActvated) {
                    Debug.Log("Boss Is Coming");
                    IsBossActvated = true;
                    UIManager.instance.uiLevelPanel.gameObject.SetActive(true);
                    UIManager.instance.uiLevelPanel.PlayUiLevelAnimation();
                    return;
                }

                IsBossActvated = false;
                Debug.Log("Stage Change");
                currentWaveIndex = 0;
                UIManager.instance.uiVictory.PlayVictoryAnimation();
                LevelManager.instance.SetNewStage();
               
              
            }
            else {
                Debug.Log("Wave Change");
                UIManager.instance.uiPenalScreen.PlayMissionCompltedAnimation();
              numberOfEnemiesInCurrentWave = LevelManager.instance.GetEnemiesInCurrentWave();
            }
        }

       
    }

    public void SpawnBoss() {
        boss.SpawnEnemy();

    }

    public void ADDListOfEnemy(Transform current) {
        list_ActiveEnemies.Add(current);

    }

    public void SetCurrentStageData(EnemyAndItsPresantage _enemyData, int _numberOfWaves, int _enemyInWaveOne) {

        currentEnemyData = _enemyData;
        totalNumberOfWavesInThisStage = _numberOfWaves;
        numberOfEnemiesInCurrentWave = _enemyInWaveOne;
    }

    public void SpwnEnemyNewWave() {
        StartCoroutine(SpawnEnemyInIntervals());
    }

    private IEnumerator SpawnEnemyInIntervals() {

        
        for (int i = 0; i < numberOfEnemiesInCurrentWave; i++) {

            SpawnEnemy();
            yield return new WaitForSeconds(currentEnemyData.waitBetweenEnemySpawn);
        }
    }

    private void SpawnEnemy() {
        int Current = Random.Range(0, 100);

        for (int i = 0; i < currentEnemyData.all_Enemy.Length; i++) {

            if (Current < currentEnemyData.all_Persantage[i]) {

                currentEnemyData.all_Enemy[i].SpawnEnemy();
                break;
            }
        }
    }
}
