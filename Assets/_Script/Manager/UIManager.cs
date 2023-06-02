using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UiPenalScreen uiPenalScreen;
    public UiVictory uiVictory;
    public UiWavePanel uiWavePanel;
    public UiLevelPanel uiLevelPanel;
    public UIGamePlayScreen uIGamePlayScreen;

    private void Awake() {
        instance = this;
    }
}
