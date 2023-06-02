using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.AI;


public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private GameObject obj_Obstckle;
    [SerializeField] private LayerMask obstckleLayer;
    [SerializeField]private EnemyWave[] all_EnemyWave;
    private EnemyWave currentWave;
    public float flt_Boundry;
    public float flt_BoundryX;
    public float flt_BoundryZ;
    public List<GameObject> list_AllEnemyInActiveInLevel;
    public bool isStartWave;
    [SerializeField]private float flt_CurrentTime;
    public int currentWaveIndex = 0;
    [SerializeField]private int currentEnemyIndex = 0;
    private bool isSpawn = true;

    private void Awake() {
        instance = this;
        currentWave = all_EnemyWave[0];
        isStartWave = true;

    }

   
    public void StartLevel() {
        SpwnLevel();
      
    }

    private void SpwnLevel() {
        for (int i = 0; i < 5; i++) {
            bool isSpawn = false;
            while (!isSpawn) {
                Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), 0,
                                Random.Range(flt_Boundry, flt_BoundryZ));
                Collider[] all_Collider = Physics.OverlapSphere(postion, 3, obstckleLayer);

                if (all_Collider.Length == 0) {
                    Instantiate(obj_Obstckle, postion, transform.rotation);
                    isSpawn = true;
                }
            }


        }
        //navMeshSurface.BuildNavMesh();
    }

    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }

        // Testing
        if (Input.GetKeyDown(KeyCode.Tab)) {
            SpawnEnemy();
        }

        // Testing  

      

       // SpawnCurrentWaveEnemy();
    }
    public void NextWaveSetup() {

        currentWave = all_EnemyWave[currentWaveIndex];
        currentEnemyIndex = 0;
        isStartWave = true;
        GameManager.instance.isKilltimeCalculation = true;
    }

    private void SpawnCurrentWaveEnemy() {
        if (!isStartWave) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>currentWave.flt_SpawnRate) {
            flt_CurrentTime = 0;
            SpawnEnemy();
            currentEnemyIndex++;
            if (currentEnemyIndex > currentWave.all_Enemy.Length-1) {
                isStartWave = false;
            }
        }
    }

    private void SpawnEnemy() {

        currentWave.all_Enemy[currentEnemyIndex].GetComponent<EnemyHandler>().SpawnEnemy();
       
    }

    public void ADDListOfEnemy(GameObject current) {
        list_AllEnemyInActiveInLevel.Add(current);
       
    }


    public void RemoveListOfEnemy(GameObject current) {

        GameManager.instance.IncreasingkillStreachIndex();
        list_AllEnemyInActiveInLevel.Remove(current);
        Destroy(current);

       

        if (!isStartWave && list_AllEnemyInActiveInLevel.Count == 0) {

            GameManager.instance.isKilltimeCalculation = false;
            currentWaveIndex++;
            if (currentWaveIndex > all_EnemyWave.Length - 1) {
                UIManager.instance.uiVictory.PlayVictoryAnimation();
            }
            else {
                UIManager.instance.uiPenalScreen.PlayMissionCompltedAnimation();

            }
        }
    }
}


[System.Serializable]
public class EnemyWave {

    public GameObject[] all_Enemy;
    public float flt_SpawnRate;
}

