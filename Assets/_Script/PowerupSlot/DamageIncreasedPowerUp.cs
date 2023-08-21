using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncreasedPowerUp : PowerUpProperty {


    public delegate void damgeIncresed();
    public damgeIncresed setDamageIncreased;
    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.damageIncreased.isUnlocked) {
            PowerUpData.insatnce.damageIncreased.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.damageIncreased.CurrentLevel++;
        }

        setDamageIncreased?.Invoke();
        this.gameObject.SetActive(true);

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.damageIncreased.CurrentLevel;
    }

    public override void SetUI(int index) {
        DamageIncreased DamageIncreased = PowerUpData.insatnce.damageIncreased;
        UnlockedInformation = DamageIncreased.str_Description;
        all_Property[0].CurrentPoerprtyValue = DamageIncreased.GetDamage.ToString();

        all_Property[0].NextPrpoertyValue = "+" + (DamageIncreased.GetDamage - DamageIncreased.all_Damage[DamageIncreased.CurrentLevel + 1]);




        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(DamageIncreased.powerUpImage,
            DamageIncreased.CurrentLevel, this, this.gameObject.activeSelf);
    }
}
