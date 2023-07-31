using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullData : PowerUpData
{
    
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;



    public override void FatchMyUpdateData() {

        SkullMissileProperites this_Properites = AbiltyManager.instance.all_Property[myPowerIndex].
                                    GetComponent< SkullMissileProperites>();

        Firerate = this_Properites.flt_FireRate[Level];
        all_MyDataDisplay[0].headerName = "FireRate";
        all_MyDataDisplay[0].CurrentValue = "-" + Firerate;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].UpdateValue = "-" + this_Properites.flt_FireRate[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = " Upgrade";
        }

        Damage = this_Properites.flt_Damage[Level];
        all_MyDataDisplay[1].headerName = "Damage";
        all_MyDataDisplay[1].CurrentValue = "-" + Damage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].UpdateValue = "-" + this_Properites.flt_Damage[Level + 1];
        }
        else {
            all_MyDataDisplay[1].UpdateValue = " Upgrade";
        }

    }
    public float Firerate {
        get {
            return flt_FireRate;
        }
        private set {
            flt_FireRate = value;
        }
    }

    public float Damage {
        get {
            return flt_Damage;
        }
        private set {
            flt_Damage = value;
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
