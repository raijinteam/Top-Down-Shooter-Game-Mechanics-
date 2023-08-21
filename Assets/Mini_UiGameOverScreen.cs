using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mini_UiGameOverScreen : MonoBehaviour
{
    public void OnClikOnReloadBtncClik() {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
