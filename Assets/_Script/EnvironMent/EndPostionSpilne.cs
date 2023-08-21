using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class EndPostionSpilne : MonoBehaviour
{
    [SerializeField] private float enposTionTime;
    [SerializeField] private float flt_Timer = 0.75f;

    [SerializeField] private SplineContainer spline;

    private Coroutine coro_Jump;
    public void EndJumpAnimation(Transform _player) {

        if (coro_Jump == null) {
            coro_Jump = StartCoroutine(JumpAnimation(_player));
        }
    }

    private IEnumerator JumpAnimation(Transform current) {



        FeelManager.instance.PlayPlayerJump(current.GetChild(0));

        yield return new WaitForSeconds(0.25f);


        float flt_CurrentTime = 0;
        while (flt_CurrentTime < 1) {

            flt_CurrentTime += Time.deltaTime / flt_Timer;
            current.transform.position = spline.EvaluatePosition(flt_CurrentTime);
            yield return null;


        }


        coro_Jump = null;

        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(GameManager.instance.MoveBoatEndOfScreen).AppendInterval(2).AppendCallback(GameManager.instance.gameCompletAnimation);
       ;



      

    }
}
