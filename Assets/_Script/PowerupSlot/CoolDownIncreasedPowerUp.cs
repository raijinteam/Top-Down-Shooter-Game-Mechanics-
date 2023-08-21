using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownIncreasedPowerUp : PowerUpProperty
{
    public delegate void Cooldown();
    public Cooldown SetCoolDown;

    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.cooldownIncreased.isUnlocked) {
            PowerUpData.insatnce.cooldownIncreased.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.cooldownIncreased.currentLevel++;
        }
        SetCoolDown?.Invoke();
        this.gameObject.SetActive(true);

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.cooldownIncreased.currentLevel;
    }

    public override void SetUI(int index) {

        CooldownIncreased coolDown = PowerUpData.insatnce.cooldownIncreased;
        UnlockedInformation = coolDown.str_Description;
        all_Property[0].CurrentPoerprtyValue = coolDown.GetCurrentCoolDown.ToString();

        all_Property[0].NextPrpoertyValue = "- " + (coolDown.GetCurrentCoolDown - coolDown.all_CooldownTime[coolDown.currentLevel + 1]);


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(coolDown.powerUpImage,
            coolDown.currentLevel, this, this.gameObject.activeSelf);
    }
}

