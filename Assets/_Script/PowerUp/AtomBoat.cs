using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AtomBoat : MonoBehaviour
{
    [SerializeField] private int counter;
    [SerializeField] private float flt_Range;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_DetsroyTime;
    [SerializeField] private RobotMotion Robot;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SpawnBot();
        }
    }

    private void SpawnBot() {
        for (int i = 0; i < counter; i++) {
            Vector3 direction = new Vector3(Random.Range(-100,100),0,Random.Range(-100, 100)).
                                normalized;
            Vector3 targetPostion = transform.position + direction * flt_Range;
            RobotMotion rb = Instantiate(Robot,direction, Quaternion.identity);
            rb.SetRobotData(flt_Damage , flt_Force,flt_Range,flt_DetsroyTime);
        }
    }
}
