using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyShotPowerUp : PowerUpProperty
{
    [SerializeField] private PlayerData PlayerData;

    [SerializeField] private DamageIncreasedPowerUp damageInceased;

    private void OnEnable() {

        damageInceased.setDamageIncreased += SetDamage;
        
    }

  

    private void OnDisable() {
        damageInceased.setDamageIncreased -= SetDamage;
    }

    private void SetDamage() {
        PlayerData.multyShotDamage += PlayerData.multyShotDamage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }


    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.multyShot.isUnlocked) {
            PowerUpData.insatnce.multyShot.isUnlocked = true;
            GameManager.instance.IsMultyShot = true;
        }
        else {
            PowerUpData.insatnce.multyShot.currentLevel++;
        }


        PlayerData.multyShotDamage = PlayerData.flt_Damage - PlayerData.flt_Damage*0.01f* PowerUpData.insatnce.multyShot.GetDamagePersantage;

        this.gameObject.SetActive(true);


    }
    public override int Getlevel() {
        return PowerUpData.insatnce.multyShot.currentLevel;
    }


    public override void SetUI(int index) {
        MultyShot multyShot = PowerUpData.insatnce.multyShot;
        UnlockedInformation = multyShot.str_Description;

        if (multyShot.GetDamagePersantage - multyShot.all_DamagePersantage[multyShot.currentLevel + 1] != 0) {
            this_WaveProperty[0].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[0].CurrentPoerprtyValue = multyShot.GetDamagePersantage.ToString();
            this_WaveProperty[0].NextPrpoertyValue = "+ " + Mathf.Abs(multyShot.GetDamagePersantage - multyShot.all_DamagePersantage[multyShot.currentLevel + 1]);

        }



        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(multyShot.powerUpImage,
            multyShot.currentLevel, this, this.gameObject.activeSelf);
    }
}
