using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Collectable : MonoBehaviour
{

    [SerializeField] private Transform body;
    [SerializeField] private Collider Body_Collider;
    [SerializeField] private ParticleSystem ps_Expltion;



    public virtual void CollectedCollcteable() {

       transform.SetParent(null);
        body.gameObject.SetActive(false);
        Body_Collider.enabled = false;
        ps_Expltion.gameObject.SetActive(true);
        ps_Expltion.Play();
        StartCoroutine(Delay_Destroyed());

    }

    private IEnumerator Delay_Destroyed() {

        yield return new WaitForSeconds(2);
        Destroy(transform.gameObject);
    }
}
