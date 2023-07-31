using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageIncreasedPowerUp : PowerUpData
{

    [SerializeField] private int criticalDamagePesantage;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private PlayerData playerData;

    public int CriticalDamage {
        get {
            return criticalDamagePesantage;
        }
        private set {
            criticalDamagePesantage = value;
        }
    }

    public override void FatchMyUpdateData() {
        criticalDamagePrperites this_Property = AbiltyManager.instance.all_Property[myPowerIndex].
                        GetComponent<criticalDamagePrperites>();

        CriticalDamage = this_Property.all_Level_Critical_Damage_Persantage[Level];
        all_MyDataDisplay[0].headerName = "Critical Damage";
        all_MyDataDisplay[0].CurrentValue = "+" + criticalDamagePesantage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].UpdateValue = "+" + this_Property.all_Level_Critical_Damage_Persantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade";
        }

        playerData.persantage_CriticalDamage = CriticalDamage;

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
        return false;
    }
}
