using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChainPowerUp : MonoBehaviour
{

    [SerializeField] private ChainData chainData;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private List<GameObject> list_ChainAffected;
    [SerializeField] private bool isSetTarget;

    private void Update() {
       
        ChainHanadler();
    }

    private void ChainHanadler() {
        if (isSetTarget) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        
        if (flt_CurrentTime > chainData.ChainTime) {
            isSetTarget = true;
            ChainEnemy();
            flt_CurrentTime = 0;
        }
    }

    private void ChainEnemy() {
        int total_Enemy = GameManager.instance.list_ActiveEnemies.Count;
        if (total_Enemy == 0) {
            isSetTarget = false;
            return;
        }
        for (int i = 0; i < list_ChainAffected.Count; i++) {
            if (list_ChainAffected[i].TryGetComponent<EnemyHandler>(out EnemyHandler enemyHandler)) {

                enemyHandler.StopChainPowerUp();
            }
        }
        list_ChainAffected.Clear();
        if (total_Enemy > chainData.ChainCounter) {

            for (int i = 0; i < chainData.ChainCounter; i++) {

                bool isSetTarget = false;

                while (!isSetTarget) {
                    int index = Random.Range(0, total_Enemy);

                    GameObject current = GameManager.instance.list_ActiveEnemies[index].gameObject;
                    if (!list_ChainAffected.Contains(current)) {
                        list_ChainAffected.Add(current);
                        if (current.TryGetComponent<EnemyHandler>(out EnemyHandler enemyHandler)) {
                            enemyHandler.StartChainPowerUp();
                        }
                        isSetTarget = true;
                    }
                }
                
            }
        }
        else {
            for (int i = 0; i < total_Enemy; i++) {
                bool isSetTarget = false;

                while (!isSetTarget) {
                    int index = Random.Range(0, total_Enemy);

                    GameObject current = GameManager.instance.list_ActiveEnemies[index].gameObject;
                    if (!list_ChainAffected.Contains(current)) {
                        list_ChainAffected.Add(current);
                        isSetTarget = true;
                    }
                }
            }
        }
        isSetTarget = false;
      
       
    }

    private void SetChainVfxPowerUp() {
       
        flt_CurrentTime = 0;
        isSetTarget = true;
        ChainEnemy();
    }
} 
