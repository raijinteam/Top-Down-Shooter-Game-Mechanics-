using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePooler : MonoBehaviour {

    public static SpikePooler instance;
    [SerializeField] private int totaleValue;
  
    [SerializeField] private ParticleSystem[] partcleVfx;
    [SerializeField] private int CurrentIndex;


    private void Awake() {
        instance = this;
    }


   

    public void PlayPasrtcleVfx(Vector3 postion,float flt_Damage,float fltForce, float _scale) {

        partcleVfx[CurrentIndex].transform.position = postion;
        partcleVfx[CurrentIndex].transform.localScale = new Vector3(0.8f + _scale, 0.8f + _scale, 0.8f + _scale);
        partcleVfx[CurrentIndex].gameObject.SetActive(true);
        partcleVfx[CurrentIndex].Play();
        partcleVfx[CurrentIndex].GetComponent<BossSpike>().SetSpike(flt_Damage, fltForce);
        CurrentIndex++;
        if (CurrentIndex >= partcleVfx.Length) {
            CurrentIndex = 0;
        }
    }
}
