using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatData : EnemyHandler
{


    [Header("All Script Campaotant")]
    [SerializeField] private GameObject bat;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private BatShootnig batShootnig;

   

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_Chain;



    public override void SpawnEnemy() {

        SpawnAndTargetPostion spawnAndTarget = GetFlyIngEnemyPostion();

        Vector3 SpawnPostion = spawnAndTarget.startPostion;
        Vector3 targetPostion = spawnAndTarget.EndPostion;
        GameObject currentBat = Instantiate(bat, SpawnPostion, Quaternion.identity);

        currentBat.transform.LookAt(PlayerManager.instance.Player.transform);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentBat.transform.DOMove(targetPostion, 1).SetEase(Ease.Linear)).
            AppendCallback(currentBat.GetComponent<BatData>().SetAllScriptData);
        ;
    }
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
        GameManager.instance.ADDListOfEnemy(transform);
      
    }

    public void SetData() {
        enemyHealth.enabled = true;
        batMovement.enabled = true;
        batShootnig.enabled = true;


    }
}
