using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MinWallMotion : MonoBehaviour
{
    [SerializeField] private GameObject[] all_LeftWall;
    [SerializeField] private GameObject[] all_RigthWall;

    [SerializeField]private float flt_XLeftpostion;
    [SerializeField]private float flt_XRigthPostion;


    [ContextMenu("SetLeftWall")]
    private void SetLeftWall() {

        float flt_Postion = 0;
        for (int i = 0; i < all_LeftWall.Length; i++) {

            all_LeftWall[i].transform.position = new Vector3(flt_XLeftpostion, 0, flt_Postion);

            flt_Postion += 5.5f;
        }
    }

    [ContextMenu("SetRightWall")]
    private void SetRigthPostion() {

        float flt_Postion = 0;
        for (int i = 0; i < all_RigthWall.Length; i++) {

            all_RigthWall[i].transform.position = new Vector3(flt_XRigthPostion, 0, flt_Postion);

            flt_Postion += 5.5f;
        }
    }
}
