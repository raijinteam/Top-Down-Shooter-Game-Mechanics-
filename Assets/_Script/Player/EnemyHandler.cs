using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class EnemyHandler : MonoBehaviour
{
    public abstract void SetHitByLaser(Vector3 _Direction , float force,float damage);

    public abstract void SetInVisible();

    public abstract void SetVisible();
    public abstract void StopChainPowerUp();
    public abstract void StartChainPowerUp();
}
