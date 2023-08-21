using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpData : MonoBehaviour {
    public static PowerUpData insatnce;
    public Explodinshot explodinshot;
    public Rehoest rehoest;
    public MicroMissielData microMissielData;
    public Monotove monotove;
    public MultyShot multyShot;
    public SkullMissile skullMissile;
    public Chain Chain;
    public DamageIncreased damageIncreased;
    public FireRateDecreased fireRateDecreased;
    public CooldownIncreased cooldownIncreased;
    public MaxHp maxHp;
    public HpRagon HpReagon;
    public CriticalDamage criticalDamage;
    public criticaDamagePersantage criticaDamagePersantage;


    private void Awake() {

        insatnce = this;
    }


    public float GetDamageIncreased(float Damage) {
        if (!damageIncreased.IsPowerActive) {
            return Damage;
        }
        else {

            float flt_CurrentDamage = Damage + Damage * damageIncreased.GetDamage * 0.01f;

            return flt_CurrentDamage;
        }
    }

}

[System.Serializable]
public struct Explodinshot {

    
    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public float[] all_Damage;
    public float[] all_AreaDamage;
    public float[] all_CoolDown;
    public int[] all_CounterProjectile;
    public int currentLevel;
    public string str_Description;

    public float GetCurrentDamage {
        get {
            return all_Damage[currentLevel];
        }
    }

    public float GetAreaDamage {
        get {
            return all_AreaDamage[currentLevel];
        }
    }
    public float GetCoolDownTime {
        get {
            return all_CoolDown[currentLevel];
        }
    }
    public int GetCounterProjecTile {
        get {

            return all_CounterProjectile[currentLevel];
        }
    }
}

[System.Serializable]
public struct Rehoest {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_Damage;
    public int[] all_Counter;
    public int currentLevel;
    public string str_Description;
    public int GetDamage {
        get {
            return all_Damage[currentLevel];
        }
    }

    public int GetCounter {
        get {
            return all_Counter[currentLevel];
        }
    }
}

[System.Serializable]
public struct MicroMissielData {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public bool isMicroMissielActive;
    public int[] counter;
    public float[] PersanatgeOfDamage;
    public string str_Description;

    public int currenrLevel;

    public int GetCurrentCounter {
        get {
            return counter[currenrLevel];
        }
    }

    public float GetCurrentPersantageOfDamage {
        get {
            return PersanatgeOfDamage[currenrLevel];
        }
    }
}


[System.Serializable]
public struct Monotove {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_ProjectileCounter;
    public float[] all_DamageOverTime;
    public float[] all_CoolDown;
    public int currentLevel;
    public string str_Description;
    public int GetProjecTileCounter {
        get {
            return all_ProjectileCounter[currentLevel];
        }
    }
    public float GetDamageOverTime {

        get {
            return all_DamageOverTime[currentLevel];
        }
    }
    public float GetCoolDownTime {
        get {
            return all_CoolDown[currentLevel];
        }
    }
}

[System.Serializable]
public struct MultyShot {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;

    public float[] all_DamagePersantage;
    public int currentLevel;
    public string str_Description;


    public float GetDamagePersantage {
        get {
            return all_DamagePersantage[currentLevel];
        }
    }


}

[System.Serializable]
public struct SkullMissile {
    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public float[] all_Damage;
    public float[] all_CoolDown;
    public int[] all_ProjecTileCounter;
    public int CurrentLevel;
    public string str_Description;

    public float GetDamage {
        get {
            return all_Damage[CurrentLevel];
        }
    }
    public float GetCurrentCoolDown {

        get {
            return all_CoolDown[CurrentLevel];
        }
    }

    public int GetProjectileCounter {
        get{
            return all_ProjecTileCounter[CurrentLevel];
        }
    }
}

[System.Serializable]
public struct Chain {

  
    public bool IsPowerActive;
    public Sprite powerUpImage;
    public float[] all_ChainTime;
    public int[] all_Counter;
    public int CurrentLevel;
    public string str_Description;
    public bool isUnlocked;

   public float GetCurrentChainTime {
        get {
            return all_ChainTime[CurrentLevel];
        }
    }
    public int GetCurrentCouneter {

        get {
            return all_Counter[CurrentLevel];
        }
    }
}

[System.Serializable]
public struct DamageIncreased {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public float[] all_Damage;
    public int CurrentLevel;
    public string str_Description;

    public float GetDamage {
        get {
            return all_Damage[CurrentLevel];
        }
    }

}

[System.Serializable]

public struct FireRateDecreased {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public float[] all_FireRate;
    public int currentLevel;
    public string str_Description;

    public float GetCurrentFirerate {
        get {
            return all_FireRate[currentLevel];
        }
    }
}
[System.Serializable]
public struct CooldownIncreased {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public float[] all_CooldownTime;
    public int currentLevel;
    public string str_Description;

    public float GetCurrentCoolDown {
        get {
            return all_CooldownTime[currentLevel];
        }
    }


}

[System.Serializable]
public struct MaxHp {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_MaxHpPersantage;
    public int currentLevel;
    public string str_Description;
    public int GetCurrentLevelHP {
        get {
            return all_MaxHpPersantage[currentLevel];
        }
    }
}


[System.Serializable]
public struct HpRagon {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_ReagonPersantage;
    public int currentLevel;
    public string str_Description;

    public int GetCurrenrHPReagonPersantage {
        get {
            return all_ReagonPersantage[currentLevel];
        }
    }
}

[System.Serializable]
public struct CriticalDamage {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_CriticalDamage;
    public int currentLevel;
    public string str_Description;

    public int GetCurrentCriticalDamage {
        get {
            return all_CriticalDamage[currentLevel];
        }
    }
}

[System.Serializable]
public struct criticaDamagePersantage {

    public bool IsPowerActive;
    public bool isUnlocked;
    public Sprite powerUpImage;
    public int[] all_CriticalDamagePersantage;
    public int currentLevel;
    public string str_Description;

    public int GetCurrentCrticalDamagePersantage {
        get {
            return all_CriticalDamagePersantage[currentLevel];
        }
    }
}



