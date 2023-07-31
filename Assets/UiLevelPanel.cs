using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiLevelPanel : MonoBehaviour {


    [SerializeField] private string id_Animation = "Name Of Animation";
    [SerializeField] private bool isApperance;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] private TextMeshProUGUI txt_Level;
 

    public void PlayLevelAnimation(int _CurrentStageINdex, float flt_AnimationTime) {
        txt_Level.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetText(_CurrentStageINdex));
        StartCoroutine(EndOfAnimation(flt_AnimationTime));
    }

    private IEnumerator EndOfAnimation(float flt_AnimationTime) {
        yield return new WaitForSeconds(flt_AnimationTime);
        this.gameObject.SetActive(false);
    }

    private string GetText(int currentWave) {
        string Text;
        if (isApperance) {
            Text = "{" + id_Animation + "}Level : " + currentWave + "{/" + id_Animation + "}";
        }
        else {
            Text = null;
        }

        return Text;
    }
}
