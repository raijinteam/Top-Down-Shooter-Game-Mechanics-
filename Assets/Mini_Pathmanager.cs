using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Pathmanager : MonoBehaviour
{

    public static Mini_Pathmanager insatnce;
    
    [SerializeField] private GameObject path;
    [SerializeField] private GameObject lastPath;
    [SerializeField] private List<GameObject>  all_ActivePath = new List<GameObject>();
    [SerializeField] private float flt_pathspeed;
    [SerializeField] private float flt_StartPostion;
    [SerializeField] private float flt_DistanceBetweenTwoPath;
    [SerializeField] private int TotalPath;


    private void Awake() {
        insatnce = this;
    }


    private void Start() {

        float flt_SpawnPostion = flt_StartPostion;
        for (int i = 0; i < TotalPath; i++) {


          GameObject currentPath =  Instantiate(path, new Vector3(0, 0, flt_SpawnPostion), Quaternion.identity, transform);
            flt_SpawnPostion += flt_DistanceBetweenTwoPath;
            lastPath = currentPath;
            all_ActivePath.Add(currentPath);
        }
    }


    private void Update() {

        if (!Mini_GameManager.instance.IsPlayerLive) {
            return;
        }

        for (int i = 0; i < all_ActivePath.Count; i++) {

            all_ActivePath[i].transform.Translate(Vector3.back * flt_pathspeed * Time.deltaTime);
        }
    }


    public void SpwnNewPath(GameObject DestroyPath) {

        all_ActivePath.Remove(DestroyPath);
        Destroy(DestroyPath);

        float flt_SpawnPostion = lastPath.transform.position.z + flt_DistanceBetweenTwoPath;
        GameObject currentPath = Instantiate(path, new Vector3(0, 0, flt_SpawnPostion), Quaternion.identity, transform);
        flt_SpawnPostion += flt_DistanceBetweenTwoPath;
        lastPath = currentPath;
        all_ActivePath.Add(currentPath);

    }
}
