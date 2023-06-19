using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvileMageData : EnemyHandler
{
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;
    private GameObject Obj_Indicator;

    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EivileMageShooting evileMageShooting;
    [SerializeField] private EvileMageTrigger evileMageTrigger;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private EvileMageMovement evileMageMovement;
    [SerializeField] private Collider body;

    private EvileMageData current;

   [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    [Header("Spawner")]
    [SerializeField] private EvileMageData evileMage;
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;



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

                 current = Instantiate(evileMage, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).AppendCallback(ScaleAnimation).AppendInterval(0.5f)
                    .AppendCallback(current.SetAllScriptData);

                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }

    private void ScaleAnimation() {
        FeelManager.instance.PlayScaleAnimation(current.transform);
    }
    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {

        enemyHealth.SetLaserAffacted(damage);
        evileMageMovement.EveileKnockback(_Direction, force);
    }

    public override void StartChainPowerUp() {
        evileMageMovement.SetInVisible();
        evileMageShooting.SetInVisible();
        obj_ChainVfx.gameObject.SetActive(true);
    }
    public override void StopChainPowerUp() {
        evileMageMovement.SetVisible();
        evileMageShooting.SetVisible();
        obj_ChainVfx.gameObject.SetActive(true);
    }

    public override void SetInVisible() {
        evileMageMovement.SetInVisible();
        evileMageShooting.SetInVisible();
    }
    public override void SetVisible() {
        evileMageMovement.SetVisible();
        evileMageShooting.SetVisible();
    }

    public void SetAllScriptData() {
       
        SetData();
        GameManager.instance.ADDListOfEnemy(transform);
        ExpandSpherCast();
        Instantiate(obj_Explotion, transform.position, obj_Explotion.transform.rotation);
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
        evileMageShooting.enabled = true;
        evileMageTrigger.enabled = true;
        evileMageMovement.enabled = true;
        body.enabled = true;
        // navMeshAgent.enabled = true;

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.Obj_Indicator = indicator;
    }

    public void DestroyIndicator() {
        Destroy(Obj_Indicator);
    }
}
