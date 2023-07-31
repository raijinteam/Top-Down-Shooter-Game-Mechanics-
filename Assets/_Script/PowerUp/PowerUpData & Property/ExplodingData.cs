using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingData : PowerUpData
{
   
    [SerializeField] private bool isUnlocked;
    [SerializeField] private int Level;

    public delegate void SetExplodingData();
    public SetExplodingData SetData;
   



    //public float FireRate {
    //    get {
            
    //        return flt_FireRate;
    //    }
    //    set {
            
    //        flt_FireRate = value;
    //    }
    //}

    //public float Damage {
    //    get {
    //        return flt_Damage;
    //    }
    //    set {
    //        flt_Damage = value;
    //    }
    //}

    //public float Force {
    //    get {
    //        return flt_force;
    //    }
    //    set {
    //        flt_force = value;
    //    }
    //}
    //public float AreaDamage {

    //    get {
    //        return flt_AreaDamage;
    //    }
    //    set {
    //        flt_AreaDamage = value;
    //    }
    //}

    //public float AreaRange {
    //    get {
    //        return flt_AreaRange;
    //    }
    //    set {
    //        flt_AreaRange = value;
    //    }
    //}

  
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

    public override void FatchMyUpdateData() {

        ExplodingShortProperites this_Prperites = AbiltyManager.instance.all_Property[myPowerIndex].
                                                GetComponent<ExplodingShortProperites>();

        
       
       


    }

}
