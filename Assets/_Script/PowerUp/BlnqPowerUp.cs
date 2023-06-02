using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlnqPowerUp : MonoBehaviour
{
    [Header("Componant")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float  flt_Range;
    [SerializeField] private bool isPowerUpStart;
    [SerializeField] private int Counter;
    [SerializeField] private int maxCounter;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask ground;
    private Vector3 targetPosition;  // Position clicked by the player
    [SerializeField]private float flt_Force = 50;


    private void OnEnable() {
        SetBlinqPowerUp();
    }
    private void Update() {

        PowerUpHandler();
     
    }

    private void PowerUpHandler() {

        if (Input.GetMouseButtonDown(0)) {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) {
                // Get the clicked position


                targetPosition = hit.point;

                //  Binq Time KnockBack Stop 
                playerMovement.SetBlinq();
                SpreadSpherCast(hit.point);
                player.position = new Vector3(hit.point.x, player.position.y, hit.point.z);

                Counter++;
                if (Counter >= maxCounter) {
                    this.gameObject.SetActive(false);
                    Counter = 0;
                }

            }
        }
    }

    private void SpreadSpherCast(Vector3 targetPostion) {

        Collider[] all_Collider = Physics.OverlapSphere(targetPostion, flt_Range);

        for (int i = 0; i < all_Collider.Length; i++) {
            if (all_Collider[i].TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

                Debug.Log(enemyTrigger.gameObject.name);

                Vector3 direction = (all_Collider[i].transform.position - targetPostion).normalized;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                enemyTrigger.SethitByBullet(0, flt_Force, direction);
            }
        }

    }

    private void SetBlinqPowerUp() {
        isPowerUpStart = true;
        Counter = 0;
    }
}
