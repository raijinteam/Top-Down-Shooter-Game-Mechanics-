using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrnadeBulletMotion : MonoBehaviour
{


    [Header("BulletData")]
    [SerializeField] private float flt_DestroyTime;
    [SerializeField] private float flt_BulletDamage;
    
    [SerializeField] private Vector3 bulletTargetDirection;
    [SerializeField] private float flt_TargetRechedTime;
    [SerializeField] private float flt_JumpAccerletion;
    private float flt_DealyDestroy = 2;
    

    [Header("Vfx")]
    [SerializeField] private GrenadeExplotion particle_Explotion;


   



    public void SetBulletData(Vector3 _BulletTargetPostion, float _BulletDamage
                     , float flt_DestroyTime) {

        this.bulletTargetDirection = _BulletTargetPostion;
        this.flt_BulletDamage = _BulletDamage;
      
        this.flt_DestroyTime = flt_DestroyTime;



        StartCoroutine(BulletMotion());
    }

    private IEnumerator BulletMotion() {

        Vector3 StartPostion = transform.position;


        float jumpheight = flt_JumpAccerletion * flt_TargetRechedTime / 
                    (MathF.Sqrt(2 * Physics.gravity.magnitude));


        // Keep track of how much time has passed since the start of the jump
        float elapsedTime = 0f;

        while (elapsedTime < flt_TargetRechedTime) {
            // Calculate how far along the jump we are (0 to 1)
            float t = elapsedTime / flt_TargetRechedTime;

            // Calculate the height of the jump at this point in time
            float height = Mathf.Sin(t * Mathf.PI) * jumpheight;

            // Calculate the new position of the player
            Vector3 newPosition = Vector3.Lerp(StartPostion, bulletTargetDirection, t) + Vector3.up * height;

            // Move the player to the new position
            transform.position = newPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Snap the player to the final position to ensure accuracy

        
        transform.position = bulletTargetDirection;
        GrenadeExplotion current = Instantiate(particle_Explotion, transform.position,
            transform.rotation);

        current.SetData(flt_BulletDamage,flt_DestroyTime);


        Destroy(gameObject);
    }



   

   
}
