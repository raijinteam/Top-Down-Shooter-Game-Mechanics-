using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundTuchVfx : MonoBehaviour
{
    [SerializeField] private GameObject obj_TouchVfx;
    [SerializeField] private Transform spawnLeftPostion;
    [SerializeField] private Transform spawnRightPostion;

    public void SpwnVfx() {
        Instantiate(obj_TouchVfx, spawnLeftPostion.position, transform.rotation);
        Instantiate(obj_TouchVfx, spawnRightPostion.position, transform.rotation);
    }
 }
