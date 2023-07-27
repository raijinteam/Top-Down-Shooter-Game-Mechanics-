using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class OrcWepon : MonoBehaviour {

    [Header("Camponanat")]
    [SerializeField] private GameObject PlaySwordTuchVfx;
    [SerializeField] private Collider collider_Hamer;
    [SerializeField] private Collider collider_Stick;
    [SerializeField] private EnemyData collisionHandlerForEnemy;
    [SerializeField] private EnemySoundManager enemySound;
    public Transform parent;


    [Header("WeponData")]
    public float flt_Force;
    public float flt_Damage;

    [SerializeField] private Transform transform_Weapon;



    private void Start() {
        flt_Force = collisionHandlerForEnemy.GetKnockBackForce();
        flt_Damage = collisionHandlerForEnemy.GetDamage();

        transform_Weapon.DOScaleZ(1.1f, 0.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    public void PLaySworVFx(Vector3 collsionPoint) {
        Instantiate(PlaySwordTuchVfx, collsionPoint, transform.rotation);
        enemySound.Play_Attack();
        
    }
    public void SetAllColider(bool value) {
        collider_Hamer.enabled = value;
        collider_Stick.enabled = value;
    }
}
 
