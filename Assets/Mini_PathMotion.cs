using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_PathMotion : MonoBehaviour
{

    [SerializeField]private Transform[] all_SpawnPostion;
    [SerializeField]private int NoOfObstackleSpawn;
    [SerializeField]private GameObject[] all_Obstacle;
    private List<Transform> list_SpawnPostion;

    [SerializeField] private GameObject coin;
    [SerializeField] private float flt_PersantageCoin;

    [SerializeField] private GameObject Collcetive;
    [SerializeField] private float flt_PersantageCollcetive;
    

    private void OnEnable() {

        list_SpawnPostion = new List<Transform>();

        for (int i = 0; i < all_SpawnPostion.Length; i++) {

            list_SpawnPostion.Add(all_SpawnPostion[i]);
        }

        for (int i = 0; i <NoOfObstackleSpawn; i++) {

            int randomPostionIndex = Random.Range(0, list_SpawnPostion.Count);
            int RandomObstackleINdex = Random.Range(0, all_Obstacle.Length);

            Instantiate(all_Obstacle[RandomObstackleINdex], list_SpawnPostion[randomPostionIndex].position, Quaternion.identity,transform);

            list_SpawnPostion.RemoveAt(randomPostionIndex);
        }

        

        for (int i = 0; i < list_SpawnPostion.Count; i++) {

            int index = Random.Range(0, 100);
            if (index < flt_PersantageCoin) {
                int RandomCoinINdex = Random.Range(0, list_SpawnPostion.Count);

                Vector3 Postion = Vector3.zero;
                Postion = list_SpawnPostion[RandomCoinINdex].position;
                Instantiate(coin, new Vector3(Postion.x, 0, Postion.z), Quaternion.identity, transform);
                list_SpawnPostion.RemoveAt(RandomCoinINdex);
            }
            
        }
        for (int i = 0; i < list_SpawnPostion.Count; i++) {

            int index = Random.Range(0, 100);
            if (index < flt_PersantageCoin) {
                int RandomCoinINdex = Random.Range(0, list_SpawnPostion.Count);

                Vector3 Postion = Vector3.zero;
                Postion = list_SpawnPostion[RandomCoinINdex].position;
                Instantiate(Collcetive, new Vector3(Postion.x, 0, Postion.z), Quaternion.identity, transform);
                list_SpawnPostion.RemoveAt(RandomCoinINdex);
            }

        }
    }


    
}
