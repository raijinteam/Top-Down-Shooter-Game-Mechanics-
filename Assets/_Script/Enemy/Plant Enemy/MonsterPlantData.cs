using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlantData : EnemyHandler
{
    [SerializeField] private int flt_MinKnockBackForce;
    [SerializeField] private int flt_MaxKnockBackForce;
    [SerializeField] private float flt_KnockBackForce;
    [SerializeField] private float flt_RangeOfSpheareCast;
    [SerializeField] private GameObject obj_Explotion;

    [Header("All Script Campaotant")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private MonsterPlantShootine monsterPlantShootine;
    private GameObject obj_Indicator;

    [Header("Chain Vfx")]
    [SerializeField] private GameObject obj_ChainVfx;

    [Header("Spawner")]
    [SerializeField] private MonsterPlantData obj_Plant;
    [SerializeField] private float flt_Scale;
    [SerializeField] private float flt_MaxScale;
    [SerializeField] private float flt_Boundry;
    [SerializeField] private float flt_BoundryX;
    [SerializeField] private float flt_BoundryZ;
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField] private GameObject obj_Indiacter;


    public override void SpawnEnemy() {

        float flt_YDownPostion = 1.5f;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry,
               LevelManager.instance.flt_BoundryX), flt_YDownPostion,
               Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));


            if (!Physics.Raycast(postion, Vector3.down, 10000, obstckle_Layer)) {
                GameObject Indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0,
                      postion.z), obj_Indiacter.transform.rotation);

                MonsterPlantData current = Instantiate(obj_Plant, postion, transform.rotation);
                current.transform.localScale = new Vector3(flt_Scale, 0, flt_Scale);
                current.SetSpawnIndicator(Indicator);
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(2).AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_MaxScale, 0.5f)).
                    Append(current.transform.DOScaleY(flt_Scale, 0.25f))

                    .AppendCallback(current.GetComponent<MonsterPlantData>().SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }
    public override void SetHitByLaser(Vector3 _Direction, float force, float damage) {

        enemyHealth.SetLaserAffacted(damage);
        monsterPlantShootine.PlantKnockBack(_Direction, force);
    }

    public override void StartChainPowerUp() {
        monsterPlantShootine.SetInVisible();
        obj_ChainVfx.gameObject.SetActive(true);
    }

    public override void StopChainPowerUp() {
        monsterPlantShootine.SetVisible();
        obj_ChainVfx.gameObject.SetActive(false);
            
    }

    public override void SetInVisible() {

        monsterPlantShootine.SetInVisible();
    }
    public override void SetVisible() {

        monsterPlantShootine.SetVisible();
    }


    public void SetAllScriptData() {

        SetData();
        LevelManager.instance.ADDListOfEnemy(this.gameObject);
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
        monsterPlantShootine.enabled = true;
      
        

    }

    public void SetSpawnIndicator(GameObject indicator) {
        this.obj_Indicator = indicator;
    }

    public void DestroyIndicator() {
        Destroy(obj_Indicator);
    }
}
