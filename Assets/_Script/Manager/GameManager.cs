using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameManager : MonoBehaviour {
    public static GameManager instance;


    [SerializeField] private int GameLevel; // player level.private int playerLevel
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
    [SerializeField] private int  newwaveStartSpawn = 5;

    [Header("Boss handling")]
    [SerializeField] private bool IsBossActvated;
    [SerializeField] private EnemyHandler boss;
    [SerializeField] private int BossIndex = 5;
   

    private void Awake() {
        instance = this;
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -15f, 0f);
    }


    public int GetLevel() {
        return GameLevel;
    }
    public void SetLevel() {
        GameLevel++;
       
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            Time.timeScale = 1;
        }
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
        UIManager.instance.waveStartingScreen.PlayUiWaveStartAnimation(currentWaveIndex + 1);
       



    }

    public void PLayLeveAnimation() {
        UIManager.instance.uilevelScreen.PlayUiWaveAnimation(currentStageIndex + 1);
    }
    

    

    public void EnemyKilled(Transform _enemy) {

        list_ActiveEnemies.Remove(_enemy);
        Destroy(_enemy.gameObject);

        if (list_ActiveEnemies.Count == 0) {

            GameManager.instance.isKilltimeCalculation = false;
            currentWaveIndex++;
            if (currentWaveIndex > totalNumberOfWavesInThisStage) {

                //if (currentStageIndex % BossIndex == 0 && !IsBossActvated) {
                //    Debug.Log("Boss Is Coming");
                //    IsBossActvated = true;
                //    PlayerManager.instance.StageCompletd();
                //    UIManager.instance.waveStartingScreen.gameObject.SetActive(true);
                //    UIManager.instance.waveStartingScreen.PlayUiBossLevelAnimation();
                //    return;
                //}

                if (currentWaveIndex != 0) {
                    IsBossActvated = false;
                    Debug.Log("Stage Change");
                    currentWaveIndex = 0;
                    currentStageIndex++;
                    UIManager.instance.uiStageCompletedScreen.PlayVictoryAnimation();
                    PlayerManager.instance.StageCompletd();
                    
                }



            }
            else {
                Debug.Log("Wave Change");
                
                UIManager.instance.uiWaveCompltedScreen.PlayWaveCompltedAnimation();
                numberOfEnemiesInCurrentWave = LevelManager.instance.GetEnemiesInCurrentWave();
            }
        }


    }

    public void WaveCompleteAnimationComplted() {
        UIManager.instance.waveStartingScreen.PlayUiWaveStartAnimation(currentWaveIndex + 1);
    }

    public void SpawnBoss() {
        boss.SpawnEnemy();

    }

    public void ADDListOfEnemy(Transform current) {
        list_ActiveEnemies.Add(current);
        Debug.Log("EndAdd In List");

    }

    public void SetCurrentStageData(EnemyAndItsPresantage _enemyData, int _numberOfWaves, int _enemyInWaveOne) {

        currentEnemyData = _enemyData;
        totalNumberOfWavesInThisStage = _numberOfWaves;
        numberOfEnemiesInCurrentWave = _enemyInWaveOne;
    }

    public void SpwnEnemyNewWave() {

        SpawnFirstWave(); 
        // StartCoroutine(SpawnEnemyInIntervals());
    }

    private void SpawnFirstWave() {
        int BaseValue = newwaveStartSpawn;
       BaseValue = 1;
        for (int i = 0; i <BaseValue; i++) {
            SpawnEnemy();
        }
    }

    private IEnumerator SpawnEnemyInIntervals() {

        int BaseValue = newwaveStartSpawn;
        BaseValue = numberOfEnemiesInCurrentWave - BaseValue;

        for (int i = 0; i < BaseValue; i++) {
            yield return new WaitForSeconds(currentEnemyData.waitBetweenEnemySpawn);
            SpawnEnemy();

        }

        //int BaseValue = newwaveStartSpawn ;

        //for (int i = 0; i < numberOfEnemiesInCurrentWave; i++) {

        //    if (i < BaseValue) {
        //        SpawnEnemy();

        //        yield return new WaitForSeconds(50000);
        //    }
        //    else {

        //        yield return new WaitForSeconds(currentEnemyData.waitBetweenEnemySpawn);
        //        SpawnEnemy();
        //    }


        //}
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
