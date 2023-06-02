using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour
{
    public Collider Sword;
    public Transform parent;
    public float damage;
    public float flt_KnockBack;
    [SerializeField] private GameObject PlaySwordTuchVfx;
    [SerializeField] private bool isSpider;
    [SerializeField] private bool isSlime;

    private void Start() {

        damage = parent.GetComponent<EnemyData>().GetDamage();
        flt_KnockBack = parent.GetComponent<EnemyData>().GetKnockBackForce();
       
        Sword.enabled = true;
    }
    public  void PLaySworVFx() {
        Instantiate(PlaySwordTuchVfx, transform.position, transform.rotation);
    }
}
