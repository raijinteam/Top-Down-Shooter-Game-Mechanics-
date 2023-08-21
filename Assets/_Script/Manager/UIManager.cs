using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UiLevelPanel uilevelScreen;
    public WaveStartingScreen waveStartingScreen;
    public UIGamePlayScreen uIGamePlayScreen;
    public UiWaveCompletedScreen uiWaveCompltedScreen;
    public UiStageCompletedScreen uiStageCompletedScreen;
    public uiPowerUp powerup2Ui;
    public PowerUpDataUI[] all_PowerUpUi;
    public UiBossPanel uiBossPanel;

    private void Awake() {
        instance = this;
    }
}
