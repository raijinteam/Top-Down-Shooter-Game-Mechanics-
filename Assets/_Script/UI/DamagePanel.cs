using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class DamagePanel : MonoBehaviour
{
    
    [SerializeField] private float flt_Speed;
    [SerializeField]private TextMeshProUGUI txt_Update;
    private float destroyTimer = 2f;

    private void Start() {
        StartCoroutine(AlphaZero());
        Destroy(gameObject, destroyTimer);
    }

    private IEnumerator AlphaZero() {
        yield return new WaitForSeconds(0.5f);
        txt_Update.DOFade(0, 1);
        txt_Update.transform.DOScale(0, 1);
    }

    public void SetDamagePanel(float _damage, Color _Color) {
        txt_Update.text = "-" + _damage.ToString();
        txt_Update.color = _Color;
    }

    void Update()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.Translate(Vector3.up * flt_Speed * Time.deltaTime);
        
    }

    public void IncresedHealth(int flt_Health, Color red) {
        txt_Update.text = "+" + flt_Health.ToString();
        txt_Update.color = red;
    }
}
