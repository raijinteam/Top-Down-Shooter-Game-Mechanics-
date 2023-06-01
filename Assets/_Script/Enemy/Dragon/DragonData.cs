using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonData : EnemyHandler
{
    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private DraganShooting draganShooting;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    public int damage;
    public float flt_knockBackForce;


    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {
        enemyHealth.SetLaserAffacted(damage);
    }
    public override void StartChainPowerUp() {
        batMovement.setInvisible();
        draganShooting.setInvisible();
        obj_ChainVfx.SetActive(true);
    }
    public override void StopChainPowerUp() {
        batMovement.SetVisible();
        draganShooting.SetVisible();
        obj_ChainVfx.SetActive(false);
    }
    public override void SetInVisible() {
        batMovement.setInvisible();
        draganShooting.setInvisible();
    }
    public override void SetVisible() {
        batMovement.SetVisible();
        draganShooting.SetVisible();
    }

    public void SetAllScriptData() {

        SetData();
        LevelManager.instance.ADDListOfEnemy(this.gameObject);

    }

    public void SetData() {
        enemyHealth.enabled = true;
        batMovement.enabled = true;
        draganShooting.enabled = true;


    }
}
