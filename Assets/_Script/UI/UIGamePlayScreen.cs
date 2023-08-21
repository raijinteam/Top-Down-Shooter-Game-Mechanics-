using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGamePlayScreen : MonoBehaviour
{

    public VariableJoystick variableJoystick;
    public TextMeshProUGUI txt_PowerUpName;
    [SerializeField] private Image img_PowerUp;
    [SerializeField] private Image img_Ultimate;
    [SerializeField] private Button btn_PowerUp;
    public Image img_BG;
    public PowerUpDataUI[] all_PowerUpData;
    [SerializeField] private GameObject obj_Panel;


    
    [SerializeField] private float flt_UltimateSpriteSeTime = 1;
    
    private bool isMove = false;

   

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            // StartCoroutine(SetImgPowerUpTest());

            ShowPowerUpTimer(3);
        }
    }


    public void Onclick_PowerUpSelected(int Index) {
        GameManager.instance.Player.GetComponent<PlayerPowerUpHandler>().
                    setPowerUpIndex(Index);
    }

    internal void PowerUpScreen(bool v) {
        obj_Panel.SetActive(v);
    }

    public void OnClick_PowerUpBtnClick() {
        img_PowerUp.gameObject.SetActive(true);
        img_Ultimate.fillAmount = 0;
        btn_PowerUp.interactable = false;
        GameManager.instance.Player.GetComponent<PowerUpHandler>().SetPowerUpPanel();
        GameManager.instance.Player.GetComponent<PlayerPowerUpHandler>().IncresedPowerUpPoint();
       
       
       
      
       
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
