using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    [SerializeField] private ParticleSystem[] all_WaterExpltion;

    [SerializeField] private float flt_Angle;
    [SerializeField] private float flt_AnimationTime;
    [SerializeField] private bool isGetDirection;

    private Coroutine cour_Target;
  


    public void StopExpltion() {
       
        for (int i = 0; i < all_WaterExpltion.Length; i++) {
            all_WaterExpltion[i].gameObject.SetActive(false);
        }
        transform.rotation = Quaternion.identity;
        if (cour_Target != null) {
            StopCoroutine(cour_Target);
        }
        
    }

    public void StartExpltion() {
       
        for (int i = 0; i < all_WaterExpltion.Length; i++) {
            all_WaterExpltion[i].gameObject.SetActive(true);
        }
       
    }

    private void Update() {
        AngleMotion();
    }

    private void AngleMotion() {

        if (isGetDirection) {
            isGetDirection = false;
            Quaternion target = Quaternion.Euler(flt_Angle, 0, 0);
           cour_Target =  StartCoroutine(GoesToTargetAngle(target));
        }
    }

    private IEnumerator GoesToTargetAngle(Quaternion target) {
        Quaternion startAngle = transform.rotation;
        float flt_CurrentTime = 0;
        float flt_MaxTime = flt_AnimationTime;
        
        while(flt_CurrentTime < 1) {

            flt_CurrentTime += Time.deltaTime / flt_MaxTime;

            transform.rotation = Quaternion.Slerp(startAngle, target, flt_CurrentTime);
            yield return null;
        }

        transform.rotation = target;
        flt_Angle = -flt_Angle;
        isGetDirection = true;

    }
}
