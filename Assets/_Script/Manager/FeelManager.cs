using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelManager : MonoBehaviour {
    public static FeelManager instance;

    [Header("---- Enemy Damage PopUp ----")]
    [SerializeField] private MMF_Player popup_Damage;
    [SerializeField] private Gradient gradient_Normal;
    [SerializeField] private Gradient gradient_Critical;
   

   

    [Header("---- Player Damage -----")]
    [SerializeField] private MMF_Player mmf_Player_Damage;
    [SerializeField] private float flt_MinDamage;
    [SerializeField] private float flt_maxDamage;
    [SerializeField] private float min_Amplitude;
    [SerializeField] private float max_Amplitude;
    [SerializeField] private float min_Gain;
    [SerializeField] private float max_Gain;
   

   

    [Header("---- PlayerJump ----")]
    [SerializeField] private MMF_Player mmfPlayer_Jump;
    [SerializeField] private ParticleSystem particleVfx;
    [SerializeField] private string scaleup;
    [SerializeField] private string scaleDown;

    [SerializeField] private MMF_Player mmf_Camera;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float ZoomTime = 4;


    private void Awake() {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        instance = this;
    }

    public void SpawnDamagePopUp(Vector3 postion, float damage, Transform taget,bool isCrtical) {

        MMF_FloatingText floatingText = popup_Damage.GetFeedbackOfType<MMF_FloatingText>();

        floatingText.Value = "-" + damage;

        floatingText.ForceColor = true;


        if (isCrtical) {
            floatingText.AnimateColorGradient = gradient_Critical;
        }
        else {
            floatingText.AnimateColorGradient = gradient_Normal;
        }
       
        floatingText.TargetTransform = taget;
        popup_Damage?.PlayFeedbacks(postion, damage);
    }

   
   
    public void PlayerDamageTimeCameraShake(Transform body,float damage ) {

        if (damage < flt_MinDamage) {
            return;
        }

        MMF_Scale scale = mmf_Player_Damage.GetFeedbackOfType<MMF_Scale>();
        MMF_CameraShake cameraShake = mmf_Player_Damage.GetFeedbackOfType<MMF_CameraShake>();

        float flt_CurrentAmpltude = (((damage - flt_MinDamage) * (max_Amplitude - min_Amplitude)) / (flt_maxDamage - flt_MinDamage)) + min_Amplitude;
        float fltcurrntGain = (((damage - flt_MinDamage) * (max_Gain - min_Gain)) / (flt_maxDamage - flt_MinDamage)) + min_Gain;

        cameraShake.CameraShakeProperties.Amplitude = flt_CurrentAmpltude;
        cameraShake.CameraShakeProperties.Frequency = fltcurrntGain;

        scale.AnimateScaleTarget = body;
        mmf_Player_Damage.PlayFeedbacks();
    }
    public void PlayPlayerJump(Transform body) {

        MMF_SquashAndStretch mmf_Strech = mmfPlayer_Jump.GetFeedbackOfType<MMF_SquashAndStretch>(scaleup);
        mmf_Strech.SquashAndStretchTarget = body;
        MMF_SquashAndStretch mmf_StrechDown = mmfPlayer_Jump.GetFeedbackOfType<MMF_SquashAndStretch>(scaleDown);
        mmf_StrechDown.SquashAndStretchTarget = body;
        mmfPlayer_Jump.PlayFeedbacks();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            PLayZoomCamera();
        }
    }

    public void PLayZoomCamera() {

        mainCamera.gameObject.SetActive(false);
        
        MMF_Rotation mmfRotation = mmf_Camera.GetFeedbackOfType<MMF_Rotation>("1");
        mmfRotation.AnimateRotationTarget = mmf_Camera.transform;
        MMF_Rotation mmfRotation1 = mmf_Camera.GetFeedbackOfType<MMF_Rotation>("2");
        mmfRotation1.AnimateRotationTarget = mmf_Camera.transform;
        mmf_Camera.PlayFeedbacks();
        StartCoroutine(ResetCameraData());


    }

    private IEnumerator ResetCameraData() {
        yield return new WaitForSeconds(9);
        mainCamera.gameObject.SetActive(true);

    }
}