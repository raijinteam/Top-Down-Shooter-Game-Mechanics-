using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplotion : MonoBehaviour
{
    [SerializeField] private Collider This_Collider;
    private float flt_Damage;
    
    private float flt_DestroyTime;

    public void SetData(float flt_Damage  , float flt_destroyTime) {
        this.flt_Damage = flt_Damage;
       
        flt_DestroyTime = flt_destroyTime;
      //  StartCoroutine(DestroyExpltion(flt_destroyTime));
      // use Invoke
    }

    private IEnumerator DestroyExpltion() {

        yield return new WaitForSeconds(flt_DestroyTime);
        transform.position = new Vector3(0, 50000, 0);
         yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other) {

       
        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

            enemyTrigger.SetHitbyMolotovePowerUp( flt_Damage);
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("ExitCall");
        if (other.TryGetComponent<EnemyTrigger>(out EnemyTrigger enemyTrigger)) {

            enemyTrigger.StopMolotovePowerUp();
        }
    }
}
