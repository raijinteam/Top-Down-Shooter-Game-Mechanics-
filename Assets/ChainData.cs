using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainData : PowerUpData {

    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

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
