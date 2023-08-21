using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveStartingScreen : MonoBehaviour
{
    [SerializeField] private string id_Animation = "Name Of Animation";
    [SerializeField] private bool isApperance;
    [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] private TextMeshProUGUI txt_WaveScreen;
   

    public void PlayUiWaveStartAnimation(int index,float  flt_Time) {

        txt_WaveScreen.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetWaveText(index));
        StartCoroutine(EndOfLevelAnimation(flt_Time));
    }

    private IEnumerator EndOfLevelAnimation(float flt_DelayOfComplteAnimkation) {
        yield return new WaitForSeconds(flt_DelayOfComplteAnimkation);
        this.gameObject.SetActive(false);
      
       
    }
   
    private string GetWaveText(int index) {
        string Text;
        if (isApperance) {
            Text = "{" + id_Animation + "}" + "Wave " + index + "{/" + id_Animation + "}";
        }
        else {
            Text = null;
        }

        return Text;
    }
}
