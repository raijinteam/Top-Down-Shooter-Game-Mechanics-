using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI;

public class UiBossPanel : MonoBehaviour
{
    [SerializeField] private string id_Animation = "Name Of Animation";
    [SerializeField] private bool isApperance;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] private TextMeshProUGUI txt_Level;


    public void PlayLevelAnimation( float flt_AnimationTime) {
        txt_Level.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetText());
        StartCoroutine(EndOfAnimation(flt_AnimationTime));
    }

    private IEnumerator EndOfAnimation(float flt_AnimationTime) {
        yield return new WaitForSeconds(flt_AnimationTime);
        this.gameObject.SetActive(false);
    }

    private string GetText() {
        string Text;
        if (isApperance) {
            Text = "{" + id_Animation + "}  Boss Level  {/" + id_Animation + "}";
        }
        else {
            Text = null;
        }

        return Text;
    }
}
