using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeData : EnemyHandler
{
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;

    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private SlimeMovement slimeMovement;
    [SerializeField] private SlimeAttacking slimeAttacking;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Collider body;
    [SerializeField] private MMF_Player spawn_MMFPlayer;

    private GameObject obj_Indicator;

    [Header("ChainVfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    [Header("Spawner")]
    [SerializeField] private SlimeData slimeData;
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;
    private SlimeData current;

    public override void SpawnEnemy() {
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

                 current = Instantiate(slimeData, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                current.SetSpawnIndicator(indicator);
                Vector3 PlayerPostion = new Vector3(PlayerManager.instance.Player.transform.position.x, flt_YTopPostion,
                                    PlayerManager.instance.Player.transform.position.z);

                current.transform.LookAt(PlayerPostion);
                Sequence seq = DOTween.Sequence();

                seq.AppendInterval(1).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5f)).
                    AppendCallback(current.DestroyIndicator);
                isSpawn = true;
            }
        }
    }

    private void DestroyIndicator() {
        Destroy(obj_Indicator);
        spawn_MMFPlayer.PlayFeedbacks();
        StartCoroutine(SetAllScriptData(0.5f));
        Instantiate(obj_Explotion, transform.position, obj_Explotion.transform.rotation);
    }

    private IEnumerator SetAllScriptData(float flt_AnimationTime) {

        yield return new WaitForSeconds(flt_AnimationTime);
        SetData();
        GameManager.instance.ADDListOfEnemy(transform);
        ExpandSpherCast();
       
    }

   

    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {

        enemyHealth.SetLaserAffacted(damage);
        slimeMovement.KnockBack(_Direction, force);
    }

    public override void StartChainPowerUp() {
        slimeMovement.SetInVisible();
        slimeAttacking.isvisible = false;
        obj_ChainVfx.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        slimeMovement.SetVisible();
        slimeAttacking.isvisible = true;
        obj_ChainVfx.gameObject.SetActive(false);
    }

    public override void SetInVisible() {
        slimeMovement.SetInVisible();
        slimeAttacking.isvisible = false;

    }
    public override void SetVisible() {
        slimeMovement.SetVisible();
        slimeAttacking.isvisible = true;
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
        slimeMovement.enabled = true;
            slimeAttacking.enabled = true;
        // navMeshAgent.enabled = true;
        body.enabled = true;

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.obj_Indicator = indicator;
    }

   
}
