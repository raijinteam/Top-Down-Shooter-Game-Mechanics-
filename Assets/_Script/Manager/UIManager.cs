using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UiWavePanel uilevelScreen;
    public WaveStartingScreen waveStartingScreen;
    public UIGamePlayScreen uIGamePlayScreen;
    public UiWaveCompletedScreen uiWaveCompltedScreen;
    public UiStageCompletedScreen uiStageCompletedScreen;
    public uiPowerUp powerup2Ui;
    private void Awake() {
        instance = this;
    }
}
