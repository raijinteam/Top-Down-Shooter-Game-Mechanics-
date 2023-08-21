using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPostionOfDamagePopUp : MonoBehaviour {

    private void LateUpdate() {

       transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, -transform.eulerAngles.y, 0);
    }
}
