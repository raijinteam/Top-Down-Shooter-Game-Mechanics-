using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AstrlArrowPowerUp : MonoBehaviour {
    [SerializeField] private float flt_CurrentTime;

    private float flt_Boundry;
    private float flt_BoundrX;
    private float flt_BoundryZ;


    [SerializeField] private int No_OfArrow_Spawn;
    [SerializeField] private float flt_Damage;
   
    [SerializeField] private Arrow arrow;

    private void Start() {
        flt_Boundry = LevelManager.instance.flt_Boundry;
        flt_BoundrX = LevelManager.instance.flt_BoundryX;
        flt_BoundryZ = LevelManager.instance.flt_BoundryZ;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SpawnArrow();
        }
    }

    private void SpawnArrow() {
        for (int i = 0; i < No_OfArrow_Spawn; i++) {
            Arrow current = Instantiate(arrow, GetRandomPostion(), arrow.transform.rotation);
            arrow.SetBulletData(flt_Damage);
        }
        
    }

    private Vector3 GetRandomPostion() {
        float flt_YPostion = 20;
        float x = Random.Range(flt_Boundry, flt_BoundrX);
        float z = Random.Range(flt_Boundry, flt_BoundryZ);

        return new Vector3(x, flt_YPostion, z);
    }
}
