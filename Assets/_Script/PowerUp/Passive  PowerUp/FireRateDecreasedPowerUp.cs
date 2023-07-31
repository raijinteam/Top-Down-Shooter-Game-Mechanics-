using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateDecreasedPowerUp : PowerUpData
{
   
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private int persantage_Firerate;


    public override void FatchMyUpdateData() {

        fireRateDecreasedProperites this_Properity = AbiltyManager.instance.all_Property[myPowerIndex].
                            GetComponent<fireRateDecreasedProperites>();

        persantage_Firerate = this_Properity.flt_FireRatePersantage[Level];
        all_MyDataDisplay[0].headerName = "FireRate";
        all_MyDataDisplay[0].CurrentValue = "+" + persantage_Firerate;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].UpdateValue = "+" + this_Properity.flt_FireRatePersantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade";
        }

        PlayerManager.instance.Player.DecreasedPersentageFireRate = persantage_Firerate;
        


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
