using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class explodingShot : PowerUpProperty
{
    [Header("Bullet - Data")]
    [SerializeField] private Transform bulletSpawnpostion;
    [SerializeField] private ExplodingBulletMotion obj_Bullet;
    [SerializeField] private float flt_CurrentTimeForSpawnBullet;
    [SerializeField] private CoolDownIncreasedPowerUp coolDownPowerUp;
    [SerializeField] private DamageIncreasedPowerUp damageIncreased;
    [Header("Properites")]
    [SerializeField] private float flt_FireRate;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_force;
    [SerializeField] private float flt_AreaDamage;
    [SerializeField] private float flt_AreaRange;
    [SerializeField] private int ProjectileCounter;
    [SerializeField] private float flt_CurrentCoolDownTime;

    private void OnEnable() {

        coolDownPowerUp.SetCoolDown += SetCoolDown;
        damageIncreased.setDamageIncreased += SetDamage;
    }

   

    private void OnDisable() {

        coolDownPowerUp.SetCoolDown -= SetCoolDown;
        damageIncreased.setDamageIncreased -= SetDamage;
    }

    private void SetCoolDown() {

        flt_CurrentCoolDownTime = flt_CurrentCoolDownTime - flt_CurrentCoolDownTime * PowerUpData.insatnce.cooldownIncreased.GetCurrentCoolDown * 0.01f;
    }

    private void SetDamage() {
        flt_Damage = flt_Damage - flt_Damage * PowerUpData.insatnce.damageIncreased.GetDamage * 0.01f;
    }

    public override void SetUI(int index) {
        Explodinshot explode = PowerUpData.insatnce.explodinshot;

        UnlockedInformation = explode.str_Description;

        int WaveIndex = 0;
        if (explode.GetCurrentDamage - explode.all_Damage[explode.currentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[0].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = explode.GetCurrentDamage.ToString();
            all_Property[WaveIndex].NextPrpoertyValue = "+ " + MathF.Abs(explode.GetCurrentDamage - explode.all_Damage[explode.currentLevel + 1]);
            WaveIndex++;
        }
        if (explode.GetAreaDamage - explode.all_AreaDamage[explode.currentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[1].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = explode.GetAreaDamage.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = "+ " + MathF.Abs(explode.GetAreaDamage - explode.all_AreaDamage[explode.currentLevel + 1]);
            WaveIndex++;
        }
        if (explode.GetCoolDownTime - explode.all_CoolDown[explode.currentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[2].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = explode.GetCoolDownTime.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = "- " + MathF.Abs(explode.GetCoolDownTime - explode.all_CoolDown[explode.currentLevel + 1]);
            WaveIndex++;
        }

        if (explode.GetCounterProjecTile - explode.all_CounterProjectile[explode.currentLevel + 1] != 0) {

            this_WaveProperty[WaveIndex].prpoertyName = all_Property[3].prpoertyName;
            this_WaveProperty[WaveIndex].CurrentPoerprtyValue = explode.GetCoolDownTime.ToString();
            this_WaveProperty[WaveIndex].NextPrpoertyValue = "+ " + MathF.Abs(explode.GetCounterProjecTile - explode.all_CounterProjectile[explode.currentLevel + 1]);

        }


        UIManager.instance.all_PowerUpUi[index].SetMyPowerUpPanel(explode.powerUpImage,
            explode.currentLevel, this, this.gameObject.activeSelf);
    }
    public override int Getlevel() {
        return PowerUpData.insatnce.explodinshot.currentLevel;
    }

    public override void setPowerUpInPlayer() {
        if (!PowerUpData.insatnce.explodinshot.isUnlocked) {
            PowerUpData.insatnce.explodinshot.isUnlocked = true;
        }
        else {
            PowerUpData.insatnce.explodinshot.currentLevel++;
        }

        flt_AreaDamage = PowerUpData.insatnce.explodinshot.GetAreaDamage;
        flt_Damage = PowerUpData.insatnce.explodinshot.GetCurrentDamage;
        flt_FireRate = PowerUpData.insatnce.explodinshot.GetCoolDownTime;
        ProjectileCounter = PowerUpData.insatnce.explodinshot.GetCounterProjecTile;
        flt_CurrentCoolDownTime = flt_FireRate;
        this.gameObject.SetActive(true);

    }





    private void Update() {

        BulletHandler();
    }

    private void BulletHandler() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }
       
        flt_CurrentTimeForSpawnBullet += Time.deltaTime;
        if (flt_CurrentTimeForSpawnBullet > flt_CurrentCoolDownTime) {
            flt_CurrentTimeForSpawnBullet = 0;
            SpawnBullet();
        }
    }

    private void SpawnBullet() {

        for (int i = 0; i < ProjectileCounter; i++) {
            FireBullet();
        }

       
                                                   
    }

    private void FireBullet() {
        if (GameManager.instance.list_ActiveEnemies.Count == 0) {
            return;
        }

        int Index = Random.Range(0, GameManager.instance.list_ActiveEnemies.Count);

        Transform target = GameManager.instance.list_ActiveEnemies[Index].transform;
        bulletSpawnpostion.LookAt(target);

        ExplodingBulletMotion current = Instantiate(obj_Bullet, bulletSpawnpostion.position,
                            transform.rotation);

        Vector3 direction = bulletSpawnpostion.forward;




      

        current.SetBulletData(direction, flt_Damage, flt_force, flt_AreaDamage, flt_AreaRange);
    }



   
}
