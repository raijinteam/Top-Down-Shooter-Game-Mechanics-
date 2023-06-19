using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpData : MonoBehaviour
{

    public int myPowerIndex;
    public myDataDisplay[] all_MyDataDisplay = new myDataDisplay[4];
    public abstract bool IsActivePowerUp();

    public abstract bool IsUnlocked();

    public abstract void SetUnLockedStatus();

    public abstract int PowerUpLevel();

    public abstract void SetLevelOfPowerUp();
    public abstract void FatchMyUpdateData();
}
