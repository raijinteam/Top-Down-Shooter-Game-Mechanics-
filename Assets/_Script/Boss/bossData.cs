using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class bossData : EnemyHandler {

    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;
    private GameObject spawn_Indicator;

    [Header("All Script Campaotant")]
    [SerializeField] private bossData skeletonData;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private BossMovement bossMotion;
    [SerializeField] private Attack2 attack;
    [SerializeField] private Attack1Motion attack1;
   
    [SerializeField] private Collider body;
    [SerializeField] private MMF_Player spawn_MMFPlayer;



    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    [Header("Spawner")]
   
  
    [SerializeField] private GameObject obj_Indiacter;
    private bossData current;

    public override void SpawnEnemy() {
        float flt_YTopPostion = 100;
      

        GameObject indicator = Instantiate(obj_Indiacter, new Vector3(skeletonData.transform.position.x, 0, skeletonData.transform.position.z),
                                                  obj_Indiacter.transform.rotation);

        current = Instantiate(skeletonData, new Vector3(skeletonData.transform.position.x, 100, skeletonData.transform.position.z), transform.rotation);

        current.SetSpawnIndicator(indicator);
        Vector3 PlayerPostion = new Vector3(GameManager.instance.Player.transform.position.x, flt_YTopPostion,
                            GameManager.instance.Player.transform.position.z);

        current.transform.LookAt(PlayerPostion);
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(1).Append(current.transform.DOMoveY(flt_DownPostion, 0.5f)).
            AppendCallback(current.DestroyIndicator);
    }

    private void DestroyIndicator() {
        enemyHealth.EnemySound.Play_GroundTouchSFX();
        Destroy(spawn_Indicator);
        spawn_MMFPlayer.PlayFeedbacks();
        StartCoroutine(SetAllScriptData(0.5f));
        Instantiate(obj_Explotion, new Vector3(transform.position.x, 0, transform.position.z), obj_Explotion.transform.rotation);
    }
    private IEnumerator SetAllScriptData(float flt_AnimationTime) {

        yield return new WaitForSeconds(flt_AnimationTime);
        FeelManager.instance.PLayZoomCamera();
        yield return new WaitForSeconds(9);

        SetData();
        GameManager.instance.ADDListOfEnemy(transform);
        ExpandSpherCast();

    }



    public override void SetHitByLaser(Vector3 _Dircetion, float _force, float _damage) {
        bossMotion.KnockBack(_Dircetion, _force);
        enemyHealth.SetLaserAffacted(_damage);
    }

    public override void StopChainPowerUp() {
      
    }
    public override void StartChainPowerUp() {
       
    }
    public override void SetInVisible() {
        bossMotion.SetInVisible();
       
    }
    public override void SetVisible() {
        bossMotion.SetVisible();
       
    }




    public void SetSpawnIndicator(GameObject _Indicator) {
        this.spawn_Indicator = _Indicator;
    }


    private void ExpandSpherCast() {
        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_RangeOfSpheareCast);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].TryGetComponent<PlayerMovement>(out PlayerMovement playermovemet)) {

                float distance = Mathf.Abs(Vector3.Distance(transform.position, all_Collider[i].transform.position));
                flt_KnockBackForce = ((flt_MinKnockBackForce - flt_MaxKnockBackForce) / flt_RangeOfSpheareCast) * distance +
                    flt_MaxKnockBackForce;
                Vector3 knockBackDirection = (transform.position - all_Collider[i].transform.position).normalized;
                if (knockBackDirection == Vector3.zero) {
                    Vector3 randomDirection = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)).normalized;
                    knockBackDirection = randomDirection;
                }
                playermovemet.KnockBack(-knockBackDirection, flt_KnockBackForce);
            }
        }
    }
    public void SetData() {
        enemyHealth.enabled = true;
        bossMotion.enabled = true;

        attack.enabled = true;
        attack1.enabled = true;
        body.enabled = true;
       

    }
}
