using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownIncreasedPowerUp : PowerUpData
{
   
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;
    [SerializeField] private int persentage_CoolDown;


    public override void FatchMyUpdateData() {

        CooldownDecreasedProperites this_Properity = AbiltyManager.instance.all_Property[myPowerIndex].
                            GetComponent<CooldownDecreasedProperites>();

        persentage_CoolDown = this_Properity.coolDownDecreasedPersantage[Level];
        all_MyDataDisplay[0].headerName = "CoolDown";
        all_MyDataDisplay[0].CurrentValue = "+" + persentage_CoolDown;
        if (Level != PowerUpHandler.instance.MaxLevelUp) {
            all_MyDataDisplay[0].UpdateValue = "+" + this_Properity.coolDownDecreasedPersantage[Level + 1];
        }
        else {
            all_MyDataDisplay[0].UpdateValue = "Upgrade";
        }

        PlayerManager.instance.Player.DecreasedPersentageCoolDown = persentage_CoolDown;
        


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
        return false;
    }
}
