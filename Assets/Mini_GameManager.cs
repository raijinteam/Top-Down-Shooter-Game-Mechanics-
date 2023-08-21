using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mini_GameManager : MonoBehaviour
{
    public static Mini_GameManager instance;

    [Header("Game Data")]
    public bool IsPlayerLive;
    [SerializeField] private Mini_UiGameOverScreen Mini_UiGameOverScreen;

    [Header("Game Coin")]
    [SerializeField] private int CoinValue;

    [Header("Collectable")]
    [SerializeField] private int Collectabled;
     
  

    private void Awake() {
        instance = this;
    }

    private void Start() {
       

        
    }

    public void GameOverScreenActive() {
        IsPlayerLive = false;
        Mini_UiGameOverScreen.gameObject.SetActive(true);

    }

    public void StartPlayGame() {
        IsPlayerLive = true;
    }
}

