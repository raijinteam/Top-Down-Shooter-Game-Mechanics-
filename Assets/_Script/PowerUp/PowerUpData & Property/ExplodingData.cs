using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingData : PowerUpData
{
   
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    [Header("Properites")]
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;
    [SerializeField] private float flt_AreaDamage;
    [SerializeField] private float flt_AreaRange;
   



    public float FireRate {
        get {
            
            return flt_FireRate;
        }
        set {
            
            flt_FireRate = value;
        }
    }

    public float Damage {
        get {
            return flt_Damage;
        }
        set {
            flt_Damage = value;
        }
    }

    public float Force {
        get {
            return flt_force;
        }
        set {
            flt_force = value;
        }
    }
    public float AreaDamage {

        get {
            return flt_AreaDamage;
        }
        set {
            flt_AreaDamage = value;
        }
    }

    public float AreaRange {
        get {
            return flt_AreaRange;
        }
        set {
            flt_AreaRange = value;
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

    public override void FatchMyUpdateData() {

        ExplodingShortProperites this_Prperites = AbiltyManager.instance.all_Property[myPowerIndex].
                                                GetComponent<ExplodingShortProperites>();

        FireRate = this_Prperites.all_Level_Firerate[Level];
        all_MyDataDisplay[0].headerName = "Firerate";
        all_MyDataDisplay[0].OldValue = FireRate.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "-" + this_Prperites.all_Level_Firerate[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }
        

        Damage = this_Prperites.all_Level_Damage[Level];
        all_MyDataDisplay[1].headerName = "Damage";
        all_MyDataDisplay[1].OldValue = "+" + Damage.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].newValue = "+" + this_Prperites.all_Level_Damage[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[1].newValue = "Upgrade";
        }


        Force = this_Prperites.all_Level_Force[Level];
        all_MyDataDisplay[2].headerName = "Force";
        all_MyDataDisplay[2].OldValue = "+" + Force.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[2].newValue = this_Prperites.all_Level_AreaForce[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[2].newValue = "Upgrade";
        }

        AreaDamage = this_Prperites.all_Level_AreaDamage[Level];
        all_MyDataDisplay[3].headerName = "AreaDamage";
        all_MyDataDisplay[3].OldValue = "+" + AreaDamage.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[3].newValue = this_Prperites.all_Level_AreaDamage[Level + 1].ToString();
        }
        else {
            all_MyDataDisplay[2].newValue = "Upgrade";
        }

        AreaRange = this_Prperites.all_Level_AreaForce[Level];


    }

}
