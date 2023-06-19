using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncresedPowerUp : PowerUpData
{
   
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    [SerializeField] private int damagePesantage;


   public int DamagePersantage {
        get {
            return damagePesantage;
        }
        private set {
            damagePesantage = value;
        }
    }
    

    public override void FatchMyUpdateData() {

        DamageIncreasedProperites this_Properity = AbiltyManager.instance.all_Property[myPowerIndex].
                                    GetComponent<DamageIncreasedProperites>();

        damagePesantage = this_Properity.damage_Increased_Persantage[Level];
        all_MyDataDisplay[0].headerName = "% Damage";
        all_MyDataDisplay[0].OldValue = "+" + damagePesantage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Properity.damage_Increased_Persantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade" ;
        }

       PlayerManager.instance.Player.damageIncreasePercent = damagePesantage;

       
        
        
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
