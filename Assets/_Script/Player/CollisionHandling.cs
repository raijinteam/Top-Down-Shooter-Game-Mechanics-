using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandling : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField]private PlayerHealth playerHealth;
    [SerializeField]private PlayerMovement playerMovement;
    [SerializeField] private GameObject obj_WaterParticle;


    [SerializeField] private bool isKavachActive;


    // tag
    private string tag_Weppon = "Weapon";
    private string tag_Water = "Water";

 

    public void SetHitByNormalBullet(float _damage, float _force, Vector3 _direction) {
        if (isKavachActive) {
            return;
        }
        playerHealth.TakeDamage(_damage);
        playerMovement.KnockBack(_direction, _force);
    }


    private void OnTriggerEnter(Collider other) {
       

        if (other.gameObject.CompareTag(tag_Weppon)) {

            if (other.TryGetComponent<Wepon>(out Wepon wepon)) {
                Vector3 direction = (wepon.parent.position - transform.position).normalized;
                wepon.Sword.enabled = false;
                wepon.PLaySworVFx();
                playerMovement.KnockBack(-direction, wepon.flt_KnockBack);
                playerHealth.TakeDamage(wepon.damage);
            }
            else if (other.TryGetComponent<OrcWepon>(out OrcWepon orcWepon)) {
                Vector3 direction = (orcWepon.parent.position - transform.position).normalized;
                orcWepon.SetAllColider(false);
                orcWepon.PLaySworVFx(other.ClosestPoint(transform.position));
                playerMovement.KnockBack(-direction, orcWepon.flt_Force);
                playerHealth.TakeDamage(orcWepon.flt_Damage);
            }
          


        }
        if (other.gameObject.CompareTag(tag_Water)) {

            Debug.Log("GameOver");
            GameManager.instance.isPlayerLive = false;
            Instantiate(obj_WaterParticle, transform.position, transform.rotation);
            StartCoroutine(delay_Destroy());
          
           
        }
       
    }

    private IEnumerator delay_Destroy() {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }



    public void SetKavachActive() {

        isKavachActive = true;
    }

    public  void StopKavach() {
        isKavachActive = false;
    }

   
}
