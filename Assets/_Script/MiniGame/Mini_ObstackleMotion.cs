using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_ObstackleMotion : MonoBehaviour {

    [SerializeField] private float flt_Speed;
    public void SetData(float flt_MovemenTSpeed) {


        this.flt_Speed = flt_MovemenTSpeed;
    }

    private void Update() {

        ObstakleMotion();

    }

    private void OnTriggerEnter(Collider other) {

        if (other.TryGetComponent<Mini_Boundry>(out Mini_Boundry mini_Boundry)) {
            this.gameObject.SetActive(false);
        }
    }

    private void ObstakleMotion() {
        transform.Translate(Vector3.back * flt_Speed * Time.deltaTime);
    }
}
