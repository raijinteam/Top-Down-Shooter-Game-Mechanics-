using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultyShotData : PowerUpData
{
  
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    [SerializeField] private int counter;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_FireRate;

    public override void FatchMyUpdateData() {
        MultyShotProperites this_Property = AbiltyManager.instance.all_Property[myPowerIndex].
                            GetComponent<MultyShotProperites>();

        Max_Counter = this_Property.max_Counter[Level];
        all_MyDataDisplay[0].headerName = "Counter";
        all_MyDataDisplay[0].OldValue = "+" + Max_Counter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[0].newValue = "+" +  this_Property.max_Counter[Level + 1];
        }

        FireRate = this_Property.flt_FireRate[Level];
        all_MyDataDisplay[1].headerName = "FireRate";
        all_MyDataDisplay[1].OldValue = "+" + FireRate;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[1].newValue = "+" + this_Property.flt_FireRate[Level + 1];
        }

        Damage = this_Property.flt_Damage[Level];
        all_MyDataDisplay[2].headerName = "Damage";
        all_MyDataDisplay[2].OldValue = "+" + Damage;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[2].newValue = "+" + this_Property.flt_Damage[Level + 1];
        }

        Force  = this_Property.flt_Force[Level];
        all_MyDataDisplay[3].headerName = "Damage";
        all_MyDataDisplay[3].OldValue = "+" + Force;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[3].newValue = "+" + this_Property.flt_Force[Level + 1];
        }

    }

    public int Max_Counter {
        get {
            return counter;
        }
        private set {
            counter = value;
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
    public float FireRate {
        get{
            return flt_FireRate;
        }
        private set {
            flt_FireRate = value;
        }
    }

    public float Force {
        get {
            return flt_Force;
        }
        private set {
            flt_Force = value;
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


[System.Serializable]
public struct myDataDisplay {

    public string headerName;
    public string OldValue;
    public string newValue;
}
