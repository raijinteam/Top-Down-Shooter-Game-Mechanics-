using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("PlayerData")]
    public bool isPlayerLive;

    [Header("PowerUp Handler")]
    public bool isRichoestPowerUp;
    internal bool isDealthBlowPowerUpActivated;
    public bool isLifeStealPowerUpActive;
    public bool isMissilePowerUpActive;
    [Header("killStreach Data")]
    public bool isKilltimeCalculation;
   
    [SerializeField] private KillStretch killStretch;
    [SerializeField] private float flt_MaxKillStreachTime;
    [SerializeField]private float flt_CurrentTime;

    private void Awake() {
        instance = this;
    }
    private void Update() {
        KillstreachCalculation();
    }

    private void KillstreachCalculation() {
        if (!isKilltimeCalculation) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime>flt_MaxKillStreachTime) {
            killStretch.killTretchedIndex = 0;
            flt_CurrentTime = 0;
        }
    }
    public void IncreasingkillStreachIndex() {
        if (flt_CurrentTime<flt_MaxKillStreachTime) {
            killStretch.killTretchedIndex++;
        }
    }
}
