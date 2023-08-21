using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {

        if (other.TryGetComponent<Mini_Obstackle>(out Mini_Obstackle obstackle)) {

            Mini_GameManager.instance.GameOverScreenActive();
        }
        if (other.TryGetComponent<Mini_Coin>(out Mini_Coin coin)) {
            coin.CollectedCollcteable();
        }
        if (other.TryGetComponent<Mini_Collectable>(out Mini_Collectable collectable)) {

            collectable.CollectedCollcteable();
        }
    }
}
