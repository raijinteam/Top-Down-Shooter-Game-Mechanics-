using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField]private bool isMusic;
    public bool isSound;

    private void Awake() {
        instance = this;


    }

    

    public bool Ismusic {
        get {

          return  isMusic;

        } private set {

            isMusic = value;
        }
    }

    public  bool IsSound {
        get {
            return isSound;
        }
        private set {
            isSound = value;
        }
    }

}
