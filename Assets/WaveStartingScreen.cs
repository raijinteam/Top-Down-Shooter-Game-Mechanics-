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
    [SerializeField] private float flt_DelayOfComplteAnimkation;



   
    public void PlayUiBossLevelAnimation() {

        txt_WaveScreen.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetBossText());
        StartCoroutine(EndOfLevelAnimation(true));
    }

    public void PlayUiWaveStartAnimation(int index) {

        txt_WaveScreen.text = null;
        this.gameObject.SetActive(true);
        textAnimatorPlayer.ShowText(GetWaveText(index));
        StartCoroutine(EndOfLevelAnimation(false));
    }

    private IEnumerator EndOfLevelAnimation(bool isBoss) {
        yield return new WaitForSeconds(flt_DelayOfComplteAnimkation);
        this.gameObject.SetActive(false);
        if (isBoss) {
            GameManager.instance.SpawnBoss();
        }
        else {
            GameManager.instance.SpwnEnemyNewWave();
        }
        this.gameObject.SetActive(false);
    }
    private string GetBossText() {
        string Text;
        if (isApperance) {
            Text = "{" + id_Animation + "}" + "Boss Is Coming" + "{/" + id_Animation + "}";
        }
        else {
            Text = null;
        }

        return Text;
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
