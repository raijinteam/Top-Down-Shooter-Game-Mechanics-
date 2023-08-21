using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveBullet : MonoBehaviour
{
    [Header("Camponant")]
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Transform body;
    [SerializeField] private BulletSoundSystem bulletSound;

    [Header("Bullet Data")]
    [SerializeField] private float amplitude = 1f;    // amplitude of the snake movement
    [SerializeField] private float frequency = 1f;    // frequency of the snake movement
    [SerializeField] private float speed = 1f;        // speed of the bullet movement
    private float force;
    private float time = 0f;        // current time
    private float damage;
    private float flt_DelayOfDestroy = 3;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem explotion;

    private Vector3 dirction;
    private Vector3 perpendiqulerDirection;
    private bool isMove;


    //tag
    private string tag_Player = "Player";
    private string tag_Obstackle = "Obstackle";


    private void Start() {

        isMove = true;
        Destroy(gameObject, 15);
    }
    public void SetBulletData(Vector3 direction, float damage, float flt_Force) {
        this.damage = damage;
        this.dirction = direction;
        this.force = flt_Force;
        perpendiqulerDirection = Vector3.Cross(direction, Vector3.up);
        bulletSound.Play_BulletSpawn();

    }
    private void Update() {
        BulletMotion();
    }

    private void BulletMotion() {
        if (!isMove) {
            return;
        }


        //calculate the snake movement
        float x = Mathf.Sin(time * frequency) * amplitude;

        transform.position += (perpendiqulerDirection.normalized * x + dirction) * speed * Time.deltaTime;

        // increase the time
        time += Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag(tag_Player)) {

            if (other.TryGetComponent<CollisionHandling>(out CollisionHandling collisionHandling)) {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                collisionHandling.SetHitByNormalBullet(damage, force, new Vector3(direction.x, 0, direction.z).normalized);
                BulletDestroy();
            }

        }


    }

    private void OnCollisionEnter(Collision collision) {
        BulletDestroy();
    }

    private void BulletDestroy() {

        bulletSound.Play_BulletDestroyed();
        body.gameObject.SetActive(false);
        thisCollider.enabled = false;
        explotion.gameObject.SetActive(true);
        explotion.Play();
        isMove = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(flt_DelayOfDestroy);

        if (gameObject != null) {
            Destroy(gameObject);
        }
    }
}
