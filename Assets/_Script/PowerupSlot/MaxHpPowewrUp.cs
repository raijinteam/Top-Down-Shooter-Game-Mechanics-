using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpPowewrUp : PowerUpProperty {

    [SerializeField] PlayerHealth playerHealth;
    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.maxHp.isUnlocked) {
            PowerUpData.insatnce.maxHp.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.maxHp.currentLevel++;
        }

        this.gameObject.SetActive(true);

        playerHealth.flt_MaxHealth = playerHealth.flt_MaxHealth +
                            (playerHealth.flt_MaxHealth * PowerUpData.insatnce.maxHp.GetCurrentLevelHP * 0.01f);
        playerHealth.IncreasedMaxHealth(playerHealth.flt_MaxHealth);

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.maxHp.currentLevel;
    }

    public override void SetUI(int index) {
        MaxHp maxhp = PowerUpData.insatnce.maxHp;
        UnlockedInformation = maxhp.str_Description;
        all_Property[0].CurrentPoerprtyValue = maxhp.GetCurrentLevelHP.ToString();

        all_Property[0].NextPrpoertyValue = "+ " + (maxhp.GetCurrentLevelHP + maxhp.all_MaxHpPersantage[maxhp.currentLevel + 1]);


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(maxhp.powerUpImage,
            maxhp.currentLevel, this, this.gameObject.activeSelf);
    }
}
