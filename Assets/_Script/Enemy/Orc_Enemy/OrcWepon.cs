using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWepon : MonoBehaviour {

    [Header("Camponanat")]
    [SerializeField] private Collider collider_Hamer;
    [SerializeField] private Collider collider_Stick;
    [SerializeField] private orcEnemyData collisionHandlerForEnemy;
    public Transform parent;

    [Header("WeponData")]
    public float flt_Force;
    public float flt_Damage;

    private void Start() {
        flt_Force = collisionHandlerForEnemy.flt_knockBackForce;
        flt_Damage = collisionHandlerForEnemy.damage;
    }
    public void SetAllColider(bool value) {
        collider_Hamer.enabled = value;
        collider_Stick.enabled = value;
    }
}
 
