using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAttackHandler : MonoBehaviour
{

    [SerializeField] private BeeMotion beeAttack;
    private string tag_Player = "Player";

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag.Equals(tag_Player)) {


            beeAttack.isPlayerInRangeOfAttack = true;

        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(tag_Player)) {

            beeAttack.isPlayerInRangeOfAttack = false;



        }
    }
}
