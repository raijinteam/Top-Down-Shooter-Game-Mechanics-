using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPowerUp : MonoBehaviour
{

    [Header("Laser Data")]
    [SerializeField] private float flt_PowerUpTime;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Damage;
    [SerializeField] private int laserCount;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;

    [Header("Laser")]
    [SerializeField] private Transform[] startParent;
    [SerializeField] private ParticleSystem[] particle_Start;
    [SerializeField] private ParticleSystem[] particle_End;
    [SerializeField] private LineRenderer[] lineRenderer;


   
    private Transform[] all_Targets;
    private List<GameObject> list_EnemyListDuplicate = new List<GameObject>();


    private void OnEnable() {


        damageIncreased.setDamageIncreased += SetDamage;
        all_Targets = new Transform[laserCount];
        SetLaserPowerup();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_PowerUpTime);
        //StartCoroutine(UIManager.instance.uIGamePlayScreen.SetImgPowerUp(flt_PowerUpTime));

    }
    private void OnDisable() {
        damageIncreased.setDamageIncreased -= SetDamage;
    }

    private void SetDamage() {
        flt_Damage = flt_Damage + flt_Damage * PowerUpData.insatnce.damageIncreased.GetDamage * 0.01f;
    }

    private void Update() {
        
        HandleLaserActivationTime();
        FindTargetAndShootLaser();

    }

    private void SetLaserPowerup() {
        flt_CurrentTime = 0;
        SetInitialTargets();
    }


    private void FindTargetAndShootLaser() {


        for (int i = 0; i < all_Targets.Length; i++) {

            if (all_Targets[i] == null) {

                particle_Start[i].gameObject.SetActive(false);
                particle_End[i].gameObject.SetActive(false);
                lineRenderer[i].gameObject.SetActive(false);
                SetNewTargetForDeadEnemy(i);
                continue;
            }

            particle_Start[i].gameObject.SetActive(true);
            particle_End[i].gameObject.SetActive(true);
            lineRenderer[i].gameObject.SetActive(true);

            Vector3 dirction = (all_Targets[i].transform.position - transform.position).normalized;

            startParent[i].transform.LookAt(all_Targets[i]);
            Vector3 endPosition = all_Targets[i].position - dirction * 0.75f;

            particle_End[i].transform.position = endPosition;

            lineRenderer[i].SetPosition(0, particle_Start[i].transform.position);
            lineRenderer[i].SetPosition(1, particle_End[i].transform.position);
            LaserDamageWithKnockback(all_Targets[i], dirction);
        }


    }

    private void LaserDamageWithKnockback(Transform _target, Vector3 _direction) {

        if (_target.gameObject.TryGetComponent<EnemyHandler>(out EnemyHandler _movement)) {

            // _movement.KnockBack(_direction, 10f);
           
           
            _movement.SetHitByLaser(_direction, flt_Force, flt_Damage);
        }


    }

    private void HandleLaserActivationTime() {
       
        flt_CurrentTime += Time.deltaTime;

        if (flt_CurrentTime > flt_PowerUpTime) {

          
            for (int i = 0; i < all_Targets.Length; i++) {

                particle_Start[i].gameObject.SetActive(false);
                particle_End[i].gameObject.SetActive(false);
                lineRenderer[i].gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                all_Targets[i] = null;
            }

        }
    }

   

    private void SetInitialTargets() {

        if (list_EnemyListDuplicate != null) {
            list_EnemyListDuplicate.Clear();
        }

        List<GameObject> list_NewEnemyListDuplicate = new List<GameObject>();

        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            list_NewEnemyListDuplicate.Add(GameManager.instance.list_ActiveEnemies[i].gameObject);
        }

        int enemyCount = list_NewEnemyListDuplicate.Count;

        if (enemyCount > 5) {
            enemyCount = 5;
        }

        for (int i = 0; i < enemyCount; i++) {

            float currentLowestDistance = 0;

            for (int j = 0; j < list_NewEnemyListDuplicate.Count; j++) {
                float flt_CurrentDistance =
                           Mathf.Abs(Vector3.Distance(list_NewEnemyListDuplicate[j].
                                           transform.position, transform.position));

                if (currentLowestDistance == 0) {
                    currentLowestDistance = flt_CurrentDistance;
                    all_Targets[i] = list_NewEnemyListDuplicate[j].transform;

                }
                else {

                    if (currentLowestDistance > flt_CurrentDistance) {
                        currentLowestDistance = flt_CurrentDistance;
                        all_Targets[i] = list_NewEnemyListDuplicate[j].transform;


                    }
                }


            }

            list_EnemyListDuplicate.Add(all_Targets[i].gameObject);
            list_NewEnemyListDuplicate.Remove(all_Targets[i].gameObject);
        }

    }

    private void SetNewTargetForDeadEnemy(int _targetIndex) {

        List<GameObject> list_NewEnemyListDuplicate = new List<GameObject>();



        for (int i = 0; i < GameManager.instance.list_ActiveEnemies.Count; i++) {

            if (list_EnemyListDuplicate.Contains(GameManager.instance.list_ActiveEnemies[i].gameObject)) {
                continue;
            }

            list_NewEnemyListDuplicate.Add(GameManager.instance.list_ActiveEnemies[i].gameObject);
           

        }

        if (list_NewEnemyListDuplicate.Count == 0) {
            return;
        }

        float flt_Distance = 0;
        for (int i = 0; i < list_NewEnemyListDuplicate.Count; i++) {

            float flt_CurrentDistance =
                           Mathf.Abs(Vector3.Distance(list_NewEnemyListDuplicate[i].
                                           transform.position, transform.position));

            if (i == 0) {
                flt_Distance = flt_CurrentDistance;
                all_Targets[_targetIndex] = list_NewEnemyListDuplicate[i].transform;
            }
            else if (flt_CurrentDistance < flt_Distance) {
                flt_Distance = flt_CurrentDistance;
                all_Targets[_targetIndex] = list_NewEnemyListDuplicate[i].transform;
            }


        }

        list_EnemyListDuplicate.Add(all_Targets[_targetIndex].gameObject);

        if (list_NewEnemyListDuplicate != null) {
            list_NewEnemyListDuplicate.Clear();
        }

    }
}
