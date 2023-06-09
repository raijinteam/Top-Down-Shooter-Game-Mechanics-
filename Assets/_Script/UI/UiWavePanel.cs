using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;
using TMPro;
using System;

public class UiWavePanel : MonoBehaviour
{
    [SerializeField] private  string id_Animation = "Name Of Animation";
    [SerializeField] private bool isApperance;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] private TextMeshProUGUI txt_Level;
    public float flt_DealyCompletAnimation;

    public  void PlayUiWaveAnimation(int _currentWave) {
        txt_Level.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetText(_currentWave));
        StartCoroutine(EndOfAnimation());
    }

    private IEnumerator EndOfAnimation() {
        yield return new WaitForSeconds(flt_DealyCompletAnimation);
        this.gameObject.SetActive(false);
    }

    private string GetText(int currentWave) {
        string Text;
        if (isApperance) {
            Text = "{"+id_Animation+"}Level : "+currentWave + "{/" + id_Animation + "}";
        }
        else {
            Text = null;
        }
       
        return Text;
    }
}
