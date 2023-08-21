using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderData : EnemyHandler { 
    
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;


    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private SlimeMotion spiderMotion;
    
    [SerializeField] private Collider body;
    [SerializeField] private MMF_Player spawn_MMFPlayer;




    private GameObject obj_Indicator;

    [Header("Chain VFx")]
    [SerializeField] private GameObject Obj_ChainVfx;

    [Header("Spawner")]
    [SerializeField] private SpiderData spiderData;
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;
    private SpiderData current;

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

                 current = Instantiate(spiderData, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
              
                Vector3 PlayerPostion = new Vector3(GameManager.instance.Player.transform.position.x, flt_YTopPostion,
                                    GameManager.instance.Player.transform.position.z);

                current.transform.LookAt(PlayerPostion);
                Sequence seq = DOTween.Sequence();

                seq.AppendInterval(1).Append(current.transform.DOMoveY(flt_DownPostion, 0.5f)).
                    AppendCallback(current.DestroyIndicator);
                isSpawn = true;
            }
        }
    }

    public void DestroyIndicator() {
        enemyHealth.EnemySound.Play_GroundTouchSFX();
        Destroy(obj_Indicator);
        spawn_MMFPlayer.PlayFeedbacks();
        Instantiate(obj_Explotion, transform.position, obj_Explotion.transform.rotation);
        StartCoroutine(SetAllScriptData(0.5f));
    }

    public IEnumerator SetAllScriptData(float flt_AnimationTime) {

        yield return new WaitForSeconds(flt_AnimationTime);
        SetData();
        GameManager.instance.ADDListOfEnemy(transform);
        ExpandSpherCast();
       
    }



    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {
        enemyHealth.SetLaserAffacted(damage);
        spiderMotion.KnockBack(_Direction, force);
    }

    public override void StartChainPowerUp() {
        spiderMotion.SetInVisible();
        Obj_ChainVfx.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        spiderMotion.SetVisible();
        Obj_ChainVfx.gameObject.SetActive(false);
    }

    public override void SetInVisible() {
        spiderMotion.SetInVisible();
       
    }
    public override void SetVisible() {
        spiderMotion.SetVisible();
        
    }

    public void SetAllScriptData() {

        SetData();
        GameManager.instance.ADDListOfEnemy(transform);
        ExpandSpherCast();
        Instantiate(obj_Explotion, new Vector3(transform.position.x, 0, transform.position.z), obj_Explotion.transform.rotation);
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
        spiderMotion.enabled = true;
        body.enabled = true;
       

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.obj_Indicator = indicator;
    }

   


}
