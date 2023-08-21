using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_ObstaclePooling : MonoBehaviour {

    [SerializeField] private Mini_ObstackleMotion[] mini_Obstackle;
    [SerializeField] private float flt_SpawnRate;
    [SerializeField] private float flt_CurrentTime;
    [SerializeField] private int CurrentIndex;
    [SerializeField] private Vector3 spawPostion;
    [SerializeField] private float flt_Boundry;
    [SerializeField]private float Speed;


    private void Update() {
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime > flt_SpawnRate) {
            spawnObstacled();
            flt_CurrentTime = 0;
        }
    }
    public void spawnObstacled() {


        mini_Obstackle[CurrentIndex].transform.position = GetRandomPostion();
        mini_Obstackle[CurrentIndex].SetData(Speed);
        mini_Obstackle[CurrentIndex].gameObject.SetActive(true);
        CurrentIndex++;
        if (CurrentIndex >= mini_Obstackle.Length) {
            CurrentIndex = 0;
        }
    }


    private Vector3 GetRandomPostion() {

        return new Vector3(Random.Range(-flt_Boundry, flt_Boundry), spawPostion.y, spawPostion.z);
    }

   
}
