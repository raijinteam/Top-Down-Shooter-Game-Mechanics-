using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlayScreen : MonoBehaviour
{
    [SerializeField] private Image img_PowerUp;
    [SerializeField] private Image img_Ultimate;
    [SerializeField] private Button btn_PowerUp;


    
    [SerializeField] private float flt_UltimateSpriteSeTime = 1;
    
    private bool isMove = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            // StartCoroutine(SetImgPowerUpTest());

            ShowPowerUpTimer(3);
        }
    }


    public void Onclick_PowerUpSelected(int Index) {
        PlayerManager.instance.Player.GetComponent<PlayerPowerUpHandler>().
                    setPowerUpIndex(Index);
    }

    public void OnClick_PowerUpBtnClick() {
        img_PowerUp.gameObject.SetActive(true);
        PlayerManager.instance.Player.GetComponent<PlayerPowerUpHandler>().isPowerUpStart = true;
        btn_PowerUp.interactable = false;
        PlayerManager.instance.Player.GetComponent<PlayerPowerUpHandler>().StartUltimate();
        img_Ultimate.fillAmount = 0;
      
       
    }

    public void ShowPowerUpTimer(float _maxTime) {

        StartCoroutine(SetImgPowerUp(_maxTime));
    }

    private IEnumerator SetImgPowerUp(float maxTime) {


        float startValue = 0;
        Debug.Log("PowerUpStart");
        float flt_CurrentTime = 0;

        while (flt_CurrentTime < 1) {
            Debug.Log("PowerUpNotCompltered");
            flt_CurrentTime += Time.deltaTime/ maxTime;
            img_PowerUp.fillAmount = Mathf.Lerp(startValue, 1, flt_CurrentTime);
            yield return null;
        }
        Debug.Log("PowerUpCompltered");
        img_PowerUp.gameObject.SetActive(false);

        PlayerManager.instance.Player.GetComponent<PlayerPowerUpHandler>().UltimateCompleted();
    }

    public void SetUltimateScore(float fillAmmount) {

        if (img_Ultimate.fillAmount >= 1) {
            return;
        }
        img_Ultimate.gameObject.SetActive(true);
        StartCoroutine(SetImgUlitmate(fillAmmount));
        
       
    }

    private IEnumerator SetImgUlitmate(float fillAmmount) {


        float startValue = img_Ultimate.fillAmount;
        
        float flt_CurrentTime = 0;
        while (flt_CurrentTime < 1) {

            flt_CurrentTime += Time.deltaTime / flt_UltimateSpriteSeTime;
            img_Ultimate.fillAmount = Mathf.Lerp(startValue, fillAmmount, flt_CurrentTime);
            yield return null;
        }
        if (fillAmmount >= 1) {
            btn_PowerUp.interactable = true;
            img_Ultimate.gameObject.SetActive(false);
        }
    }
}