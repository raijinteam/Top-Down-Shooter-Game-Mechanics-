using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPlantBulletMotion : MonoBehaviour {
    [Header("Componant")]
    [SerializeField] private GameObject obj_Body;
    [SerializeField] private Collider Collider_Bullet;

    [Header("BulletData")]
    [SerializeField] private float flt_BulletDamage;
    [SerializeField] private float flt_BulletKnockBackforce;
    [SerializeField] private Vector3 bulletTargetDirection;
    [SerializeField] private float flt_TargetRechedTime;
    [SerializeField] private float flt_JumpAccerletion;
    private float flt_DealyDestroy = 2;
    private GameObject obj_CurrentTarget;

    [Header("Vfx")]
    [SerializeField] private ParticleSystem particle_Explotion;
    [SerializeField] private GameObject obj_TargetPostion;

    //tag
    private string tag_Player = "Player";
    private string tag_Obstackle = "Obstackle";



    public void SetBulletData(Vector3 _BulletTargetPostion, float _BulletDamage, float _KnockbackForce) {

        this.bulletTargetDirection = _BulletTargetPostion;
        this.flt_BulletDamage = _BulletDamage;
        this.flt_BulletKnockBackforce = _KnockbackForce;

        obj_CurrentTarget =  Instantiate(obj_TargetPostion, bulletTargetDirection, Quaternion.identity);
        StartCoroutine(BulletMotion());
    }

    private IEnumerator BulletMotion() {

        Vector3 StartPostion = transform.position;


        float jumpheight = flt_JumpAccerletion * flt_TargetRechedTime / (MathF.Sqrt(2 * Physics.gravity.magnitude));


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

        Destroy(obj_CurrentTarget);
        transform.position = bulletTargetDirection;
        particle_Explotion.gameObject.SetActive(true);
        particle_Explotion.Play();
        obj_Body.SetActive(false);
        Collider_Bullet.enabled = false;
        StartCoroutine(DestroyedBullet());



    }

    private IEnumerator DestroyedBullet() {
        yield return new WaitForSeconds(flt_DealyDestroy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag(tag_Player)) {

            if (other.TryGetComponent<CollisionHandling>(out CollisionHandling collisionHandling)) {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                collisionHandling.SetHitByNormalBullet(flt_BulletDamage, 
                    flt_BulletKnockBackforce, new Vector3(direction.x, 0, direction.z).normalized);
                DestroyedBullet();
            }

        }
        if (other.gameObject.CompareTag(tag_Obstackle)) {
            DestroyedBullet();
        }
    }
}
