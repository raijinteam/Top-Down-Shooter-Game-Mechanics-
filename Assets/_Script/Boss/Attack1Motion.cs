using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack1Motion : MonoBehaviour {

    [SerializeField] private BossMovement boss;
    [SerializeField] private float flt_StartTimeToIdleBattel;
    [SerializeField] [Range(1.2f, 5)] private float flt_DelayBetweenTwoAttack;
    [SerializeField] private float flt_SpawnSpikeTime;
    [SerializeField] private int totalAttack;
    [SerializeField] private Transform spikeSpawnPostion;

    [SerializeField] private Vector3 targetPostion;
    [SerializeField] private float flt_offest;
    [SerializeField] private float flt_DistancebetweenTwoSpike;
    [SerializeField] private GameObject PlaySwordTuchVfx;
    [SerializeField] private float flt_Range;
    [SerializeField] private LayerMask PlayerLayer;

    private void OnEnable() {
        boss.Attack1CallBack += Attack1Start;
    }
    private void OnDisable() {
        boss.Attack1CallBack -= Attack1Start;
    }

    private void Attack1Start() {
        StartCoroutine(WeaponAttack());
    }

    private IEnumerator WeaponAttack() {


        yield return new WaitForSeconds(flt_StartTimeToIdleBattel);
        Debug.Log("WaponAttack");
        for (int i = 0; i < totalAttack; i++) {


            boss.ChangeAnimation(boss.bossAnimationName.Attack1);      
            Debug.Log("WaponAttackAnimation Start");
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SpawnSpike());
            boss.SetIdleAnimation();
            yield return new WaitForSeconds(2);
        }

        boss.AttackEnded();

    }

    private IEnumerator SpawnSpike() {

        Vector3 direction = Vector3.zero;
        Vector3 spawnPostion = Vector3.zero;

        PLaySworVFx();
        SphereCastNearSword();

        if (boss.isVisible) {
            direction = (GameManager.instance.Player.transform.position - spikeSpawnPostion.position).normalized;
            targetPostion = GameManager.instance.Player.transform.position + direction * flt_offest;
            spawnPostion = spikeSpawnPostion.position + direction * flt_offest;

            float flt_Disatnce = Mathf.Abs(Vector3.Distance(spikeSpawnPostion.position, targetPostion));
            float scaleMultiplier = 0f;

            while (flt_Disatnce > 1) {

                SpikePooler.instance.PlayPasrtcleVfx(spawnPostion, boss.damage, boss.force, scaleMultiplier);
                spawnPostion += direction * flt_DistancebetweenTwoSpike;
                scaleMultiplier += 0.1f;
                flt_Disatnce = Mathf.Abs(Vector3.Distance(spawnPostion, targetPostion));
                yield return new WaitForSeconds(0.1f);
            }
        }
        //else {
        //    direction = transform.forward;
        //    targetPostion = spikeSpawnPostion.position + direction * 20;
        //    spawnPostion = spikeSpawnPostion.position + direction * flt_offest;

        //}

      
       
       


        



    }

    private void SphereCastNearSword() {
        Collider[] all_Collider = Physics.OverlapSphere(spikeSpawnPostion.position, flt_Range, PlayerLayer);

        for (int i = 0; i < all_Collider.Length; i++) {

            if (all_Collider[i].TryGetComponent<CollisionHandling>(out CollisionHandling player)) {

                Vector3 direction = (player.transform.position - spikeSpawnPostion.position).normalized;
                float flt_Distance = Mathf.Abs(Vector3.Distance(player.transform.position, spikeSpawnPostion.position));
                if (flt_Distance < 0.5f) {

                    direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
                }

                player.SetHitByNormalBullet(boss.damage, boss.force, direction);
                
            }
        }
    }

    public void PLaySworVFx() {
        Instantiate(PlaySwordTuchVfx, spikeSpawnPostion.position, transform.rotation);

    }
}
