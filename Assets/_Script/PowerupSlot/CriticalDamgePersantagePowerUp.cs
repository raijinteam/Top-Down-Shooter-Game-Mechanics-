using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamgePersantagePowerUp : PowerUpProperty {

    [SerializeField] private PlayerData playerData;
    public override void setPowerUpInPlayer() {

        if (!PowerUpData.insatnce.criticaDamagePersantage.isUnlocked) {
            PowerUpData.insatnce.criticaDamagePersantage.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.criticaDamagePersantage.currentLevel++;
        }
        this.gameObject.SetActive(true);
        playerData.persantage_CriticalDamageChance = PowerUpData.insatnce.criticaDamagePersantage.GetCurrentCrticalDamagePersantage;

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.criticaDamagePersantage.currentLevel;
    }

    public override void SetUI(int index) {
        criticaDamagePersantage criticalDamageChance = PowerUpData.insatnce.criticaDamagePersantage;
        UnlockedInformation = criticalDamageChance.str_Description;
        all_Property[0].CurrentPoerprtyValue = criticalDamageChance.GetCurrentCrticalDamagePersantage.ToString();

        all_Property[0].NextPrpoertyValue = "+" + criticalDamageChance.GetCurrentCrticalDamagePersantage + criticalDamageChance.all_CriticalDamagePersantage
                                        [criticalDamageChance.currentLevel + 1];


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(criticalDamageChance.powerUpImage,
            criticalDamageChance.currentLevel, this, this.gameObject.activeSelf);
    }
}
