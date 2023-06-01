using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float flt_MaxHealth;
    [SerializeField] private float flt_CurrrentHealth; 
    [SerializeField] private TextMeshProUGUI txt_Health;
    [SerializeField] private PlayerData PlayerData;

    [SerializeField] private GameObject player_UI;

    [Header("Health Slider")]
    [SerializeField] private Slider slider_Health; // normal slider
    [SerializeField] private Slider slider_Animation; // another slider to give damage effect
    private float flt_CurrentSliderHealth;

    [Header("Damage effect popup")]
    [SerializeField] private GameObject txt_Damage;
    [SerializeField] private Transform obj_txtDamageSpawnPostion;
    [SerializeField] private float flt_SpawnTextYpositionOffset;
    [SerializeField]private float currentTimePassed;


    private Coroutine Cour_Slider_Animation;

    

    private void OnEnable() {

        flt_CurrrentHealth = flt_MaxHealth;
        slider_Health.maxValue = flt_MaxHealth;
        slider_Animation.maxValue = flt_MaxHealth;
        slider_Health.value = flt_CurrrentHealth;
        slider_Animation.value = flt_CurrrentHealth;
        txt_Health.text = flt_CurrrentHealth.ToString();
        flt_CurrentSliderHealth = flt_MaxHealth;
    }




    private void LateUpdate() {
        UiSetup();
    }
    private void UiSetup() {
        player_UI.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, -transform.eulerAngles.y, 0);
    }

    public void IncreasedHealth(float damage) {

        if (flt_CurrrentHealth == flt_MaxHealth) {
            return;
        }

        float flt_Health = (damage * (PlayerData.IncreasedPersantageHealth / 100));

        float GetHealth = flt_Health;
        flt_CurrrentHealth += flt_Health;
     

        if (flt_CurrrentHealth > flt_MaxHealth) {
            GetHealth = flt_CurrrentHealth - flt_MaxHealth;
            flt_CurrrentHealth = flt_MaxHealth;
        }

      
       

        if (Cour_Slider_Animation != null) {
            StopCoroutine(Cour_Slider_Animation);
        }

        Cour_Slider_Animation = StartCoroutine(SetSlider(flt_CurrrentHealth));

        GameObject current = Instantiate(txt_Damage, new Vector3(transform.position.x, flt_SpawnTextYpositionOffset,
            transform.position.z), Quaternion.identity, obj_txtDamageSpawnPostion);

       
        current.GetComponent<DamagePanel>().IncresedHealth(((int)GetHealth), Color.yellow);



        slider_Health.value = flt_CurrrentHealth;
        txt_Health.text = flt_CurrrentHealth.ToString();

    }

    public void TakeDamage(float damage) {
     

        flt_CurrrentHealth -= damage;
       

        if (flt_CurrrentHealth > 0) {

            if (Cour_Slider_Animation != null) {
                StopCoroutine(Cour_Slider_Animation);
            }

            Cour_Slider_Animation =  StartCoroutine(SetSlider(flt_CurrrentHealth));
           
            GameObject current = Instantiate(txt_Damage, new Vector3(transform.position.x, flt_SpawnTextYpositionOffset,
                transform.position.z), Quaternion.identity, obj_txtDamageSpawnPostion);

            current.GetComponent<DamagePanel>().SetDamagePanel(damage, Color.red);
           


            slider_Health.value = flt_CurrrentHealth;
            txt_Health.text = flt_CurrrentHealth.ToString();
        }
        else {
            Destroy(gameObject);
            GameManager.instance.isPlayerLive = false;
            
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
