using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpHandler : MonoBehaviour
{

    public static PowerUpHandler instance;
    
    public PowerUpProperty[] all_PowerUp;

    [SerializeField] private List<PowerUpProperty> list_AllPowerUpsInGame = new List<PowerUpProperty>();
    [SerializeField] private List<PowerUpProperty> list_powerUpSelected = new List<PowerUpProperty>();
    [SerializeField] private List<PowerUpProperty> list_Of_Player_ActivePowerUpInGame;
    [SerializeField] private List<PowerUpProperty> list_Of_Player_PassivePowerUpInGame;
    

    [SerializeField] private int max_ActivePowerUp;
    [SerializeField] private int max_PassivePowerUP;
    public int MaxLevelUp = 5;


    private void Awake() {
        instance = this;
    }


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetPowerUpPanel();
        }
    }


  

    public void SetPlayerPowerUpSelected(int index) {



        list_powerUpSelected[index].setPowerUpInPlayer();

        if (list_Of_Player_ActivePowerUpInGame.Contains(list_powerUpSelected[index]) ) {
            return;
        }
        if (list_Of_Player_PassivePowerUpInGame.Contains(list_powerUpSelected[index])) {
            return;
        }

        if (list_powerUpSelected[index].isActivePowerUp) {
            list_Of_Player_ActivePowerUpInGame.Add(list_powerUpSelected[index]);
        }
        else {
            list_Of_Player_PassivePowerUpInGame.Add(list_powerUpSelected[index]);
        }



    }

   

    public void SetPowerUpPanel() {

       
       
        if (list_AllPowerUpsInGame.Count != 0) {
            list_AllPowerUpsInGame.Clear();
        }

        if (list_powerUpSelected.Count != 0) {
            list_powerUpSelected.Clear();
        }

        AddFromActivePowerups();
        AddFromPassivePowerups();
        RandomizePowerUps();
        UIManager.instance.powerup2Ui.StartSlotAnimation();



    }

    private void AddFromActivePowerups() {


        if (list_Of_Player_ActivePowerUpInGame.Count >= max_ActivePowerUp) {

            for (int i = 0; i < list_Of_Player_ActivePowerUpInGame.Count; i++) {
                if (list_Of_Player_ActivePowerUpInGame[i].Getlevel() == MaxLevelUp) {
                    continue;
                }

                list_AllPowerUpsInGame.Add(list_Of_Player_ActivePowerUpInGame[i]);
            }
        }
        else {
            for (int i = 0; i < all_PowerUp.Length; i++) {

                if (all_PowerUp[i].isActivePowerUp && all_PowerUp[i].Getlevel() != MaxLevelUp) {

                    list_AllPowerUpsInGame.Add(all_PowerUp[i]);
                }
            }
        }
    }

    private void AddFromPassivePowerups() {

        if (list_Of_Player_PassivePowerUpInGame.Count >= max_PassivePowerUP) {

            for (int i = 0; i < list_Of_Player_PassivePowerUpInGame.Count; i++) {
                if (list_Of_Player_PassivePowerUpInGame[i].Getlevel() == MaxLevelUp) {
                    continue;
                }

                list_AllPowerUpsInGame.Add(list_Of_Player_PassivePowerUpInGame[i]);
            }
        }
        else {
            for (int i = 0; i < all_PowerUp.Length; i++) {

                if (!all_PowerUp[i].isActivePowerUp && all_PowerUp[i].Getlevel() != MaxLevelUp) {

                    list_AllPowerUpsInGame.Add(all_PowerUp[i]);
                }
            }
        }
    }

    private void AddAllPowerups() {

        if(list_AllPowerUpsInGame.Count != 0) {
            list_AllPowerUpsInGame.Clear();
        }  

        if(list_powerUpSelected.Count != 0) {
            list_powerUpSelected.Clear();
        }

        for (int i = 0; i < all_PowerUp.Length; i++) {

            list_AllPowerUpsInGame.Add(all_PowerUp[i]);
        }

      
    }

    


    private void RandomizePowerUps() {


        Debug.Log(list_AllPowerUpsInGame.Count);
        if (list_AllPowerUpsInGame.Count >= 3) {

            for (int i = 0; i < 3; i++) {
                int randomIndex = Random.Range(0, list_AllPowerUpsInGame.Count);
                list_powerUpSelected.Add(list_AllPowerUpsInGame[randomIndex]);
                list_AllPowerUpsInGame[randomIndex].SetUI(i);
                list_AllPowerUpsInGame.RemoveAt(randomIndex);
            }
        }
        else {



            for (int i = 0; i < list_AllPowerUpsInGame.Count; i++) {

                list_powerUpSelected.Add(list_AllPowerUpsInGame[i]);
              
                list_AllPowerUpsInGame[i].SetUI(i);
            }

            for (int i = list_AllPowerUpsInGame.Count; i < 3; i++) {

                UIManager.instance.uIGamePlayScreen.all_PowerUpData[i].gameObject.SetActive(false);
            }

        }

    }

   
}
