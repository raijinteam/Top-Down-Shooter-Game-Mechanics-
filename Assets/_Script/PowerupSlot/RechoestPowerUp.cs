using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechoestPowerUp : PowerUpProperty
{
    [SerializeField] private PlayerData playerData;

    



    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.rehoest.isUnlocked) {
            PowerUpData.insatnce.rehoest.isUnlocked = true;
            GameManager.instance.IsRechoest = true;
        }
        else {
            PowerUpData.insatnce.rehoest.currentLevel++;
        }

        playerData.Rechoest_damagePersantage = PowerUpData.insatnce.rehoest.GetDamage;
        playerData.RechoestCounter = PowerUpData.insatnce.rehoest.GetCounter;

        this.gameObject.SetActive(true);



    }
    public override int Getlevel() {
        return PowerUpData.insatnce.rehoest.currentLevel;
    }

    public override void SetUI(int index) {

        Rehoest rehoest = PowerUpData.insatnce.rehoest;
        UnlockedInformation = rehoest.str_Description;

        int waveIndex = 0;
        if (rehoest.GetDamage - rehoest.all_Damage[rehoest.currentLevel + 1] != 0) {

            this_WaveProperty[waveIndex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[waveIndex].CurrentPoerprtyValue = rehoest.GetDamage.ToString();
            this_WaveProperty[waveIndex].NextPrpoertyValue = " + " + MathF.Abs(rehoest.GetDamage - rehoest.all_Damage[rehoest.currentLevel + 1]);
            waveIndex++;
        }
        if (rehoest.GetCounter - rehoest.all_Counter[rehoest.currentLevel + 1] != 0) {

            this_WaveProperty[waveIndex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[waveIndex].CurrentPoerprtyValue = rehoest.GetCounter.ToString();
            this_WaveProperty[waveIndex].NextPrpoertyValue = " + " + MathF.Abs(rehoest.GetCounter - rehoest.all_Counter[rehoest.currentLevel + 1]);
            waveIndex++;
        }




        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(rehoest.powerUpImage,
            rehoest.currentLevel, this, this.gameObject.activeSelf);
    }

}
