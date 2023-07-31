using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotoveData : PowerUpData
{

    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    [SerializeField] private float flt_Damage;
    [SerializeField] private int max_Counter;


    public override void FatchMyUpdateData() {

        MolotovProperites this_Properites = AbiltyManager.instance.all_Property[myPowerIndex].
                                        GetComponent<MolotovProperites>();
        MolotovCounter = this_Properites.max_Counter[Level];
        all_MyDataDisplay[0].headerName = "Counter";
        all_MyDataDisplay[0].CurrentValue = "+" + MolotovCounter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[0].UpdateValue = "+" + this_Properites.max_Counter[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade" ;
        }


        Damage = this_Properites.flt_Damage[Level];
        all_MyDataDisplay[1].headerName = "Damage";
        all_MyDataDisplay[1].CurrentValue = "+" + Damage;

        if (Level != PowerUpHandler.instance.MaxLevelUp) {

            all_MyDataDisplay[0].UpdateValue = "+" + this_Properites.flt_Damage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade";
        }

    }
    public float Damage {
        get {
            return flt_Damage;
        }set {
            flt_Damage = value;
        }
    }

    public int MolotovCounter {
        get {
            return max_Counter;
        }set {
            max_Counter = value;
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
