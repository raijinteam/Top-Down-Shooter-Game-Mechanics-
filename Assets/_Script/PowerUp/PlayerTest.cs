using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField]private PlayerShooting playerShooting;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.F1)) {

            if (playerShooting.enabled == true) {
                playerShooting.enabled = false;
            }
            else {
                playerShooting.enabled = true;
            }
           
        }
    }
}
