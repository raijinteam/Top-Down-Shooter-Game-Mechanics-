using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyHealth : MonoBehaviour
{
    private EnemyData enemyData;
    [SerializeField] private float flt_MaxHealth;
    [SerializeField] private float flt_CurrrentHealth;
    [SerializeField] private TextMeshProUGUI txt_Health;

    [SerializeField] private GameObject enemy_UI;

    [Header("Health Slider")]
    [SerializeField] private Slider slider_Health; // normal slider
    [SerializeField] private Slider slider_Animation; // another slider to give damage effect
    private float flt_CurrentSliderHealth;

    

    [Header("Damage effect popup")]
    [SerializeField] private GameObject txt_Damage;
    [SerializeField] private Transform obj_txtDamageSpawnPostion;
    [SerializeField] private float flt_SpawnTextYpositionOffset;
    [SerializeField] private float currentTimePassed;

    [Header("Laser Data")]
    private bool isHitByLaser = false;
    private float laserDamage; // get from laser
    private float flt_LaserDamageTime = 1; // take damage every x second

    private float flt_CurrentLaserDamageTimer = 0f;
    private float flt_LaserAffectedDuration = 2f; // duration it will get affected by laser after it gets stop
    private float flt_CurrentLaserAffectedTimer = 0f;
    private Coroutine Cour_Slider_Animation;

    [Header("Monotive PowerUp")]
    [SerializeField] private bool isStartMonotivePowerUp;
    [SerializeField] private float flt_CurrnetTimeForMonotivePowerUp;
    [SerializeField] private float flt_DamageOverTime = 1;
    [SerializeField] private float flt_MonotioniveDamage;



    private void OnEnable() {

        enemyData = GetComponent<EnemyData>();
        flt_MaxHealth = enemyData.GetmaxHealth();
        flt_CurrrentHealth = flt_MaxHealth;
        slider_Health.maxValue = flt_MaxHealth;
        slider_Animation.maxValue = flt_MaxHealth;
        slider_Health.value = flt_CurrrentHealth;
        slider_Animation.value = flt_CurrrentHealth;
        txt_Health.text = flt_CurrrentHealth.ToString();
        flt_CurrentSliderHealth = flt_MaxHealth;
    }

    private void Update() {
        LaserHandling();
    }


    private void LateUpdate() {
        UiSetup();
       
    }

    public void EnemyDie() {
        TakeDamage(flt_MaxHealth);

        // if ( boss ){
        //TakeDamage(IncreasedDamageValue);        
        // }
    }


    public void SetHitbyMolotovePowerUp(float damage) {
        isStartMonotivePowerUp = true;
        this.flt_MonotioniveDamage = damage;
       
    }

    public void StopMolotovePowerUp() {
        isStartMonotivePowerUp = false;
    }

    private void MonotivePowerUpHandler()
   	{
        if (!isStartMonotivePowerUp) {
            flt_CurrnetTimeForMonotivePowerUp = 0;
            return;
        }
        flt_CurrnetTimeForMonotivePowerUp += Time.deltaTime;
        if (flt_CurrnetTimeForMonotivePowerUp > flt_DamageOverTime) {
            flt_CurrnetTimeForMonotivePowerUp = 0;
            TakeDamage(flt_MonotioniveDamage);
        }

    }

    private void UiSetup() {
        enemy_UI.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, -transform.eulerAngles.y, 0);
    }

    public void SetLaserAffacted(float flt_Damage) {
        isHitByLaser = true;
        laserDamage = flt_Damage;
      
    }

    private void LaserHandling() {
        if (!isHitByLaser) {
            return;
        }
        flt_CurrentLaserDamageTimer += Time.deltaTime;
        if (flt_CurrentLaserDamageTimer>flt_LaserDamageTime) {
            TakeDamage(laserDamage);
         
            flt_CurrentLaserDamageTimer = 0;
            isHitByLaser = false;
        }
    }

    public void TakeDamage(float damage) {

        
        flt_CurrrentHealth -= damage;

        if (GameManager.instance.isLifeStealPowerUpActive) {
            PlayerManager.instance.Player.GetComponent<PlayerHealth>().IncreasedHealth(damage);
        }


        if (flt_CurrrentHealth > 0) {

            if (Cour_Slider_Animation != null) {
                StopCoroutine(Cour_Slider_Animation);
            }

            Cour_Slider_Animation = StartCoroutine(SetSlider(flt_CurrrentHealth));

            GameObject current = Instantiate(txt_Damage, new Vector3(transform.position.x, flt_SpawnTextYpositionOffset,
                transform.position.z), Quaternion.identity, obj_txtDamageSpawnPostion);

            current.GetComponent<DamagePanel>().SetDamagePanel(damage, Color.yellow);



            slider_Health.value = flt_CurrrentHealth;
            txt_Health.text = flt_CurrrentHealth.ToString();
        }
        else {

            PlayerManager.instance.Player.GetComponent<PlayerPowerUpHandler>()
                .IncreasingPlayerPoint(enemyData.GetEnemyPoint());
            LevelManager.instance.RemoveListOfEnemy(gameObject);


        }

    }

    private IEnumerator SetSlider(float targetValue) {

        float startValue = flt_CurrentSliderHealth;


        currentTimePassed = 0.0f;

        float flt_MaxTime = 2;

        while (currentTimePassed < 1) {


            currentTimePassed += Time.deltaTime / flt_MaxTime;
            flt_CurrentSliderHealth = Mathf.Lerp(startValue, targetValue, currentTimePassed);

            slider_Animation.value = flt_CurrentSliderHealth;
            yield return null;
        }
        flt_CurrentSliderHealth = targetValue;
        slider_Animation.value = targetValue;
    }

}
