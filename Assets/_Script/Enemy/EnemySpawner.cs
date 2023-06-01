using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstckle_Layer;
    [SerializeField]private GameObject obj_Indiacter;
    [SerializeField]private float flt_Boundry;
    [SerializeField]private float flt_BoundryX;
    [SerializeField]private float flt_BoundryZ;
    [SerializeField] private SkeletonData Skelton;
    [SerializeField] private EvileMageData evileMage;
    [SerializeField] private orcEnemyData orcEnemy;
    [SerializeField] private GolemData GolemEnemy;
    [SerializeField] private SlimeData slimeEnemy;
    [SerializeField] private SpiderData spiderEnemy;
    [SerializeField] private GameObject Bat;
    [SerializeField] private GameObject dragen;
    [Header("Plant")]
    [SerializeField] private MonsterPlantData obj_Plant;
    [SerializeField] private float flt_Scale;
    [SerializeField] private float flt_MaxScale;
   

    public void SpawnSkeleton() {

         float flt_YTopPostion = 100;
          float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion, 
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion,Vector3.down, 1000,obstckle_Layer)) {
               GameObject indicator =  Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z), 
                                                obj_Indiacter.transform.rotation);

                SkeletonData current = Instantiate(Skelton, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;
                

                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
       
       
    }

    public void SpawnEnemyMage() {

        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                EvileMageData current = Instantiate(evileMage, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }
    public void SpawnOrcEnemy() {

        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                orcEnemyData current = Instantiate(orcEnemy, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }

    public void SpawnMonsterPlant() {
       
        float flt_YDownPostion = 1.5f;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YDownPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));


            if (!Physics.Raycast(postion, Vector3.down, 10000, obstckle_Layer)) {
              GameObject Indicator =   Instantiate(obj_Indiacter, new Vector3(postion.x, 0, 
                    postion.z), obj_Indiacter.transform.rotation);

                MonsterPlantData current = Instantiate(obj_Plant, postion,transform.rotation);
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
    public void SpawnGolemEnemy() {

        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                GolemData current = Instantiate(GolemEnemy, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }

    public void SpawnSlimeEnemy() {

        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                SlimeData current = Instantiate(slimeEnemy, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }

    public void SpawnSpiderEnemy() {
        float flt_YTopPostion = 100;
        float flt_YDownPostion = 2;
        bool isSpawn = false;
        while (!isSpawn) {
            Vector3 postion = new Vector3(Random.Range(flt_Boundry, flt_BoundryX), flt_YTopPostion,
                Random.Range(flt_Boundry, flt_BoundryZ));

            if (!Physics.Raycast(postion, Vector3.down, 1000, obstckle_Layer)) {
                GameObject indicator = Instantiate(obj_Indiacter, new Vector3(postion.x, 0, postion.z),
                                                 obj_Indiacter.transform.rotation);

                SpiderData current = Instantiate(spiderEnemy, postion, transform.rotation);

                current.SetSpawnIndicator(indicator);
                float flt_CurrentScale = current.transform.localScale.y;
                float flt_AnimatScale = flt_CurrentScale - 0.3f;


                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(0.5F).Append(current.transform.DOMoveY(flt_YDownPostion, 0.5F)).
                    AppendCallback(current.DestroyIndicator).
                    Append(current.transform.DOScaleY(flt_AnimatScale, 0.5F)).
                    Append(current.transform.DOScaleY(flt_CurrentScale, 0.5F))
                        .AppendCallback(current.SetAllScriptData);
                current.transform.rotation = Quaternion.identity;
                isSpawn = true;
            }
        }
    }

    public void SpawnBatEnemy() {

        SpawnAndTargetPostion spawnAndTarget = GetFlyIngEnemyPostion();

        Vector3 SpawnPostion = spawnAndTarget.startPostion;
        Vector3 targetPostion = spawnAndTarget.EndPostion;
        GameObject currentBat = Instantiate(Bat, SpawnPostion, Quaternion.identity);

        currentBat.transform.LookAt(PlayerManager.instance.Player.transform);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(currentBat.transform.DOMove(targetPostion, 1).SetEase(Ease.Linear)).
            AppendCallback(currentBat.GetComponent<BatData>().SetAllScriptData);
       ;
    }
    public void SpawnDragon() {

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

    private SpawnAndTargetPostion GetFlyIngEnemyPostion() {

        SpawnAndTargetPostion spawnAndTarget = new SpawnAndTargetPostion();
        int postionY = 4;
        int Range = 50;
        Vector3 postion = Vector3.zero;
        int Index =  Random.Range(0,100);
        
        if (Index<=25) {
            spawnAndTarget.startPostion = new Vector3(Random.Range(Range, LevelManager.instance.flt_Boundry), postionY,
                            Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));
            spawnAndTarget.EndPostion = new Vector3(LevelManager.instance.flt_Boundry,
                                    spawnAndTarget.startPostion.y, spawnAndTarget.startPostion.z);
        }
        else if (Index > 25 && Index<=50) {
            spawnAndTarget.startPostion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryX), postionY,
                           Random.Range(-Range, LevelManager.instance.flt_BoundryZ));
            spawnAndTarget.EndPostion = new Vector3(spawnAndTarget.startPostion.x,
                                    spawnAndTarget.startPostion.y, LevelManager.instance.flt_BoundryZ);
        }
        else if (Index>50 &&Index<75) {
            spawnAndTarget.startPostion = new Vector3(Random.Range(-Range, LevelManager.instance.flt_BoundryX), postionY,
                           Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));

            spawnAndTarget.EndPostion = new Vector3(LevelManager.instance.flt_BoundryX,
                                  spawnAndTarget.startPostion.y, spawnAndTarget.startPostion.z);

        }
        else {
            spawnAndTarget.startPostion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryX), postionY,
                          Random.Range(Range, LevelManager.instance.flt_Boundry));

            spawnAndTarget.EndPostion = new Vector3(spawnAndTarget.startPostion.x,
                                 spawnAndTarget.startPostion.y, LevelManager.instance.flt_Boundry);
        }
        return spawnAndTarget;
    }

    

   
}


public struct SpawnAndTargetPostion{

    public Vector3 startPostion;
    public Vector3 EndPostion;
}
