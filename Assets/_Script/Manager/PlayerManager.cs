
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
   
    [SerializeField] private Transform levelStartPostion;
    [SerializeField] private Transform levelEndPostion;
    [SerializeField] private Transform gameStartBoat;
    [SerializeField] private Transform GameEndBoat;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private Transform BoatEndPostion;
    [SerializeField] private GameObject obj_Player;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    public GameObject Player;
    [SerializeField]private float flt_JumpTime;
    [SerializeField]private float flt_JumpAccerletion;

    private void Awake() {
        instance = this;
    }

    private void Start() {
       // SpawnPLayer();
    }
    private void SpawnPLayer() {
        GameObject player = Instantiate(obj_Player, spawnPostion.position, spawnPostion.rotation, gameStartBoat);
        this.Player = player;
        Player.GetComponent<Rigidbody>().useGravity = false;
       
        UIManager.instance.uiLevelPanel.gameObject.SetActive(true);
      
       
        StartAnimation();
       
       
      
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            GameManager.instance.isPlayerLive = false;
            StartCoroutine(PlayerGoesToEndPostion(levelEndPostion.position));
        }

       
    }

    private IEnumerator PlayerGoesToEndPostion(Vector3 targetpostion) {

        float flt_fltcurrenttime = 0;
        Vector3 dirction = (targetpostion - Player.transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.z, -dirction.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, targetAngle, 0);

        Vector3 targetPostion = targetpostion;

        Vector3 startPostion = Player.transform.position;



        float CurrentMaxTime = 2;
        while (flt_fltcurrenttime < 1) {

            flt_fltcurrenttime += Time.deltaTime / CurrentMaxTime;
            Player.transform.position = Vector3.Lerp(startPostion, targetpostion, flt_fltcurrenttime);
            yield return null;
        }
               
        Player.transform.position = new Vector3(targetPostion.x,Player.transform.localScale.y, targetPostion.z);
        Player.GetComponent<Rigidbody>().useGravity = true;
       
        LevelEndAnimation();
        
    }

    private void LevelEndAnimation() {
        Sequence seq = DOTween.Sequence();
        GameEndBoat.transform.position = new Vector3(GameEndBoat.position.x, GameEndBoat.position.y, 33);
       
        seq.Append(GameEndBoat.DOMoveZ(1.36f, 2)).AppendCallback(JumpEndAnimation).AppendInterval(2).
            Append(GameEndBoat.DOMoveZ(-33, 2));
    }

    private void StartAnimation() {
        Sequence seq = DOTween.Sequence();
        gameStartBoat.transform.position = new Vector3(gameStartBoat.position.x,gameStartBoat.position.y, -33);
        seq.Append(gameStartBoat.DOMoveZ(-23.5f, 2)).AppendCallback(JumpStartAnimation);
           
    }

    private void JumpStartAnimation() {
        Vector3 childPostion = Player.transform.localPosition;
        Quaternion childQuternion = Player.transform.localRotation;
        Vector3 childScale = Player.transform.localScale;
        Player.transform.parent = null;
       // Player.transform.position = childPostion;
        Player.transform.rotation = childQuternion;
        Player.transform.localScale = childScale;

        StartCoroutine(Jump(levelStartPostion.position,false));
    }
    private void JumpEndAnimation() {
        Player.transform.SetParent(GameEndBoat);
       
        StartCoroutine(Jump(BoatEndPostion.position,true));
    }

    private IEnumerator Jump(Vector3 targetpostion,bool isEndPostion) {

        Player.GetComponent<PlayerMovement>().player_Animator.SetTrigger("Jump");


        // Get the starting position of the player
        Vector3 startPosition = Player.transform.position;


        float jumpheight = flt_JumpAccerletion * flt_JumpTime / (MathF.Sqrt(2*Physics.gravity.magnitude));

       
        // Keep track of how much time has passed since the start of the jump
        float elapsedTime = 0f;

        while (elapsedTime < flt_JumpTime) {
            // Calculate how far along the jump we are (0 to 1)
            float t = elapsedTime / flt_JumpTime;

            // Calculate the height of the jump at this point in time
            float height = Mathf.Sin(t * Mathf.PI) * jumpheight;

            // Calculate the new position of the player
            Vector3 newPosition = Vector3.Lerp(startPosition, targetpostion, t) + Vector3.up * height;

            // Move the player to the new position
            Player.transform.position = newPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Snap the player to the final position to ensure accuracy
        Player.transform.position = targetpostion;


        Rigidbody rb = Player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Player.transform.localEulerAngles = Vector3.zero;
        GameManager.instance.isPlayerLive = true;
        GameManager.instance.isKilltimeCalculation = true;
        cinemachine.Follow = Player.transform;

    }

}
