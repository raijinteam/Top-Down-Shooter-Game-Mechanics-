using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageChancessPowerUp : PowerUpData
{
 
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private int percentage_Of_CritaclaDamage;
    public int Percentage_CriticalDamage {
        get {
            return percentage_Of_CritaclaDamage;
        }
        private set {
            percentage_Of_CritaclaDamage = value;
        }
    }

    public override void FatchMyUpdateData() {
        CriticalDamageChancessProperites this_Property = AbiltyManager.instance.all_Property[myPowerIndex].
                        GetComponent<CriticalDamageChancessProperites>();

        Percentage_CriticalDamage = this_Property.all_Critical_Damage_Chancess_Persantage[Level];
        all_MyDataDisplay[0].headerName = " % Critical Damage";
        all_MyDataDisplay[0].OldValue = "+" + Percentage_CriticalDamage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Property.all_Critical_Damage_Chancess_Persantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }

        playerData.persantage_CriticalDamage = Percentage_CriticalDamage;



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
