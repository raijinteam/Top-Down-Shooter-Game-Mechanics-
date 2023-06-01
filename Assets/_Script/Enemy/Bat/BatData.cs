using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatData : EnemyHandler
{
   

    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private BatShootnig batShootnig;

    public int damage;
    public float flt_knockBackForce;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_Chain;

    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {
        enemyHealth.SetLaserAffacted(damage);
       
    }

    public override void StartChainPowerUp() {
        batMovement.setInvisible();
        batShootnig.setInvisible();
        obj_Chain.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        batMovement.SetVisible();
        batShootnig.SetVisible();
        obj_Chain.gameObject.SetActive(false);
    }


    public override void SetInVisible() {
        batMovement.setInvisible();
        batShootnig.setInvisible();
    }
    public override void SetVisible() {
        batMovement.SetVisible();
        batShootnig.SetVisible();
    }
    public void SetAllScriptData() {

        SetData();
        LevelManager.instance.ADDListOfEnemy(this.gameObject);
      
    }

    public void SetData() {
        enemyHealth.enabled = true;
        batMovement.enabled = true;
        batShootnig.enabled = true;


    }
}
