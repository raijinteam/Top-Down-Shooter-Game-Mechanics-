using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbiltyManager : MonoBehaviour {

    public static AbiltyManager instance;

    public GameObject[] all_Property;


    private void Awake() {
        instance = this;
    }

    
}

   
