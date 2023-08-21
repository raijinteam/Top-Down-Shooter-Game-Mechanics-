using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterPlantBulletMotion : MonoBehaviour {
    [Header("Componant")]
    [SerializeField] private BulletSoundSystem bulletSound;
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
    [SerializeField] private float flt_BulletRange;

    [Header("Vfx")]
    [SerializeField] private ParticleSystem particle_Explotion;
    [SerializeField] private GameObject obj_TargetPostion;

    //tag
    private string tag_Player = "Player";
    private string tag_Obstackle = "Obstackle";
    [SerializeField] private LayerMask layerMask;

    public void SetBulletData(Vector3 _BulletTargetPostion, float _BulletDamage, float _KnockbackForce) {

        this.bulletTargetDirection = _BulletTargetPostion;
        this.flt_BulletDamage = _BulletDamage;
        this.flt_BulletKnockBackforce = _KnockbackForce;
        bulletSound.Play_BulletSpawn();
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
        SetSpherCast();
        transform.position = bulletTargetDirection;
        particle_Explotion.gameObject.SetActive(true);
        particle_Explotion.Play();
        bulletSound.Play_BulletDestroyed();
        obj_Body.SetActive(false);
        Collider_Bullet.enabled = false;
        StartCoroutine(DestroyedBullet());



    }

    private IEnumerator DestroyedBullet() {
        yield return new WaitForSeconds(flt_DealyDestroy);
        Destroy(gameObject);
    }

    private void SetSpherCast() {


        Collider[] all_Collider = Physics.OverlapSphere(transform.position, flt_BulletRange, layerMask);

        for (int i = 0; i < all_Collider.Length; i++) {


            if (all_Collider[i].TryGetComponent<CollisionHandling>(out CollisionHandling Player)) {

                float distance = Mathf.Abs(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                  new Vector3(all_Collider[i].transform.position.x, 0, all_Collider[i].transform.position.z)));
                Vector3 direction = (Player.transform.position - transform.position).normalized;

                direction = new Vector3(direction.x, 0, direction.z).normalized;
                if (distance <0.5f) {

                    direction = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
                }

                Player.SetHitByNormalBullet(flt_BulletDamage, flt_BulletKnockBackforce, direction);
            }

        }


    }
}
