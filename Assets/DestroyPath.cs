using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {

        if (other.TryGetComponent<Mini_PathMotion>(out Mini_PathMotion path)) {

            Mini_Pathmanager.insatnce.SpwnNewPath(path.gameObject);
        }
    }
}
