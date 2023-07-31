using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class PowerUpDataUI : MonoBehaviour {

    [Header("AnimationSetup")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject bg;
    [SerializeField] private Image ShowPanel;
    [SerializeField] private Image[] all_Img;


    [Header("DataSetup")]
    [SerializeField] private int myIndex;
    [SerializeField] private Image img_Symbole;
    [SerializeField] private TextMeshProUGUI txt_PowerUpName;
    [SerializeField] private TextMeshProUGUI txt_PowerUpInformation;
    [SerializeField] private GameObject panel_Lock;
    [SerializeField] private GameObject panel_Unlocked;
    [SerializeField] private TextMeshProUGUI txt_Level;
    [SerializeField] private TextMeshProUGUI[] all_txt_Header;
    [SerializeField] private TextMeshProUGUI[] all_Txt_Value;

   

    [SerializeField] private TextMeshProUGUI[] all_Txt_Diff;

    public void ShowItsProperty() {
        button.gameObject.SetActive(true);
        bg.gameObject.SetActive(false);
    }
    public void SetMyPanel(PowerUpData powerUpData) {

        UIManager.instance.uIGamePlayScreen.PowerUpScreen(true);
        button.gameObject.SetActive(false);
        ShowPanel.sprite = powerUpData.powerupSprite;
        bg.gameObject.SetActive(true);
        img_Symbole.sprite = powerUpData.powerupSprite;
        SetRandomPowerUp();
        txt_PowerUpName.text = powerUpData.gameObject.name;
        if (powerUpData.IsUnlocked()) {
            panel_Unlocked.gameObject.SetActive(true);
            panel_Lock.gameObject.SetActive(false);
            txt_Level.text = "Level " + powerUpData.PowerUpLevel();
            for (int i = 0; i < all_txt_Header.Length; i++) {
                all_txt_Header[i].text = powerUpData.all_MyDataDisplay[i].headerName;
                all_Txt_Value[i].text = powerUpData.all_MyDataDisplay[i].CurrentValue;
                all_Txt_Diff[i].text = powerUpData.all_MyDataDisplay[i].UpdateValue;
            }

        }
        else {
            panel_Unlocked.gameObject.SetActive(false);
            panel_Lock.gameObject.SetActive(true);
        }

      
    }

    private void SetRandomPowerUp() {
        PowerUpHandler powerup = PlayerManager.instance.Player.GetComponent<PowerUpHandler>();

        for (int i = 1; i < all_Img.Length; i++) {

            int index = Random.Range(0, powerup.all_PowerUp.Length);
            all_Img[i].sprite = powerup.all_PowerUp[index].powerupSprite;
        }
    }

    public void OnClick_PowerUp() {

        Time.timeScale = 1;
        UIManager.instance.uIGamePlayScreen.PowerUpScreen(false);
        PlayerManager.instance.Player.GetComponent<PowerUpHandler>().SetPlayerPowerUpSelected(myIndex);
    }


}
