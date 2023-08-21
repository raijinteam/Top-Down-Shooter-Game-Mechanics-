using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckHandler : MonoBehaviour
{
    [SerializeField] private skeletonMovement skeleton;
    private string tag_Player = "Player";

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag.Equals(tag_Player)) {


            skeleton.isAttckinInRange = true;

        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(tag_Player)) {

            skeleton.isAttckinInRange = false;
           


        }
    }
}
