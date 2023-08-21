using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class BatData : EnemyHandler
{
    [Header("All Script Campaotant")]
    [SerializeField] private BatData sting;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BatMovement batMovement;
  
    [SerializeField] private Collider bodyCollider;





    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_Chain;

    [Header("Spawner")]
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;
    private BatData current_Sting;
    private GameObject spawn_Indicator;
    [SerializeField] private MMF_Player spawn_MMFPlayer;

    public override void SpawnEnemy() {




        float flt_YTopPostion = 100;

        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(LevelManager.instance.flt_BoundryX_,
                LevelManager.instance.flt_BoundryX), flt_YTopPostion,
                Random.Range(LevelManager.instance.flt_BoundryZ_, LevelManager.instance.flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                current_Sting = Instantiate(sting, postion, transform.rotation);

                current_Sting.SetSpawnIndicator(indicator);
                Vector3 PlayerPostion = new Vector3(GameManager.instance.Player.transform.position.x, flt_YTopPostion,
                                    GameManager.instance.Player.transform.position.z);

                current_Sting.transform.LookAt(PlayerPostion);
                Sequence seq = DOTween.Sequence();

                seq.AppendInterval(1).Append(current_Sting.transform.DOMoveY(flt_DownPostion, 0.5f))
                    .Append(current_Sting.transform.DOMoveY(2, 0.5f)).
                    AppendCallback(current_Sting.DestroyIndicator);
                isSpawn = true;
            }
        }
    }

    private void DestroyIndicator() {
        enemyHealth.EnemySound.Play_GroundTouchSFX();
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
      
        obj_Chain.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        batMovement.SetVisible();
     
        obj_Chain.gameObject.SetActive(false);
    }


    public override void SetInVisible() {
        batMovement.setInvisible();
      
    }
    public override void SetVisible() {
        batMovement.SetVisible();
      
    }


    private void SetData() {

        enemyHealth.enabled = true;
        batMovement.enabled = true;
      
        bodyCollider.enabled = true;

    }
}
