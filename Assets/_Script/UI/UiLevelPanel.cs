using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;
using System;

public class UiLevelPanel : MonoBehaviour
{
    [SerializeField] private string id_Animation = "Name Of Animation";
    [SerializeField] private bool isApperance;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] private float flt_DelayOfComplteAnimkation;

    public void PlayUiLevelAnimation(int _currentLevel) {

        textAnimatorPlayer.ShowText(GetText(_currentLevel));
        StartCoroutine(EndOfLevelAnimation());
    }

    private IEnumerator EndOfLevelAnimation() {
        yield return new WaitForSeconds(flt_DelayOfComplteAnimkation);
        UIManager.instance.uiWavePanel.gameObject.SetActive(true);
        UIManager.instance.uiWavePanel.PlayUiWaveAnimation(1);
        this.gameObject.SetActive(false);
    }
    private string GetText(int _currentLevel) {
        string Text;
        if (isApperance) {
            Text = "{" + id_Animation + "}Level: " + _currentLevel + "{/" + id_Animation + "}";
        }
        else {
            Text = null;
        }

        return Text;
    }
}
