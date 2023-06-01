using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MolotovPowerUp : MonoBehaviour
{
    // private float flt_FireRate;
    // private float currentTimePassed
    // private int molotovCount;
    


  
    [SerializeField] private bool isPowerUpStart;

    [Header("Bullet Data")]
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    
    [SerializeField] private float flt_DestroyTime;
    [SerializeField] private int counter;
    [SerializeField] private float MinRange;
    [SerializeField] private float maxRange;
    [SerializeField] private float flt_DelayBetweenTwoBullet;
    [SerializeField] private bool isSpawnbullet;

    [Header("Bullet")]
    [SerializeField] private GameObject obj_Grenade;
    [SerializeField] private Transform spawnPostion;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetMolotovPowerUp();
        }
        BulletHandler();
      
       
    }

    private void SetMolotovPowerUp() {
        isPowerUpStart = true;
        flt_CurrentTime = 0;
        isSpawnbullet = false;
    }

   

    private void BulletHandler() {
        if (!isPowerUpStart && isSpawnbullet) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_FireRate) {
           
            isSpawnbullet = true;
            StartCoroutine(Spawnbullet());

        }
    }

    private IEnumerator Spawnbullet() {
        for (int i = 0; i < counter; i++) {

            GameObject current = Instantiate(obj_Grenade, spawnPostion.position, spawnPostion.rotation);

            Vector3 direction = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
            float distance = Random.Range(MinRange, maxRange);
            Vector3 taregtPostion = transform.position + direction * distance;

            current.GetComponent<GrnadeBulletMotion>().SetBulletData(taregtPostion, flt_Damage,
                             flt_DestroyTime);

            yield return new WaitForSeconds(flt_DelayBetweenTwoBullet);
        }

        flt_CurrentTime = 0;
        isSpawnbullet = false;
    }
}
