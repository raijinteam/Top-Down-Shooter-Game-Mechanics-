using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Col NAME ENTER : " + collision.gameObject.name);
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision) {
        Debug.Log("Col NAME EXIT : " + collision.gameObject.name);
        isGrounded = false;
    }
}
