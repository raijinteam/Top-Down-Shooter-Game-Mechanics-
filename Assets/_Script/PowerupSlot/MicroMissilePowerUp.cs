using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissilePowerUp : PowerUpProperty
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private DamageIncreasedPowerUp DamageIncreased;



    private void OnEnable() {

        DamageIncreased.setDamageIncreased += SetDamage;
    }

    private void OnDisable() {

        DamageIncreased.setDamageIncreased -= SetDamage;
    }

    private void SetDamage() {
        playerData.flt_MissileDamage += 0.01f * playerData.flt_MissileDamage * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    public override void setPowerUpInPlayer() {

        this.gameObject.SetActive(true);
        if (!PowerUpData.insatnce.microMissielData.isUnlocked) {
            PowerUpData.insatnce.microMissielData.isUnlocked = true;
            GameManager.instance.IsMicroMissile = true;
        }
        else {
            PowerUpData.insatnce.microMissielData.currenrLevel++;
        }

        playerData.MissileCounter = PowerUpData.insatnce.microMissielData.GetCurrentCounter;
        playerData.flt_MissileDamage = playerData.flt_Damage - playerData.flt_Damage*0.01f* PowerUpData.insatnce.microMissielData.GetCurrentPersantageOfDamage;




    }

    public override int Getlevel() {
        return PowerUpData.insatnce.microMissielData.currenrLevel;
    }

    public override void SetUI(int index) {

        MicroMissielData micromissile = PowerUpData.insatnce.microMissielData;
        UnlockedInformation = micromissile.str_Description;

        int WaveIndex = 0;
        if (micromissile.GetCurrentPersantageOfDamage - micromissile.PersanatgeOfDamage[micromissile.currenrLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = micromissile.GetCurrentPersantageOfDamage.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + Mathf.Abs(micromissile.GetCurrentPersantageOfDamage -
                                                                                micromissile.PersanatgeOfDamage[micromissile.currenrLevel + 1]);

            WaveIndex++;
        }
        if (micromissile.GetCurrentCounter - micromissile.counter[micromissile.currenrLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = micromissile.GetCurrentCounter.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + MathF.Abs(micromissile.GetCurrentCounter - micromissile.counter[micromissile.currenrLevel + 1]);
        }




        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(micromissile.powerUpImage,
            micromissile.currenrLevel, this, this.gameObject.activeSelf);
    }
}
