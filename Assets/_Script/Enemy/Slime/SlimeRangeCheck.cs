using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRangeCheck : MonoBehaviour
{
    [SerializeField] private SlimeMotion slimeAttcking;
    private string tag_Player = "Player";

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag.Equals(tag_Player)) {

            
            slimeAttcking.isAttckinInRange = true;

        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(tag_Player)) {

            slimeAttcking.isAttckinInRange = false;
           


        }
    }
}
