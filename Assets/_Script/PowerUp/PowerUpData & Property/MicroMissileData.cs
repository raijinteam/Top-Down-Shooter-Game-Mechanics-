using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissileData : PowerUpData {



    [SerializeField] private PlayerData playerData;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;


    public override void FatchMyUpdateData() {

        MicroMissileProperites this_Properity = AbiltyManager.instance.all_Property[myPowerIndex].
                                        GetComponent<MicroMissileProperites>();

        playerData.MissileCounter = this_Properity.counter[Level];
        all_MyDataDisplay[0].headerName = "MissleCounter";
        all_MyDataDisplay[0].OldValue = "+" + playerData.MissileCounter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Properity.counter[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }


        playerData.flt_MissileDamage = this_Properity.missle_Damage[Level];
        all_MyDataDisplay[1].headerName = "Damage";
        all_MyDataDisplay[1].OldValue = "+" + playerData.flt_MissileDamage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].newValue = "+" + this_Properity.missle_Damage[Level + 1];
        }
        else {
            all_MyDataDisplay[1].newValue = "Upgrade";
        }

        playerData.PersantageOfMissileSpawn = this_Properity.persantage_ToSpawn_MicroMissile[Level];
        all_MyDataDisplay[2].headerName = "%_Spawn";
        all_MyDataDisplay[2].OldValue = "+" + playerData.PersantageOfMissileSpawn;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].newValue = "+" + this_Properity.persantage_ToSpawn_MicroMissile[Level + 1];
        }
        else {
            all_MyDataDisplay[1].newValue = "Upgrade";
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
