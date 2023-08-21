using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChainPowerUp : PowerUpProperty
{


    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private List<GameObject> list_ChainAffected;
    [SerializeField] private bool isSetTarget;

    private float flt_ChainTime;
    private int Counter;


   
    public override void setPowerUpInPlayer() {

      
        if (!PowerUpData.insatnce.Chain.isUnlocked) {
            PowerUpData.insatnce.Chain.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.Chain.CurrentLevel++;
        }

        Counter = PowerUpData.insatnce.Chain.GetCurrentCouneter;
        flt_ChainTime = PowerUpData.insatnce.Chain.GetCurrentChainTime;
        this.gameObject.SetActive(true);



    }

    public override int Getlevel() {
        return PowerUpData.insatnce.microMissielData.currenrLevel;
    }

    public override void SetUI(int index) {

        Chain Chain = PowerUpData.insatnce.Chain;       
        UnlockedInformation = Chain.str_Description;

        int WaveIndex = 0;
        if (Chain.GetCurrentChainTime - Chain.all_ChainTime[Chain.CurrentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = Chain.GetCurrentChainTime.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + (Chain.GetCurrentChainTime - Chain.all_ChainTime[Chain.CurrentLevel + 1]);

            WaveIndex++;
        }
        if (Chain.GetCurrentCouneter - Chain.all_Counter[Chain.CurrentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = Chain.GetCurrentCouneter.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + MathF.Abs(Chain.GetCurrentCouneter - Chain.all_Counter[Chain.CurrentLevel + 1]);
        
        }

      



        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(Chain.powerUpImage,
            Chain.CurrentLevel, this, this.gameObject.activeSelf);
    }


    private void Update() {

        ChainHanadler();
    }

    private void ChainHanadler() {
        if (isSetTarget) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_ChainTime) {
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
            if (list_ChainAffected[i] == null) {
                continue;
            }
            if (list_ChainAffected[i].TryGetComponent<EnemyHandler>(out EnemyHandler enemyHandler)) {

                enemyHandler.StopChainPowerUp();
            }
        }
        list_ChainAffected.Clear();
        if (total_Enemy > Counter) {

            for (int i = 0; i < Counter; i++) {

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
