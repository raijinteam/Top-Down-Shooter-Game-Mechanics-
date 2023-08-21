using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private GameObject invisible;
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Z)) {
            invisible.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            invisible.gameObject.SetActive(false);
        }
    }
}
