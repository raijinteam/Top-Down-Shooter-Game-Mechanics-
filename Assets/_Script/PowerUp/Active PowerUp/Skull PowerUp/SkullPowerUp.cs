using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkullPowerUp : PowerUpProperty
{


    [Header("BulletData")]

    [SerializeField] private float flt_CurrentTime;

    [Header("Bullet")]
    [SerializeField] private SkullMissileMotion Obj_Skull;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private int counter;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_CoolDownTime;
    [SerializeField] private float flt_CurrentCoolDown;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;
    [SerializeField] private DamageIncreasedPowerUp DamageIncreased;


    private void OnEnable() {
        coolDown.SetCoolDown += SetCoolDownData;
        DamageIncreased.setDamageIncreased += SetDamage;
    }
    private void OnDisable() {
        coolDown.SetCoolDown -= SetCoolDownData;
        DamageIncreased.setDamageIncreased -= SetDamage;
    }


    private void SetDamage() {
        flt_Damage += flt_Damage * 0.01f * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

   
    private void SetCoolDownData() {
        flt_CurrentCoolDown  = flt_CoolDownTime - flt_CoolDownTime * 0.01f * PowerUpData.insatnce.cooldownIncreased.
                                                                        GetCurrentCoolDown;
    }
    public override void setPowerUpInPlayer() {

        this.gameObject.SetActive(true);
        if (!PowerUpData.insatnce.microMissielData.isUnlocked) {
            PowerUpData.insatnce.microMissielData.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.microMissielData.currenrLevel++;
        }

        counter = PowerUpData.insatnce.skullMissile.GetProjectileCounter;
        flt_CoolDownTime = PowerUpData.insatnce.skullMissile.GetCurrentCoolDown;
        flt_Damage = PowerUpData.insatnce.skullMissile.GetDamage;
        flt_CurrentCoolDown = flt_CoolDownTime;
        this.gameObject.SetActive(true);



    }

    public override int Getlevel() {
        return PowerUpData.insatnce.microMissielData.currenrLevel;
    }

    public override void SetUI(int index) {

        SkullMissile skullMissile = PowerUpData.insatnce.skullMissile;
        UnlockedInformation = skullMissile.str_Description;

        int WaveIndex = 0;
        if (skullMissile.GetDamage - skullMissile.all_Damage[skullMissile.CurrentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = skullMissile.GetDamage.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + Mathf.Abs(skullMissile.GetDamage - 
                                                                        skullMissile.all_Damage[skullMissile.CurrentLevel + 1]);

            WaveIndex++;
        }
        if (skullMissile.GetCurrentCoolDown - skullMissile.all_CoolDown[skullMissile.CurrentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = skullMissile.GetCurrentCoolDown.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + MathF.Abs(skullMissile.GetCurrentCoolDown - skullMissile.all_CoolDown
                                                                                                 [skullMissile.CurrentLevel + 1]);
            WaveIndex++;
        }

        if (skullMissile.GetProjectileCounter - skullMissile.all_ProjecTileCounter[skullMissile.CurrentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[2].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = skullMissile.GetProjectileCounter.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = " + " + MathF.Abs(skullMissile.GetProjectileCounter - 
                                                                    skullMissile.all_ProjecTileCounter[skullMissile.CurrentLevel + 1]);
        }




        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(skullMissile.powerUpImage,
            skullMissile.CurrentLevel, this, this.gameObject.activeSelf);
    }

    private void Update() {


        BulletHandler();

    }

    public void SetSkullPowerUp() {

        flt_CurrentTime = 0;

    }



    private void BulletHandler() {


        flt_CurrentTime += Time.deltaTime;
       
        if (flt_CurrentTime > flt_CurrentCoolDown) {
            flt_CurrentTime = 0;

            SpawnBullet();


        }
    }

    private void SpawnBullet() {

        for (int i = 0; i < counter; i++) {
            fireBullet();
        }
       
    }

    private void fireBullet() {

        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }

        SkullMissileMotion current = Instantiate(Obj_Skull, spawnPostion.position, spawnPostion.rotation);

        int Index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);
        Transform Target = GameManager.instance.list_ActiveEnemies[Index];
        spawnPostion.LookAt(Target);
      
        current.SetBulletData(spawnPostion.forward, flt_Damage);
    }
}
