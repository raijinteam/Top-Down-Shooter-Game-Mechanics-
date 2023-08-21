using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiPowerUp : MonoBehaviour
{
    public delegate void EventHandler(int Round);
    public EventHandler SlotEvent;
    [SerializeField] private ReelMotion[] all_ReelMotion;
    [SerializeField] private bool[] all_ReelMotionStatus;
    [SerializeField] private bool isSlotanimatioStart;

    [SerializeField] private PowerUpDataUI[] all_PowerupDataui;


    public void StartSlotAnimation() {
        Time.timeScale = 0;
        isSlotanimatioStart = true;
        SlotEvent?.Invoke(5);
    }


    private void Update() {

        for (int i = 0; i < all_ReelMotion.Length; i++) {
            all_ReelMotionStatus[i] = all_ReelMotion[i].Ismove;
        }

        for (int i = 0; i < all_ReelMotionStatus.Length; i++) {

            if (all_ReelMotionStatus[i] == true) {
                return;
            }
        }
        isSlotanimatioStart = false;

        for (int i = 0; i < all_PowerupDataui.Length; i++) {
            all_PowerupDataui[i].ShowItsProperty();
        }

    }
}
