using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelManager : MonoBehaviour {
    public static FeelManager instance;

    [Header("---Damage - PopUp ----")]
    [SerializeField] private MMF_Player popup_Damage;
    [SerializeField] private Gradient gradient_Normal;
    [SerializeField] private Gradient gradient_Critical;
   

    [Header("---- Scale ----- ")]
    [SerializeField] private MMF_Player mMF_Player_Scale;
    [SerializeField] private float flt_ScaleDown;
   

    [Header("---- Player Damage-----")]
    [SerializeField] private MMF_Player player_DamageCameraShake;

    [Header("---- DamageTimeScaleChange")]
    [SerializeField] private MMF_Player mmfPlayer_DamageScale;

    [Header("--- PlayerJump-----")]
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

    public void GetDamage(Vector3 postion, float damage, Transform taget,bool isCrtical) {

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

    public void PlayScaleAnimation(Transform target_Transform) {

      

        MMF_Scale scale = mMF_Player_Scale.GetFeedbackOfType<MMF_Scale>(this.scaleDown);

        scale.AnimateScaleTarget = target_Transform;
        scale.RemapCurveOne = target_Transform.localScale.y - flt_ScaleDown;
        scale.RemapCurveZero = target_Transform.localScale.y;

        MMF_ParticlesInstantiation particle = mMF_Player_Scale.GetFeedbackOfType<MMF_ParticlesInstantiation>();

        particle.ParticlesPrefab.transform.position = target_Transform.position;
        particle.Offset = new Vector3(0, -target_Transform.localScale.y, 0);


        mMF_Player_Scale.PlayFeedbacks();



    }
    public void PlayDamageScaleAnimation(Transform target_Transform) {


        MMF_Scale scale = mmfPlayer_DamageScale.GetFeedbackOfType<MMF_Scale>();

        scale.AnimateScaleTarget = target_Transform;

        mmfPlayer_DamageScale.PlayFeedbacks();



    }

    public void PlayerDamageTimeCameraShake(Transform body) {

        MMF_Scale scale = player_DamageCameraShake.GetFeedbackOfType<MMF_Scale>();

        scale.AnimateScaleTarget = body;
        player_DamageCameraShake.PlayFeedbacks();
    }
    public void PlayPlayerJump(Transform body, float flt_JumpTime, Vector3 targtPostion) {

        MMF_Rotation Rotation = mmfPlayer_Jump.GetFeedbackOfType<MMF_Rotation>();
       // MMF_HoldingPause pause = mmfPlayer_Jump.GetFeedbackOfType<MMF_HoldingPause>();
        MMF_Scale scale = mmfPlayer_Jump.GetFeedbackOfType<MMF_Scale>(this.scaleDown);

        MMF_Scale scaleup = mmfPlayer_Jump.GetFeedbackOfType<MMF_Scale>(this.scaleup);

        scaleup.AnimateScaleTarget = body;
       
    
        //pause.PauseDuration = /*flt_JumpTime - 0.2F*/ 0;
        scale.AnimateScaleTarget = body;
        particleVfx.transform.position = new Vector3(targtPostion.x,0, targtPostion.z);
        mmfPlayer_Jump.PlayFeedbacks();
    }
}