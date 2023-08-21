using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class StartPositionSpline : MonoBehaviour
{
    [SerializeField]private float flt_Timer = 0.75f;

    [SerializeField] private SplineContainer spline;    

    private Coroutine coro_Jump;
    [SerializeField] private Vector3 startpostion;

    //private void OnTriggerEnter(Collider other) {

    //    if (other.TryGetComponent<CollisionHandling>(out CollisionHandling collsion)) {

    //        if (coro_Jump == null) {
    //            coro_Jump = StartCoroutine(JumpAnimation(collsion.playerHealth.transform));
    //        }
    //    }
    //}

    public void StartJumpAnimation(Transform _player) {
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
            current.transform.position = spline.EvaluatePosition(1 - flt_CurrentTime);
            yield return null;


        }

        current.SetParent(null);
        GameManager.instance.PlayerStartJumpAnimationCompleted();
        coro_Jump = null;
    }
}
