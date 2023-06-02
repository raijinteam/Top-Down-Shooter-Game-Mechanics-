using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{

    [SerializeField] private int baseValue;
    [SerializeField] private int HealthIncreasingPersantage;
    [SerializeField] private int damageIncreasingPersantage;
    [SerializeField] private float knockBackPersanatge;


    [SerializeField] private int enemyPoint;
    [SerializeField]private  float flt_Damage;
    [SerializeField] private float flt_KnockBack;
    [SerializeField] private float flt_MaxHealth;


    private void OnEnable() {
        baseValue = GameManager.instance.GetLevel();
        SetLevel();
    }
    public float GetmaxHealth() {
        return ((int)flt_MaxHealth);
    }
    public float GetDamage() {
        return ((int)flt_Damage);
    }
    public float GetKnockBackForce() {
        return ((int)flt_Damage);
    }
    public int GetEnemyPoint() {
        return enemyPoint;
    }

    public void IncreasingLevel() {
        baseValue++;
        SetLevel();
    }

    public void SetLevel() {

       

        for (int i = 0; i < GameManager.instance.GetLevel(); i++) {

            flt_Damage = flt_Damage + (flt_Damage * damageIncreasingPersantage * 0.01f);
            flt_MaxHealth = flt_MaxHealth + (flt_MaxHealth * HealthIncreasingPersantage * 0.01f);
            flt_KnockBack = flt_KnockBack + (flt_KnockBack * knockBackPersanatge * 0.01f);
        }
        
    }


}
