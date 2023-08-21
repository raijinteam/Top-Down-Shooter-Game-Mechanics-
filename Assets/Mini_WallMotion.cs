using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_WallMotion : MonoBehaviour
{
    [SerializeField] private Transform[] all_WallPostion;
    [SerializeField] private float flt_Wallspeed;
    [SerializeField] private float flt_ChangPostion;
    [SerializeField] private float flt_NewPostion;

    private void FixedUpdate() {

        for (int i = 0; i < all_WallPostion.Length; i++) {

            all_WallPostion[i].transform.Translate(Vector3.back * flt_Wallspeed * Time.fixedDeltaTime);

            if (all_WallPostion[i].transform.position.z < flt_ChangPostion) {

                int index = i - 1;
                if (index < 0) {
                    index = all_WallPostion.Length - 1;
                }
                all_WallPostion[i].transform.position = all_WallPostion[index].position + new Vector3(0, 0, flt_NewPostion);
            }
        }
    }
}
