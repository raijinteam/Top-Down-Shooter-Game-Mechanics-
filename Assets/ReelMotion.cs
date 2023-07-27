using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ReelMotion : MonoBehaviour
{
    public bool Ismove;
    public bool isTurnZero;
    [SerializeField] private uiPowerUp power;
    [SerializeField] private float startPostion;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float changePostion;
    [SerializeField] private float afterChangeAssignPostion;
    [SerializeField] private float flt_Interval;
    [SerializeField] private int turn;
    [SerializeField] private int maxTurn;
    [SerializeField] private float flt_StopAnimationTime;
    [SerializeField] private float flt_StopInterval;
    [SerializeField] private float flt_StartAnimationTime;
    [SerializeField] private float flt_StartInterval;


    private void OnEnable() {
        power.SlotEvent += playAnimation;
    }
    private void OnDisable() {
        power.SlotEvent -= playAnimation;
    }

    private void playAnimation(int Round) {
        maxTurn = Round;
        SetRotate();
        
       
    }

   

    public void SetRotate() {

        Ismove = true;
        Sequence SEQ = DOTween.Sequence();
        startPostion = transform.localPosition.y;
        float postion = startPostion + flt_StartInterval;

        Debug.Log("Name" + transform.name + "StartPostion" + startPostion + "Postion" + postion);
        SEQ./*Append(transform.DOLocalMoveY(postion, flt_StartAnimationTime).SetEase(Ease.Linear)).*/Append(
            transform.DOLocalMoveY(startPostion, moveSpeed).SetEase(Ease.Linear)).AppendCallback(StartAnimation).SetUpdate(true);
            ;
           


    }

    private void StopSlotmachine() {
        Sequence SEQ = DOTween.Sequence();

        SEQ.Append(transform.DOLocalMoveY(startPostion - flt_StopInterval, flt_StopAnimationTime)).SetEase(Ease.Linear)
            .Append(transform.DOLocalMoveY(startPostion, flt_StopAnimationTime).SetEase(Ease.Linear)).
            AppendInterval(flt_StopAnimationTime).AppendCallback(StopCallBack).SetUpdate(true);
    }


    private void StartAnimation() {

        Ismove = true;
        isTurnZero = false;
        startPostion = transform.localPosition.y;
        float postion = startPostion - flt_Interval;
        transform.DOLocalMoveY(postion, moveSpeed).SetEase(Ease.Linear).OnComplete(ResetBackground).SetUpdate(true);

    }

   
    

    private void StopCallBack() {


        Ismove = false;
        isTurnZero = false;
    }

    void ResetBackground() {

        if (!Ismove) {

            return;
        }



        if (isTurnZero) {



            if (transform.localPosition.y - startPostion <= flt_Interval) {

                Sequence SEQ = DOTween.Sequence();

                SEQ.Append(transform.DOLocalMoveY(startPostion, moveSpeed).SetEase(Ease.Linear)).OnComplete(StopSlotmachine).
                    SetUpdate(true);



                return;
            }
            float pos = transform.localPosition.y - flt_Interval;

            transform.DOLocalMoveY(pos, moveSpeed).SetEase(Ease.Linear).OnComplete(ResetBackground).SetUpdate(true);

        }
        else {

            // Reset the background position to the start and start the animation again
            if (transform.localPosition.y < changePostion && !isTurnZero) {

                transform.localPosition = new Vector3(transform.localPosition.x, afterChangeAssignPostion, transform.localPosition.z);
                turn++;
                if (turn >= maxTurn) {

                    turn = 0;
                    isTurnZero = true;

                }

            }

            float postion = transform.localPosition.y - flt_Interval;

            transform.DOLocalMoveY(postion, moveSpeed).SetEase(Ease.Linear).OnComplete(ResetBackground).SetUpdate(true);
        }






    }



}
