using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTrigger : MonoBehaviour
{
   
    public abstract void SethitByBullet(float flt_Damage, float _flt_Force, Vector3 _Direction);

    public abstract void HitByBlackHole(Transform transform); // Hit By Black Hole
   
    public abstract void BlackHoleBlast(); // BlackHole Blast
    public abstract void SethitByAura(float flt_Damage, float flt_Force, Vector3 direction);
   

    public abstract void SetHitTidalWave(Transform _Target);
    public abstract void StopHitTidalWave();

    public abstract void SetHitbyMolotovePowerUp(float flt_Damage);

    public abstract void StopMolotovePowerUp();
    public abstract void SetDamageBlast();
    public abstract void SetHitByTerrorShot(float flt_Damage, float flt_Force);
    public abstract void SetHitOrbitBullet(float flt_Damage, float flt_Force, Vector3 direction);
}
