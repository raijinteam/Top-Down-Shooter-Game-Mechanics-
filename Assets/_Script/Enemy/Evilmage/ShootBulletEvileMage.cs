using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletEvileMage : MonoBehaviour
{
    [SerializeField] private EivileMageShooting eivileMageShooting;
   
    public void ShootBullet() {
        eivileMageShooting.SpwnBullet();
    }
}
