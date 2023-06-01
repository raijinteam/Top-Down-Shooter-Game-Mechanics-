using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class UiVictory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt_Victory;
    [SerializeField] private float flt_Angle;
    [SerializeField] private int loop;
    [SerializeField] private TextMeshProUGUI txt_VictoryChild;
    [SerializeField] private bool isStartAnimation;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private int ChildLoop;
    [SerializeField] private float flt_ChildAnimationTime;
     
    private void Start() {
        txt_Victory.gameObject.SetActive(false);
        txt_Victory.transform.localScale = new Vector3(8, 8, 8);
        txt_VictoryChild.gameObject.SetActive(false);
    }
   

    public  void PlayVictoryAnimation() {
        if ( !isStartAnimation) {
            isStartAnimation = true;
            StartAnimation();
        }
    }

    private void StartAnimation() {
        Sequence seq = DOTween.Sequence();
        txt_Victory.gameObject.SetActive(true);
        seq.Append(txt_Victory.transform.DOScale(1, flt_AnimationTime)).AppendCallback(ScaleUpanimation)
            .AppendInterval( flt_AnimationTime).AppendCallback(childScaleUp).AppendInterval((ChildLoop + 2)* flt_ChildAnimationTime).
            Append(txt_Victory.DOFade(0,flt_AnimationTime))
            .AppendCallback(StopAnimation);
    }

    private void StopAnimation() {
        isStartAnimation = false;
        txt_Victory.transform.localScale = new Vector3(8, 8, 8);
        txt_Victory.transform.DOLocalRotate(Vector3.zero, 0.01f);
        txt_Victory.gameObject.SetActive(false);
        txt_VictoryChild.DOFade(0.5f, 0.01f);
        txt_Victory.DOFade(1, 0.01f);
        txt_VictoryChild.transform.DOScale(0.91f,  0.01f);
        txt_VictoryChild.gameObject.SetActive(false);
    }

    private void childScaleUp() {

        Sequence seq = DOTween.Sequence();

        seq.Append(txt_Victory.transform.DOLocalRotate(new Vector3(0, 0, -flt_Angle), flt_ChildAnimationTime)).
            Append(txt_Victory.transform.DOLocalRotate(new Vector3(0, 0, flt_Angle), flt_ChildAnimationTime).SetEase(Ease.Linear)
            .SetLoops(ChildLoop, LoopType.Yoyo))
            .Append(txt_Victory.transform.DOLocalRotate(new Vector3(0, 0, 0), flt_ChildAnimationTime));
       
        txt_VictoryChild.transform.DOScale(1.5f, ((ChildLoop + 2)/2) * flt_ChildAnimationTime);
        txt_VictoryChild.DOFade(0, flt_ChildAnimationTime);
    }

    private void ScaleUpanimation() {
        txt_VictoryChild.gameObject.SetActive(true);
        txt_Victory.transform.DOScale(1.5f, flt_AnimationTime);
        txt_VictoryChild.DOFade(0.25f, flt_AnimationTime);

    }
}
