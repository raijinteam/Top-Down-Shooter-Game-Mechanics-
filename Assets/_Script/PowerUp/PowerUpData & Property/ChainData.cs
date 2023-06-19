using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainData : PowerUpData {

 
    [SerializeField] private int  chainCounter;
    [SerializeField] private float flt_ChainTime;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    public override void FatchMyUpdateData() {

        ChainProperites this_Prperity = AbiltyManager.instance.all_Property[myPowerIndex].GetComponent<ChainProperites>();

        ChainCounter = this_Prperity.counter[Level];
        all_MyDataDisplay[0].headerName = "ChainCounter";
        all_MyDataDisplay[0].OldValue = "+" + chainCounter;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].newValue = "+" + this_Prperity.counter[Level + 1];
        }
        else {
            all_MyDataDisplay[0].newValue = "Upgrade";
        }

        ChainTime = this_Prperity.flt_MaxTimeAffectChain[Level];
        all_MyDataDisplay[1].headerName = "ChainTime";
        all_MyDataDisplay[1].OldValue = "+" + ChainTime;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[1].newValue = "+" + this_Prperity.flt_MaxTimeAffectChain[Level + 1];
        }
        else {
            all_MyDataDisplay[1].newValue = "Upgrade";
        }
    }
    public int ChainCounter {
        get {
            return chainCounter;
        }
        private set {

            chainCounter = value;
        }
    }

    public float ChainTime {
        get {
            return flt_ChainTime;
        }
        private set {
            flt_ChainTime = value;
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
