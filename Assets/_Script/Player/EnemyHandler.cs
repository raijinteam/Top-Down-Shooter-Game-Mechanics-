using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract  class EnemyHandler : MonoBehaviour
{

    public float flt_DownPostion;
    
    public abstract void SetHitByLaser(Vector3 _Direction , float force,float damage);

    public abstract void SetInVisible();

    public abstract void SetVisible();
    public abstract void StopChainPowerUp();
    public abstract void StartChainPowerUp();
    public abstract void SpawnEnemy();

    //public SpawnAndTargetPostion GetFlyIngEnemyPostion() {

    //    SpawnAndTargetPostion spawnAndTarget = new SpawnAndTargetPostion();
    //    int postionY = 4;
    //    int Range = 50;
    //    Vector3 postion = Vector3.zero;
    //    int Index = Random.Range(0, 100);

    //    if (Index <= 25) {
    //        spawnAndTarget.startPostion = new Vector3(Random.Range(Range, LevelManager.instance.flt_BoundryX), postionY,
    //                        Random.Range(LevelManager.instance.flt_BoundryZ_, LevelManager.instance.flt_BoundryZ));
    //        spawnAndTarget.EndPostion = new Vector3(LevelManager.instance.flt_BoundryX,
    //                                spawnAndTarget.startPostion.y, spawnAndTarget.startPostion.z);
    //    }
    //    else if (Index > 25 && Index <= 50) {
    //        spawnAndTarget.startPostion = new Vector3(Random.Range(LevelManager.instance.flt_BoundryX_, LevelManager.instance.flt_BoundryX), postionY,
    //                       Random.Range(-Range, LevelManager.instance.flt_BoundryZ_));
    //        spawnAndTarget.EndPostion = new Vector3(spawnAndTarget.startPostion.x,
    //                                spawnAndTarget.startPostion.y, LevelManager.instance.flt_BoundryZ_);
    //    }
    //    else if (Index > 50 && Index < 75) {
    //        spawnAndTarget.startPostion = new Vector3(Random.Range(-Range, LevelManager.instance.flt_BoundryX_), postionY,
    //                       Random.Range(LevelManager.instance.flt_BoundryZ_, LevelManager.instance.flt_BoundryZ));

    //        spawnAndTarget.EndPostion = new Vector3(LevelManager.instance.flt_BoundryX_,
    //                              spawnAndTarget.startPostion.y, spawnAndTarget.startPostion.z);

    //    }
    //    else {
    //        spawnAndTarget.startPostion = new Vector3(Random.Range(LevelManager.instance.flt_BoundryX_, LevelManager.instance.flt_BoundryX), postionY,
    //                      Random.Range(Range, LevelManager.instance.flt_BoundryZ));

    //        spawnAndTarget.EndPostion = new Vector3(spawnAndTarget.startPostion.x,
    //                             spawnAndTarget.startPostion.y, LevelManager.instance.flt_BoundryZ);
    //    }
    //    return spawnAndTarget;
    //}
}

public struct SpawnAndTargetPostion {

    public Vector3 startPostion;
    public Vector3 EndPostion;
}
