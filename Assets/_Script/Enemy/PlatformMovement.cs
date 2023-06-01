using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMoveY(0.1f, 1f));
        seq.SetLoops(-1, LoopType.Yoyo);
        
    }

   
    
}
