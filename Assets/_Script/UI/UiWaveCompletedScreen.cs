using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UiWaveCompletedScreen : MonoBehaviour
{
    [SerializeField] private Image img_BG;
    [SerializeField] private TextMeshProUGUI rectTransform_Txt;
    [SerializeField] private bool isStartAnimation;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private float flt_ScaleUp;

    private void Start() {
        img_BG.rectTransform.localPosition = new Vector3(-2500, img_BG.rectTransform.localPosition.y,
                    img_BG.rectTransform.localPosition.z);
        rectTransform_Txt.rectTransform.localPosition = new Vector3(2500, img_BG.rectTransform.localPosition.y,
                    img_BG.rectTransform.localPosition.z);
    }



    public void PlayWaveCompltedAnimation() {
        if (!isStartAnimation) {
            isStartAnimation = true;
            StartAnimation();
        }
    }

    private void StartAnimation() {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(PanelCenterAnimation).AppendInterval(flt_AnimationTime).
            Append(transform.DOScale(flt_ScaleUp, flt_AnimationTime).
            SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear)).AppendCallback(FadeOutAnimation).AppendInterval(flt_AnimationTime)
            .AppendCallback(StopAnimation);
    }

    private void FadeOutAnimation() {
        img_BG.rectTransform.DOScaleY(0, flt_AnimationTime);
        rectTransform_Txt.DOFade(0, flt_AnimationTime);
    }

    private void StopAnimation() {


        isStartAnimation = false;
        img_BG.rectTransform.localPosition = new Vector3(-2500, img_BG.rectTransform.localPosition.y,
                   img_BG.rectTransform.localPosition.z);
        rectTransform_Txt.rectTransform.localPosition = new Vector3(2500, img_BG.rectTransform.localPosition.y,
                    img_BG.rectTransform.localPosition.z);
        img_BG.rectTransform.DOScaleY(1, 0.1f);
        rectTransform_Txt.DOFade(1, 0.01f); transform.DOScale(1, 0.01f);
        GameManager.instance.WaveCompleteAnimationComplted();

    }

    private void PanelCenterAnimation() {
        img_BG.rectTransform.DOLocalMoveX(0, flt_AnimationTime);
        rectTransform_Txt.rectTransform.DOLocalMoveX(0, flt_AnimationTime);
    }
}
