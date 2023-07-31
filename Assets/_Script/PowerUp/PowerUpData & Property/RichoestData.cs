using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichoestData : PowerUpData {


    [SerializeField] private PlayerData playerData;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    


    public override void FatchMyUpdateData() {

        RichoestProperites this_Properity = AbiltyManager.instance.all_Property[myPowerIndex].
                                GetComponent<RichoestProperites>();

        playerData.RechoestCounter = this_Properity.max_Counter[Level];
        all_MyDataDisplay[0].headerName = "MaxCounter";
        all_MyDataDisplay[0].CurrentValue = "+" + playerData.RechoestCounter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].UpdateValue = "+" + this_Properity.max_Counter[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade";
        }


        playerData.Rechoest_damagePersantage = this_Properity.damage_Persantage_Decreased[Level];
        all_MyDataDisplay[1].headerName = "damage";
        all_MyDataDisplay[1].CurrentValue = "-" + playerData.RechoestCounter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].UpdateValue = "-" + this_Properity.damage_Persantage_Decreased[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[1].UpdateValue = "Upgrade";
        }

    }
    public override bool IsUnlocked() {
        return isUnlocked;
    }
    public override void SetUnLockedStatus() {
        isUnlocked = true;
    }
    public override int PowerUpLevel() {
        return Level;
    }

    public override void SetLevelOfPowerUp() {
        Level++;
    }
    public override bool IsActivePowerUp() {
        return true;
    }
}
