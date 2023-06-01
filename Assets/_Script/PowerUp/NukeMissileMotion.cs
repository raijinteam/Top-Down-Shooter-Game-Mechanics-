using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class NukeMissileMotion : MonoBehaviour
{   
    
    [SerializeField] private float flt_DownPostion = 0;
    [SerializeField] private float flt_Animation = 2;
    [SerializeField] private float flt_Damage;
    [SerializeField] private float flt_Force;
    [SerializeField] private float flt_Range;
    private GameObject obj_Indicator;

    [Header("Vfx")]
    [SerializeField] private GameObject obj_Explotion;


    public void SetBulletData(float flt_Range,float force ,float flt_Damage, GameObject _Indicator) {
        this.flt_Range = flt_Range;
        this.flt_Force = force;
        this.flt_Damage = flt_Damage;
        obj_Indicator = _Indicator;

    }
    private  void Start() {

        startAnimation();
    }

    private void startAnimation() {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(0, flt_Animation)).AppendCallback(ExpltionVfx);
    }

    private void ExpltionVfx() {
       GameObject current =  Instantiate(obj_Explotion, transform.position, transform.rotation);
        current.GetComponent<NukeExpltion>().SetSpehreCastData(flt_Range, flt_Force, flt_Damage);
        Destroy(obj_Indicator);
       Destroy(this.gameObject);
    }
}
