using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonData : EnemyHandler
{
    [Header("All Script Campaotant")]
    [SerializeField] private GameObject dragen;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private DraganShooting draganShooting;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    public int damage;
    public float flt_knockBackForce;

    public override void SpawnEnemy() {
        SpawnAndTargetPostion spawnAndTarget = GetFlyIngEnemyPostion();


        Vector3 SpawnPostion = spawnAndTarget.startPostion;
        Vector3 targetPostion = spawnAndTarget.EndPostion;
        GameObject currentBat = Instantiate(dragen, SpawnPostion, Quaternion.identity);

        currentBat.transform.LookAt(PlayerManager.instance.Player.transform);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentBat.transform.DOMove(targetPostion, 1).SetEase(Ease.Linear)).
            AppendCallback(currentBat.GetComponent<DragonData>().SetAllScriptData);
        ;
    }

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
