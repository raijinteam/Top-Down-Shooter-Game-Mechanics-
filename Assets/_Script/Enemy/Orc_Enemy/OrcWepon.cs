using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class OrcWepon : MonoBehaviour {

    [Header("Camponanat")]
    [SerializeField] private Collider collider_Hamer;
    [SerializeField] private Collider collider_Stick;
    [SerializeField] private EnemyData collisionHandlerForEnemy;
    public Transform parent;


    [Header("WeponData")]
    public float flt_Force;
    public float flt_Damage;

    [SerializeField] private Transform transform_Weapon;



    private void Start() {
        flt_Force = collisionHandlerForEnemy.GetKnockBackForce();
        flt_Damage = collisionHandlerForEnemy.GetDamage();

        transform_Weapon.DOScale(1.2f, 0.5f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
    }
    public void SetAllColider(bool value) {
        collider_Hamer.enabled = value;
        collider_Stick.enabled = value;
    }
}
 
