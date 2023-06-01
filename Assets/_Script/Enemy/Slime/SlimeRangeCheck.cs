using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRangeCheck : MonoBehaviour
{
    [SerializeField] private SlimeAttacking slimeAttcking;
    private string tag_Player = "Player";

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag.Equals(tag_Player)) {

            Debug.Log("PlayerIn Range");
            slimeAttcking.isAttckinInRange = true;

        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(tag_Player)) {

            slimeAttcking.isAttckinInRange = false;
            Debug.Log("Player OutOf Range");


        }
    }
}
