using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            yield return new WaitForSeconds(2);

        }

        boss.ChangeState(BossState.idle);

    }

    private IEnumerator SpawnSpike() {

        Vector3 direction = (GameManager.instance.Player.transform.position - spikeSpawnPostion.position).normalized;
        targetPostion = GameManager.instance.Player.transform.position;
        Vector3 spawnPostion = spikeSpawnPostion.position + direction * flt_offest;
        float flt_Disatnce = Mathf.Abs(Vector3.Distance(spikeSpawnPostion.position, targetPostion));
        float scaleMultiplier = 0f;
        PLaySworVFx();

        while (flt_Disatnce > 1.5f) {

            SpikePooler.instance.PlayPasrtcleVfx(spawnPostion,boss.damage,boss.force, scaleMultiplier);
            spawnPostion += direction * flt_DistancebetweenTwoSpike;
            scaleMultiplier += 0.1f;
            flt_Disatnce = Mathf.Abs(Vector3.Distance(spawnPostion, targetPostion));
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PLaySworVFx() {
        Instantiate(PlaySwordTuchVfx, spikeSpawnPostion.position, transform.rotation);

    }
}
