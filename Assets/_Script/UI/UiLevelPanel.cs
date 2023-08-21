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
    private float animationDuration = 2f;
 

    public void PlayLevelAnimation(int _CurrentStageIndex) {
        txt_Level.text = null;
        //this.gameObject.SetActive(true);
      
        StartCoroutine(LevelTextAnimation(_CurrentStageIndex));
    }


    private IEnumerator LevelTextAnimation(int _CurrentStageIndex) {

        txt_Level.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetText(_CurrentStageIndex));
        yield return new WaitForSeconds(animationDuration);
        txt_Level.gameObject.SetActive(false);
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
