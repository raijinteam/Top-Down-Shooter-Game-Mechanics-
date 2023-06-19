using MoreMountains.Feedbacks;
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

   

    [Header("---- PlayerJump ----")]
    [SerializeField] private MMF_Player mmfPlayer_Jump;
    [SerializeField] private ParticleSystem particleVfx;
    [SerializeField] private string scaleup;
    [SerializeField] private string scaleDown;


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

   
   
    public void PlayerDamageTimeCameraShake(Transform body) {

        MMF_Scale scale = mmf_Player_Damage.GetFeedbackOfType<MMF_Scale>();

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
}