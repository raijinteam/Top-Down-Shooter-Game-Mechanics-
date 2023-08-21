using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps_JumpLandEffect;

    public void PlayJumpLandEffect() {
        ps_JumpLandEffect.Play();
    }
}
