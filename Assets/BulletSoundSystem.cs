using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip_BulletSpawn;
    [SerializeField] private AudioClip audioClip_BulletDestroyed;



    public void Play_BulletSpawn() {
        if (DataManager.instance.isSound) {
            audioSource.clip = audioClip_BulletSpawn;
            audioSource.Play();
        }
    }
    public void Play_BulletDestroyed() {
        if (DataManager.instance.IsSound) {
            audioSource.clip = audioClip_BulletDestroyed;
            audioSource.Play();
        }
    }
}
