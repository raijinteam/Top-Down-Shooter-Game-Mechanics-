using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpDataUI : MonoBehaviour {

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
    public void SetMyPanel(PowerUpData powerUpData) {

        UIManager.instance.uIGamePlayScreen.PowerUpScreen(true);
        txt_PowerUpName.text = powerUpData.gameObject.name;
        if (powerUpData.IsUnlocked()) {
            panel_Unlocked.gameObject.SetActive(true);
            panel_Lock.gameObject.SetActive(false);
            txt_Level.text = "Level " + powerUpData.PowerUpLevel();

        }
        else {
            panel_Unlocked.gameObject.SetActive(false);
            panel_Lock.gameObject.SetActive(true);
        }


    }

    public void OnClick_PowerUp() {

        UIManager.instance.uIGamePlayScreen.PowerUpScreen(false);
        PlayerManager.instance.Player.GetComponent<PowerUpHandler>().SetPlayerPowerUpSelected(myIndex);
    }


}
