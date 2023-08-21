using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateDecreasedPowerUp : PowerUpProperty {

    public delegate void FireRateInceased();
    public FireRateInceased setFireRate;
    public override void setPowerUpInPlayer() {
        this.gameObject.SetActive(true);
        if (!PowerUpData.insatnce.fireRateDecreased.isUnlocked) {
            PowerUpData.insatnce.fireRateDecreased.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.fireRateDecreased.currentLevel++;
        }

        setFireRate?.Invoke();
    }
    public override int Getlevel() {
        return PowerUpData.insatnce.fireRateDecreased.currentLevel;
    }


    public override void SetUI(int index) {
        FireRateDecreased fireRateDecreased = PowerUpData.insatnce.fireRateDecreased;
        UnlockedInformation = fireRateDecreased.str_Description;
        all_Property[0].CurrentPoerprtyValue = fireRateDecreased.GetCurrentFirerate.ToString();

        all_Property[0].NextPrpoertyValue = "+" + (fireRateDecreased.GetCurrentFirerate - fireRateDecreased.all_FireRate[fireRateDecreased.currentLevel + 1]);


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(fireRateDecreased.powerUpImage,
            fireRateDecreased.currentLevel, this, this.gameObject.activeSelf);
    }
}

