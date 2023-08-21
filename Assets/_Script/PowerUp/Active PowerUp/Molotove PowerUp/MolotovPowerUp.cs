using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MolotovPowerUp : PowerUpProperty {


    [SerializeField] private int Counter;
    [SerializeField] private float flt_Firerate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private float flt_Range;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_DamageOverTime;
    [SerializeField] private GrnadeBulletMotion monotove;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private float flt_CurrentFirerate;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [SerializeField] private CoolDownIncreasedPowerUp coolDown;

    private void OnEnable() {
        coolDown.SetCoolDown += SetCoolDownData;
        damageIncreased.setDamageIncreased += SetDamage;
    }

   

    private void OnDisable() {
        coolDown.SetCoolDown -= SetCoolDownData;
        damageIncreased.setDamageIncreased -= SetDamage;
    }

    private void SetCoolDownData() {
        flt_CurrentFirerate = flt_Firerate - flt_Firerate * 0.01f * PowerUpData.insatnce.cooldownIncreased.
                                                                        GetCurrentCoolDown;
    }
    private void SetDamage() {
        flt_Damage += 0.01f * flt_Damage * PowerUpData.insatnce.damageIncreased.GetDamage;
    }

    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.monotove.isUnlocked) {
            PowerUpData.insatnce.monotove.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.monotove.currentLevel++;
        }


        flt_DamageOverTime = PowerUpData.insatnce.monotove.GetDamageOverTime;
        flt_Firerate = PowerUpData.insatnce.monotove.GetCoolDownTime;
        Counter = PowerUpData.insatnce.monotove.GetProjecTileCounter;
        flt_CurrentFirerate = flt_Firerate;
        this.gameObject.SetActive(true);

    }
    public override int Getlevel() {
        return PowerUpData.insatnce.monotove.currentLevel;
    }


    public override void SetUI(int index) {
        Monotove Monotove = PowerUpData.insatnce.monotove;
        UnlockedInformation = Monotove.str_Description;
        int waveindex = 0;
        if (Monotove.GetDamageOverTime - Monotove.all_DamageOverTime[Monotove.currentLevel + 1] != 0) {

            this_WaveProperty[waveindex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[waveindex].CurrentPoerprtyValue = Monotove.GetDamageOverTime.ToString();
            this_WaveProperty[waveindex].NextPrpoertyValue = "+ " + MathF.Abs(Monotove.GetDamageOverTime - Monotove.all_DamageOverTime[Monotove.currentLevel + 1]);
            waveindex++;
        }

        if (Monotove.GetProjecTileCounter - Monotove.all_ProjectileCounter[Monotove.currentLevel + 1] != 0) {

            this_WaveProperty[waveindex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[waveindex].CurrentPoerprtyValue = Monotove.GetProjecTileCounter.ToString();
            this_WaveProperty[waveindex].NextPrpoertyValue = "+ " + MathF.Abs(Monotove.GetProjecTileCounter - Monotove.all_ProjectileCounter[Monotove.currentLevel + 1]);
            waveindex++;
        }
        if (Monotove.GetCoolDownTime - Monotove.all_CoolDown[Monotove.currentLevel + 1] != 0) {

            this_WaveProperty[waveindex].prpoertyName = all_Property[2].prpoertyName;
            this_WaveProperty[waveindex].CurrentPoerprtyValue = Monotove.GetCoolDownTime.ToString();
            this_WaveProperty[waveindex].NextPrpoertyValue = "- " + MathF.Abs(Monotove.GetCoolDownTime - Monotove.all_CoolDown[Monotove.currentLevel + 1]);

        }



        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(Monotove.powerUpImage,
            Monotove.currentLevel, this, this.gameObject.activeSelf);
    }
    private void Update() {
        if (!GameManager.instance.isPlayerLive) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_CurrentFirerate) {
            SpawnBullet();

            flt_CurrentTime = 0;
        }
    }



    private void SpawnBullet() {

        for (int i = 0; i < Counter; i++) {

            FireBullet();
        }


    }


    private void FireBullet() {

        GrnadeBulletMotion montovBUllet = Instantiate(monotove, spawnPostion.position, spawnPostion.rotation);

        Vector3 getRandomDirection = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;

        Vector3 targetPostion = new Vector3(transform.position.x, 0, transform.position.z) + getRandomDirection * flt_Range;

       
        montovBUllet.SetBulletData(targetPostion, flt_Damage, flt_DamageOverTime);
    }
}

