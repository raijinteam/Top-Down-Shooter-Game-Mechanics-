using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MirrorAttack : MonoBehaviour
{
    [Header("Mirror Data")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private CloneMirrorAttackPlayer obj_MirrorAttackPlayer;
    [SerializeField] private CloneMirrorAttackPlayer2 obj_MirrorAttackPlayer2;
    [SerializeField] private Transform[] all_MirrorPostion;
    [SerializeField] private int IndexOfMirrorAttack; // cloneCount
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDownIncresed;
    
    [SerializeField] private float flt_CurrentPowerTime;
    [SerializeField] private float flt_MaxPowerUpTime;

    private void OnEnable() {
        SetMirrorAttackPowerUp();
        UIManager.instance.uIGamePlayScreen.ShowPowerUpTimer(flt_MaxPowerUpTime);
    }
    private void Update() {
       

        MirrorAttackPowerUpHandler();
    }

    private void MirrorAttackPowerUpHandler() {
       
        flt_CurrentPowerTime += Time.deltaTime;

        if (flt_CurrentPowerTime > flt_MaxPowerUpTime) {
            this.gameObject.SetActive(false);
        }
    }

    public void SetMirrorAttackPowerUp() {
        
        flt_CurrentPowerTime = 0;
       // SpawnMirrorAttackPlayer();
        SpawnMirrorAttackPlayer2();
    }

    private void SpawnMirrorAttackPlayer2() {
        for (int i = 0; i < IndexOfMirrorAttack; i++) {
            Vector3 postion = new Vector3(Random.Range(-12, 12), transform.position.y, Random.Range(-12, 12));


            CloneMirrorAttackPlayer2 Current = Instantiate(obj_MirrorAttackPlayer2, postion, transform.rotation);
              

            Current.SetMirrorPlayerData(flt_MaxPowerUpTime, playerData.flt_Damage, playerData.flt_Force,
                playerData.flt_Firerate);
        }
    }

    private void SpawnMirrorAttackPlayer() {
        for (int i = 0; i < IndexOfMirrorAttack; i++) {

           CloneMirrorAttackPlayer Current =  Instantiate(obj_MirrorAttackPlayer, all_MirrorPostion[i].transform.position
               , transform.rotation);

            float flt_Damage = playerData.flt_Damage;
            float flt_FireRate = playerData.flt_Firerate;

            if (damageIncreased.gameObject.activeSelf) {

                flt_Damage += 0.01f * flt_Damage * PowerUpData.insatnce.damageIncreased.GetDamage;
            }
            if (coolDownIncresed.gameObject.activeSelf) {

                flt_FireRate -= 0.01f * flt_FireRate * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown;
            }

            Current.SetMirrorPlayerData(flt_MaxPowerUpTime,flt_Damage,playerData.flt_Force,
                flt_FireRate);
        }
    }
}
