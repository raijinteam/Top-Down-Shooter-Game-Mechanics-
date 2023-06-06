using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stage : MonoBehaviour
{
   

        public GameObject[] all_Enemy;
        public int[] all_Persantage; // remove this.
        public int TotalWave;
        public int totalEnemy;
        public float flt_SpawnRate = 1;

    internal void SetStageData(int minWave , int maxWave , int minEnemy , int maxEnemy) {
        totalEnemy = Random.Range(minEnemy, maxEnemy);
        TotalWave = Random.Range(minWave, maxWave);
    }
}
