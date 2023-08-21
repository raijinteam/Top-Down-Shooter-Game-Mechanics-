using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWepon : MonoBehaviour
{

   
    public  BossMovement boss;
    [SerializeField] private Collider SwordCollider;
    [SerializeField] private GameObject PlaySwordTuchVfx;
    [SerializeField] private Transform spawnPoint;


    public void EnableCollider(bool value) {
        if (value) {
            SwordCollider.enabled = true;
        }
        else {
            SwordCollider.enabled = false;
        }

    }


    public void PLaySworVFx(Vector3 postion) {
        Instantiate(PlaySwordTuchVfx, postion, transform.rotation);
       
    }



}
