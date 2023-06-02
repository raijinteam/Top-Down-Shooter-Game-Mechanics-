using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract  class EnemyHandler : MonoBehaviour
{
    public abstract void SetHitByLaser(Vector3 _Direction , float force,float damage);

    public abstract void SetInVisible();

    public abstract void SetVisible();
    public abstract void StopChainPowerUp();
    public abstract void StartChainPowerUp();
    public abstract void SpawnEnemy();

    public SpawnAndTargetPostion GetFlyIngEnemyPostion() {

        SpawnAndTargetPostion spawnAndTarget = new SpawnAndTargetPostion();
        int postionY = 4;
        int Range = 50;
        Vector3 postion = Vector3.zero;
        int Index = Random.Range(0, 100);

        if (Index <= 25) {
            spawnAndTarget.startPostion = new Vector3(Random.Range(Range, LevelManager.instance.flt_Boundry), postionY,
                            Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryZ));
            spawnAndTarget.EndPostion = new Vector3(LevelManager.instance.flt_Boundry,
                                    spawnAndTarget.startPostion.y, spawnAndTarget.startPostion.z);
        }
        else if (Index > 25 && Index <= 50) {
            spawnAndTarget.startPostion = new Vector3(Random.Range(LevelManager.instance.flt_Boundry, LevelManager.instance.flt_BoundryX), postionY,
                           Random.Range(-Range, LevelManager.instance.flt_BoundryZ));
            spawnAndTarget.EndPostion = new Vector3(spawnAndTarget.startPostion.x,
                                    spawnAndTarget.startPostion.y, LevelManager.instance.flt_BoundryZ);
        }
        else if (Index > 50 && Index < 75) {
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

public struct SpawnAndTargetPostion {

    public Vector3 startPostion;
    public Vector3 EndPostion;
}
