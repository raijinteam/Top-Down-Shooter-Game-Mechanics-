using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonData : EnemyHandler
{
    [Header("All Script Campaotant")]
    [SerializeField] private DragonData dragen;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
    [SerializeField] private DraganShooting draganShooting;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    public int damage;
    public float flt_knockBackForce;

    [Header("Spawner")]
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;
    private DragonData current_Dragen;
    private GameObject spawn_Indicator;
    [SerializeField] private MMF_Player spawn_MMFPlayer;

    public override void SpawnEnemy() {

        //SpawnAndTargetPostion spawnAndTarget = GetFlyIngEnemyPostion();

        //Vector3 SpawnPostion = spawnAndTarget.startPostion;
        //Vector3 targetPostion = spawnAndTarget.EndPostion;
        //GameObject currentBat = Instantiate(bat, SpawnPostion, Quaternion.identity);

        //currentBat.transform.LookAt(PlayerManager.instance.Player.transform);

        //Sequence sequence = DOTween.Sequence();

        //sequence.Append(currentBat.transform.DOMove(targetPostion, 1).SetEase(Ease.Linear)).
        //    AppendCallback(currentBat.GetComponent<BatData>().SetAllScriptData);
        //;


        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry,
                LevelManager.instance.flt_BoundryX), flt_YTopPostion,
                Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                current_Dragen = Instantiate(dragen, postion, transform.rotation);

                current_Dragen.SetSpawnIndicator(indicator);
                Vector3 PlayerPostion = new Vector3(PlayerManager.instance.Player.transform.position.x, flt_YTopPostion,
                                    PlayerManager.instance.Player.transform.position.z);

                current_Dragen.transform.LookAt(PlayerPostion);
                Sequence seq = DOTween.Sequence();

                seq.AppendInterval(1).Append(current_Dragen.transform.DOMoveY(flt_YDownPostion, 0.5f))
                    .Append(current_Dragen.transform.DOMoveY(5, 0.5f)).
                    AppendCallback(current_Dragen.DestroyIndicator);
                isSpawn = true;
            }
        }
    }

    private void DestroyIndicator() {
        Destroy(spawn_Indicator);
        spawn_MMFPlayer.PlayFeedbacks();
        StartCoroutine(SetAllScriptData(0.5f));

    }

    public IEnumerator SetAllScriptData(float flt_AnimationTime) {

        yield return new WaitForSeconds(flt_AnimationTime);
        SetData();
        GameManager.instance.ADDListOfEnemy(transform);

    }

    public void SetSpawnIndicator(GameObject _Indicator) {
        this.spawn_Indicator = _Indicator;
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
        GameManager.instance.ADDListOfEnemy(transform);

    }

    public void SetData() {
        enemyHealth.enabled = true;
        batMovement.enabled = true;
        draganShooting.enabled = true;


    }
}
