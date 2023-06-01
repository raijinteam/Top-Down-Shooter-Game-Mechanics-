using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour
{
    public Collider Sword;
    public Transform parent;
    public int damage;
    public float flt_KnockBack;
    [SerializeField] private GameObject PlaySwordTuchVfx;
    [SerializeField] private bool isSpider;
    [SerializeField] private bool isSlime;

    private void Start() {

        if (isSlime) {
            damage = parent.GetComponent<SlimeData>().damage;
            flt_KnockBack = parent.GetComponent<SlimeData>().flt_knockBackForce;
        }
        else if (isSpider) {
            damage = parent.GetComponent<SpiderData>().damage;
            flt_KnockBack = parent.GetComponent<SpiderData>().flt_knockBackForce;
        }
        else {
            damage = parent.GetComponent<SkeletonData>().damage;
            flt_KnockBack = parent.GetComponent<SkeletonData>().flt_knockBackForce;
        }
      
        Sword.enabled = true;
    }
    public  void PLaySworVFx() {
        Instantiate(PlaySwordTuchVfx, transform.position, transform.rotation);
    }
}
