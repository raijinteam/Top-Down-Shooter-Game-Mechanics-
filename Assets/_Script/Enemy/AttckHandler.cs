using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckHandler : MonoBehaviour
{
    [SerializeField] private EnemyAttacking enemyAttacking;
    private string tag_Player = "Player";

    private void OnTriggerStay(Collider other) {

        if (other.gameObject.tag.Equals(tag_Player)) {

           
            enemyAttacking.isAttckinInRange = true;

        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag.Equals(tag_Player)) {
           
                enemyAttacking.isAttckinInRange = false;
            Debug.Log("Player OutOf Range");


        }
    }
}
