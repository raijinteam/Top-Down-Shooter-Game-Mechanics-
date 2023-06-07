using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpData : MonoBehaviour
{

    public abstract bool IsActivePowerUp();

    public abstract bool IsUnlocked();

    public abstract void SetUnLockedStatus();

    public abstract int PowerUpLevel();

    public abstract void SetLevelOfPowerUp();
    
}
