using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    [SerializeField] private SwordTrigger swordTrigger;
    [SerializeField] private GameObject body;
    [SerializeField] private float flt_RotationSpeed;
    [SerializeField] private float flt_MovementSpeed;
    [SerializeField] private float flt_Range;
    [SerializeField] private bool isGoesTotargetPostion;
    [SerializeField] private Vector3 startPostion;
    [SerializeField] private Vector3 targetPostion;
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject explotion;


    private void Start() {
        SetNewTarget();
    }

    public  void SetSwordData(float force ,float flt_Damage) {

        swordTrigger.SetSwordData(flt_Damage, force);
    }
    private void Update() {
        body.transform.Rotate(Vector3.up * flt_RotationSpeed * Time.deltaTime,Space.World);

        GoesTotargetPostion();
    }

    private void GoesTotargetPostion() {
        if (isGoesTotargetPostion) {
            transform.Translate(direction * flt_MovementSpeed * Time.deltaTime,Space.World);

            float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position, targetPostion));
            if (flt_Distance < 0.5f) {
                isGoesTotargetPostion = false;
            }
        }
        else {
            Vector3 _CurrentDirection = (PlayerManager.instance.Player.transform.position - transform.position)
                                    .normalized;

            transform.Translate(_CurrentDirection * flt_MovementSpeed * Time.deltaTime, Space.World);

            float flt_Distance = Mathf.Abs(Vector3.Distance(transform.position,
                                        PlayerManager.instance.Player.transform.position));

            if (flt_Distance < 0.5f) {

                Destroy(gameObject);
                Instantiate(explotion, transform.position, transform.rotation);
            }
        }

    }


    private void SetNewTarget() {
        
        direction = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
        targetPostion = transform.position +  direction * flt_Range;
        isGoesTotargetPostion = true;
    }
}
