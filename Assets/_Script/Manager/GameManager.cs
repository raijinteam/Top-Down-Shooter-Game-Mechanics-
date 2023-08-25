using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Cinemachine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;


    [SerializeField] private int playerLevel; // player level.private int playerLevel
    [Header("PlayerData")]
    public bool isPlayerLive;

    [Header("Game Elements")]
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Transform startBoat;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private StartPositionSpline spline_StartPosition;
    [SerializeField] private Transform endBoat;
    [SerializeField] private Transform endSpawnPostion;
    [SerializeField] private EndPostionSpilne endPostionSpilne;
    [SerializeField] private Vector3 endBoatResetPostion;
    [SerializeField] private Vector3 startBoatResetPostion;


    [Header("PowerUp Handler")]
    [SerializeField]private bool isRichoestPowerUp;
    [SerializeField]private bool isDealthBlowPowerUpActivated;
    [SerializeField]private bool isLifeStealPowerUpActive;
    [SerializeField]private bool isMissilePowerUpActive;
    [SerializeField] private bool isInvisiblePowerupActive;

    

    [SerializeField] private bool isMultyShot;

    [Header("killStreach Data")]
    [SerializeField]private  bool isKilltimeCalculation;
    [SerializeField] private KillStretch killStretch;
    [SerializeField] private float flt_MaxKillStreachTime;
    [SerializeField] private float flt_CurrentTime;

    [Header("Wave Handling")]
    public List<Transform> list_ActiveEnemies = new List<Transform>();
    public EnemyAndItsPresantage currentEnemyData;
    public int currentStageIndex;
    public int currentWaveIndex = 0;
    public int totalNumberOfWavesInThisStage;
    public int numberOfEnemiesInCurrentWave;
    [SerializeField] private int  newwaveStartSpawn = 5;
    [SerializeField] private int noOfEnemyKilled;
  
    [SerializeField] private GameObject bossCamera;

    [Header("BossLevel")]
    [SerializeField] private EnemyHandler boss;
    [SerializeField]private bool isbossLevel;

    public Transform enemySpanwble;

    [Header("UI Elements")]
    private float waveNumberAnimationDuration = 2f;

    [Header("TEMP COMPONENTS")]
    public PlayerData Obj_Player;
    public PlayerData Player;
    

    private void Awake() {
        instance = this;
    }

    private void Start() {
        Physics.gravity = new Vector3(0, -15f, 0f);
       

        StartGame();
    }



    #region Property
   
    public int CurrentGameLevel {
        get { return playerLevel; }
        set { playerLevel++; }
    }

    public bool IsRechoest {
        get { return isRichoestPowerUp; }
        set { isRichoestPowerUp = value; }
    }
    public bool IsDeathBlow {
        get { return isDealthBlowPowerUpActivated; }
        set { isDealthBlowPowerUpActivated = value; }
    }
    public bool IsLifeSteal {
        get { return isLifeStealPowerUpActive; }
        set { isLifeStealPowerUpActive = value; }
    }
    public bool IsMicroMissile {
        get { return isMissilePowerUpActive; }
        set { isMissilePowerUpActive = value; }
    }

    public bool IsKillStreak {
        get { return isKilltimeCalculation; }
        set { isKilltimeCalculation = value; }
    }
    public bool IsMultyShot {
        get { return isMultyShot; }
        set { isMultyShot = value; }
    }
    public bool IsInVisblePowerUpActive {
        get { return isInvisiblePowerupActive; }
        set { isInvisiblePowerupActive = value; }
    }
    #endregion


    public void StartGame() {

        SpawnPLayer();
        GameStartAnimation();

        //startBoatPostion = gameStartBoat.transform.position;
        //endBoatPostion = GameEndBoat.transform.position;
    }

    private void SpawnPLayer() {
        PlayerData player = Instantiate(Obj_Player, spawnPostion.position, spawnPostion.rotation, startBoat);
        this.Player = player;

        playerCamera.Follow = player.transform;
      
    }

    private void GameStartAnimation() {

        Player.rb_Player.useGravity = false;
        Sequence seq = DOTween.Sequence();

        UIManager.instance.uilevelScreen.PlayLevelAnimation(currentStageIndex + 1);
        seq.Append(startBoat.transform.DOMoveZ(-40, 2)).AppendCallback(BoatReachedStartPoint);
    }

    private void BoatReachedStartPoint() {

        spline_StartPosition.StartJumpAnimation(Player.transform);
    }

    public void PlayerStartJumpAnimationCompleted() {
        isPlayerLive = true;
        StartSpawnCurrentLevel();
        Player.PlayerJumpAnimationCompleted();      
    }

    public void StartSpawnCurrentLevel() {

        startBoat.DOMoveY(-10, 5);


        //if (currentStageIndex % 2 == 0) {
        //    isbossLevel = true; 
        //}
        //else {
        //    isbossLevel = false;
        //}

       // isbossLevel = false;
        if (isbossLevel) {
            bossCamera.gameObject.SetActive(true);
            StartCoroutine(SpawnBoss());
        }
        else {
            bossCamera.gameObject.SetActive(false);
            LevelManager.instance.StartLevel();
           
            StartCoroutine(WaveChange());

        }



    }

    private IEnumerator SpawnBoss() {
        UIManager.instance.uiBossPanel.PlayLevelAnimation(2);
        yield return new WaitForSeconds(2);
        boss.SpawnEnemy();
    }

    public void SetCurrentStageData(EnemyAndItsPresantage _enemyData, int _numberOfWaves, int _enemyInWaveOne) {

        currentEnemyData = _enemyData;
        totalNumberOfWavesInThisStage = _numberOfWaves;
        numberOfEnemiesInCurrentWave = _enemyInWaveOne;

        
    }


    // enemiesKilledInCurrentWave = 0;
    // enemiesInCurrentWave = get from level data;
    // currentWaveIndex = 0;
    // Total number of waves = get from level data

    // private void KilledEnemy() {

    //enemiesKilledInCurrentWave += 1;

    //if (enemiesKilledInCurrentWave == enemiesInCurrentWave) {

    //    // completed wave

    //    // Check if completed Level

    //    currentWaveIndex += 1;

    //    if (currentWaveIndex >= TotalNumberOfWaves) {

    //        // Level completed
    //        current wave index = 0;
    //        enemiesKilledInCurrentWave = 0;


    //    }
    //    else {

    //        // Wave Completed
    //        enemiesKilledInCurrentWave = 0;
    //        // Start Next Wave
    //    }
    //}}



    private void Update() {
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(LevelChangeAnimation());
        }
    }
    public void EnemyKilled(Transform _enemy) {

       
        noOfEnemyKilled++;
        list_ActiveEnemies.Remove(_enemy);
        Destroy(_enemy.gameObject);


        if (isbossLevel) {
            Debug.Log("LevelChange");
            isbossLevel = false;
            StartCoroutine(LevelChangeAnimation());
        }
        else if (noOfEnemyKilled >= numberOfEnemiesInCurrentWave && list_ActiveEnemies.Count == 0) {
            currentWaveIndex++;

            if (currentWaveIndex > totalNumberOfWavesInThisStage) {
                // resetBullet();
                Debug.Log("LevelChange");
                StartCoroutine(LevelChangeAnimation());
            }
            else {
                // resetBullet();
                Debug.Log("WaveChange");

                UIManager.instance.uiWaveCompltedScreen.PlayWaveCompltedAnimation();
                StartCoroutine(WaveChange());            
            }
        }
    
       

    }


    public void gameCompletAnimation() {
      
       
        Sequence seq = DOTween.Sequence();


        CinemachineBasicMultiChannelPerlin camera = playerCamera.
              GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camera.m_AmplitudeGain = 0;
        camera.m_FrequencyGain = 0;
        endBoat.gameObject.SetActive(false);
        Player.transform.position = spawnPostion.position;
        Player.transform.rotation = spawnPostion.rotation;
        Player.transform.localScale = Obj_Player.transform.localScale;
        Player.transform.SetParent(startBoat.transform);
        endBoat.transform.position = new Vector3(endBoat.position.x, endBoat.position.y, 39);
        // cinemachine.Follow = levelStartPostion;
       
        startBoat.transform.localPosition = startBoatResetPostion;
        UIManager.instance.uIGamePlayScreen.img_BG.DOFade(0, 1);

        GameStartAnimation();

        
    }

    private void resetBullet() {
        for (int i = 0; i < enemySpanwble.childCount; i++) {
            Destroy(enemySpanwble.GetChild(i).gameObject);
        }
    }

    private IEnumerator LevelChangeAnimation() {
        UIManager.instance.uiStageCompletedScreen.PlayVictoryAnimation(2);
       
        yield return new WaitForSeconds(2);
        StartCoroutine(PlayerGoesToEndPostion(endSpawnPostion.position));
        currentStageIndex++;
        currentWaveIndex = 0;
        noOfEnemyKilled = 0;

        
      
    }

    private IEnumerator PlayerGoesToEndPostion(Vector3 targetpostion) {


        isPlayerLive = false;
        endBoat.gameObject.SetActive(true);
        endBoat.transform.localPosition = endBoatResetPostion;
        endBoat.transform.DOMoveY(-3.5f, 1);

        float flt_fltcurrenttime = 0;
        Vector3 dirction = (targetpostion - Player.transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.z, -dirction.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, targetAngle, 0);

        Vector3 targetPostion = targetpostion;

        Vector3 startPostion = Player.transform.position;

        Player.GetComponent<PlayerMovement>().SetAnimater(1);

        float CurrentMaxTime = 2;
        while (flt_fltcurrenttime < 1) {

            flt_fltcurrenttime += Time.deltaTime / CurrentMaxTime;

            Player.transform.position = Vector3.Lerp(startPostion, targetpostion, flt_fltcurrenttime);
            yield return null;
        }

        Player.transform.position = targetpostion;

        isPlayerLive = false;
        endPostionSpilne.EndJumpAnimation(Player.transform);

       


    

    }
    public void MoveBoatEndOfScreen() {


        Player.transform.SetParent(endBoat);

        Player.rb_Player.useGravity = false;
        // cinemachine.Follow = levelEndPostion;
        Player.rb_Player.velocity = Vector3.zero;
        Player.rb_Player.angularVelocity = Vector3.zero;
        Player.transform.localEulerAngles = Vector3.zero;
        Player.transform.localScale = Obj_Player.transform.localScale;
        isPlayerLive = false;
        IsKillStreak = false;

        endBoat.DOMoveZ(100, 2);
        UIManager.instance.uIGamePlayScreen.img_BG.DOFade(1, 2);
    }

    private IEnumerator WaveChange() {
       
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.waveStartingScreen.PlayUiWaveStartAnimation(currentWaveIndex + 1, waveNumberAnimationDuration);
        yield return new WaitForSeconds(waveNumberAnimationDuration/ 2);
        noOfEnemyKilled = 0;
        SpwnEnemyNewWave();
    }

    public void ADDListOfEnemy(Transform current) {
        list_ActiveEnemies.Add(current);
        Debug.Log("EndAdd In List");

    }

   

    private void SpwnEnemyNewWave() {

      
        SpawnFirstDirectEnmeySpawn(); 
        StartCoroutine(SpawnEnemyInIntervals());
    }

    private void SpawnFirstDirectEnmeySpawn() {
        int BaseValue = newwaveStartSpawn;
      
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


    public Transform GetPlayerTransform() {
        return Player.transform;
    }
}
