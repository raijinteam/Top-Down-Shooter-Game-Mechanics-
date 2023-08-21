using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_UiHomeScreen : MonoBehaviour
{
    public void OnClick_OnPlayBtnClick() {
        this.gameObject.SetActive(false);
        Mini_GameManager.instance.StartPlayGame();
    }
}
