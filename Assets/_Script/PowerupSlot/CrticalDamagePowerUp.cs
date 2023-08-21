using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrticalDamagePowerUp : PowerUpProperty {


    [SerializeField] private PlayerData playerData;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;

    private void OnEnable() {
        damageIncreased.setDamageIncreased += SetDamage;
    }

    private void OnDisable() {
        damageIncreased.setDamageIncreased += SetDamage;
    }

    private void SetDamage() {

        playerData.persantage_CriticalDamage += 0.01f * playerData.persantage_CriticalDamage * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.criticalDamage.isUnlocked) {
            PowerUpData.insatnce.criticalDamage.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.criticalDamage.currentLevel++;
        }
        playerData.persantage_CriticalDamage = PowerUpData.insatnce.criticalDamage.GetCurrentCriticalDamage;
        this.gameObject.SetActive(true);

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.criticalDamage.currentLevel;
    }

    public override void SetUI(int index) {
        CriticalDamage coolDown = PowerUpData.insatnce.criticalDamage;
        UnlockedInformation = coolDown.str_Description;
        all_Property[0].CurrentPoerprtyValue = coolDown.GetCurrentCriticalDamage.ToString();

        all_Property[0].NextPrpoertyValue = "+ " + (coolDown.GetCurrentCriticalDamage + coolDown.all_CriticalDamage[coolDown.currentLevel + 1]);


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(coolDown.powerUpImage,
            coolDown.currentLevel, this, this.gameObject.activeSelf);
    }

}
