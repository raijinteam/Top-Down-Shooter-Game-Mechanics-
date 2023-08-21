using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip_Die;
    [SerializeField] private AudioClip audioclip_Attack;
    [SerializeField] private AudioClip audioClip_groundTouch;
    [SerializeField] private AudioClip audioClip_WaterDrop;
    [SerializeField] private AudioSource audioSource;


   public void Play_DieSFX() {

        if (DataManager.instance.IsSound) {
            audioSource.clip = audioClip_Die;
            audioSource.Play();
        }
   }

    public void Play_Attack() {

        if (DataManager.instance.IsSound) {
            audioSource.clip = audioclip_Attack;
            audioSource.Play();
        }
    }
    public void Play_GroundTouchSFX() {
        if (DataManager.instance.IsSound) {
            audioSource.clip = audioClip_groundTouch;
            audioSource.Play();
        }
    }

    public void Play_WaterDropSFX() {
        if (DataManager.instance.IsSound) {
            audioSource.clip = audioClip_WaterDrop;
            audioSource.Play();
        }
    }
}
