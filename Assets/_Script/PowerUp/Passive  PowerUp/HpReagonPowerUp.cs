using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpReagonPowerUp : PowerUpProperty {

    [SerializeField] private PlayerHealth playerHealth;

    public override void setPowerUpInPlayer() {

        if (!PowerUpData.insatnce.HpReagon.isUnlocked) {
            PowerUpData.insatnce.HpReagon.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.HpReagon.currentLevel++;
        }
        playerHealth.IncresedHPReagon(PowerUpData.insatnce.HpReagon.GetCurrenrHPReagonPersantage);
        this.gameObject.SetActive(true);

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
