using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpProperty : MonoBehaviour
{
    public bool isActivePowerUp;
    public PropetyContainer[] all_Property;
    public PropetyContainer[] this_WaveProperty;
    public string UnlockedInformation;

    public abstract void SetUI(int index);
    public abstract int Getlevel();

    public abstract void setPowerUpInPlayer();

}

[System.Serializable]
public struct PropetyContainer {


    public string prpoertyName;
    public string CurrentPoerprtyValue;
    public string NextPrpoertyValue;
}

