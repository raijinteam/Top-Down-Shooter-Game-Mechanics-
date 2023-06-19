using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpPowerUp : PowerUpData
{
    [SerializeField] private PlayerHealth PlayerHealth;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private float maxHp;

    public float MaxHp {
        get {
            return maxHp;
        }
        set {
            maxHp = value;
        }
    }


    public override void FatchMyUpdateData() {
        MaxHpProperites this_Property = AbiltyManager.instance.all_Property[myPowerIndex].
                      GetComponent<MaxHpProperites>();

        MaxHp = this_Property.all_Level_MaxHP_Persantage[Level];
        all_MyDataDisplay[0].headerName = " % HP";
        all_MyDataDisplay[0].OldValue = "+" + MaxHp;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Property.all_Level_MaxHP_Persantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }

        PlayerHealth.flt_MaxHealth = PlayerHealth.flt_MaxHealth + (PlayerHealth.flt_MaxHealth * MaxHp * 0.01f);
        PlayerHealth.IncreasedMaxHealth(PlayerHealth.flt_MaxHealth);
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
