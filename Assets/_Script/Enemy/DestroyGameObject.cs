using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] private float flt_Dealy;
    

    private void Start() {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy() {
        yield return new WaitForSeconds(flt_Dealy);
        Destroy(gameObject);
    }
}
