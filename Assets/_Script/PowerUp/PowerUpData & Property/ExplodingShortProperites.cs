using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingShortProperites : MonoBehaviour
{

    public Property[] all_Property;   // 0 Damage , 1 Area Damage , 2 CoolDown , 3 ProjecTileCount
    [SerializeField] private int CurrentLevel;



    public int GetCurrentLevel {
        get {
            return CurrentLevel;
        }
    }

    public float GetCurrentLevelDamage {
        get {
            return all_Property[0].all_PropetyValue[CurrentLevel];
        }
    }

    public float GetThisLevelDamage(int i) {
        return all_Property[0].all_PropetyValue[i];
    }
    public float GetCurrentLevelAreaDamage {
        get {
            return all_Property[1].all_PropetyValue[CurrentLevel];
        }
    }
    public float GetThisLevelAreaDamage(int i) {
        return all_Property[1].all_PropetyValue[i];
    }
    public float GetCurrentLevelCoolDown {

        get {
            return all_Property[2].all_PropetyValue[CurrentLevel];
        }
    }
    public float GetThisLevelCoolDown(int i) {
        return all_Property[2].all_PropetyValue[i];
    }
    public float GetCurrentlevelProjecTileCounter {
        get {

            return all_Property[3].all_PropetyValue[CurrentLevel];
        }
    }
    public float GetThisLevelProjecTileCount(int i) {

        return all_Property[3].all_PropetyValue[i];
    }






}


[System.Serializable]
public struct Property {

    public string propertyName;
    public float[] all_PropetyValue;
}
