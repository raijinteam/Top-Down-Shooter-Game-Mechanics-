using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpReagonPowerUp : PowerUpData
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private float flt_HealthPersantage;
    [SerializeField] private float flt_HealthIncreasedTime;
    [SerializeField] private float flt_Curentime;

    private void Update() {


        flt_Curentime += Time.deltaTime;
        if (flt_Curentime > HealthIncreasedTime) {
            playerHealth.IncresedHPReagon(HealthPersantage);
        }
        
    }
    public override void FatchMyUpdateData() {

        HpReagonProperites this_Property = AbiltyManager.instance.all_Property[myPowerIndex].
                        GetComponent<HpReagonProperites>();

        HealthPersantage = this_Property.all_Level_HpReagon_Persantage[Level];
        all_MyDataDisplay[0].headerName = "% HPReagon";
        all_MyDataDisplay[0].OldValue = HealthPersantage.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Property.all_Level_HpReagon_Persantage[Level +1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }

        HealthIncreasedTime = this_Property.all_Level_HpReagon_Time[Level];
        all_MyDataDisplay[0].headerName = " HPReagon Time";
        all_MyDataDisplay[0].OldValue = HealthIncreasedTime.ToString();
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Property.all_Level_HpReagon_Time[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }



    }
    public float HealthPersantage {
        get {
            return flt_HealthPersantage;
        }private set {
            flt_HealthPersantage = value;
        }
    }

    public float HealthIncreasedTime {
        get {
            return flt_HealthIncreasedTime;
        } 
        private set {
            flt_HealthIncreasedTime = value;
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
        return false;
    }
}
