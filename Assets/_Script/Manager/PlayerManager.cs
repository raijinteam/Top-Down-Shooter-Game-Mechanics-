
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
using MoreMountains.Feedbacks;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
   

    [Header("Player Start & End Data")]
    [SerializeField] private Transform levelStartPostion;
    [SerializeField] private Transform levelEndPostion;
    [SerializeField] private MMF_Player gameStartBoat;
    [SerializeField] private Transform GameEndBoat;
    [SerializeField] private Transform spawnPostion;
    [SerializeField] private Transform BoatEndPostion;

    [SerializeField] private PlayerData obj_Player;

    [Header("Componenet")]
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    public PlayerData Player;

    [Header("Player AnimationData")]
    [SerializeField]private float flt_JumpTime;
    [SerializeField]private float flt_JumpAccerletion;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SpawnPLayer();
       
    }
    private void SpawnPLayer() {
        PlayerData player = Instantiate(obj_Player, spawnPostion.position, spawnPostion.rotation, gameStartBoat.transform);
        this.Player = player;

       


        StartAnimation();
       
       
      
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            StageCompletd();
        }


    }

    public void StageCompletd() {
        GameManager.instance.isPlayerLive = false;
        
        Player.transform.LookAt(levelEndPostion);
        StartCoroutine(PlayerGoesToEndPostion(levelEndPostion.position));
    }

    private IEnumerator PlayerGoesToEndPostion(Vector3 targetpostion) {


        yield return new WaitForSeconds(2); // VictoryANimation
        float flt_fltcurrenttime = 0;
        Vector3 dirction = (targetpostion - Player.transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirction.z, -dirction.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, targetAngle, 0);

        Vector3 targetPostion = targetpostion;

        Vector3 startPostion = Player.transform.position;



        float CurrentMaxTime = 2;
        while (flt_fltcurrenttime < 1) {

            flt_fltcurrenttime += Time.deltaTime / CurrentMaxTime;
            Player.transform.LookAt(targetpostion);
            Player.transform.position = Vector3.Lerp(startPostion, targetpostion, flt_fltcurrenttime);
            yield return null;
        }
               
        Player.transform.position = targetpostion;

        
       


        LevelEndAnimation();
        
    }

    private void LevelEndAnimation() {
        Sequence seq = DOTween.Sequence();
        GameEndBoat.gameObject.SetActive(true);
        GameEndBoat.transform.position = new Vector3(GameEndBoat.position.x, GameEndBoat.position.y, 39);
  
        seq.AppendCallback(JumpEndAnimation).AppendInterval(flt_JumpTime).
            AppendCallback(GameEndProcdure).AppendInterval(2.5f).AppendCallback(ResetPlayer);
    }

    private void ResetPlayer() {


        GameEndBoat.gameObject.SetActive(false);
        Player.transform.position = spawnPostion.position;
        Player.transform.rotation = spawnPostion.rotation;
        Player.transform.localScale = obj_Player.transform.localScale;
        Player.transform.SetParent(gameStartBoat.transform);
        GameEndBoat.transform.position = new Vector3(GameEndBoat.position.x, GameEndBoat.position.y, 39);
        cinemachine.Follow = levelStartPostion;
        GameEndBoat.transform.localPosition = new Vector3(GameEndBoat.localPosition.x, GameEndBoat.localPosition.y, 40);
        UIManager.instance.uIGamePlayScreen.img_BG.DOFade(0, 1);
          StartAnimation();
        
     

       
    }

    private void GameEndProcdure() {
        GameEndBoat.DOMoveZ(100, 2);
        UIManager.instance.uIGamePlayScreen.img_BG.DOFade(1, 2);
    }




    private void StartAnimation() {
        gameStartBoat.PlayFeedbacks();
       
        Player.GetComponent<Rigidbody>().useGravity = false;
        Sequence seq = DOTween.Sequence();

        UIManager.instance.uilevelScreen.PlayLevelAnimation(GameManager.instance.currentStageIndex + 1 , 2);
       
        gameStartBoat.transform.position = new Vector3(gameStartBoat.transform.position.x,gameStartBoat.transform.position.y, -60);
        gameStartBoat.PlayFeedbacks();
        seq.Append(gameStartBoat.transform.DOMoveZ(-40, 4)).AppendCallback(JumpStartAnimation);
           
    }

    private void JumpStartAnimation() {

        gameStartBoat.enabled = false;
        Vector3 childPostion = Player.transform.localPosition;
        Quaternion childQuternion = Player.transform.localRotation;
        Vector3 childScale = Player.transform.localScale;
        Player.transform.parent = null;
      
        Player.transform.rotation = childQuternion;
        Player.transform.localScale = childScale;
       

       
        StartCoroutine(Jump(levelStartPostion.position,false));
    }
    private void JumpEndAnimation() {

       
        StartCoroutine(Jump(BoatEndPostion.position,true));
    }

    private IEnumerator Jump(Vector3 targetpostion,bool isEndPostion) {

        Player.GetComponent<PlayerMovement>().player_Animator.SetTrigger("Jump");

        if (!isEndPostion) {
            FeelManager.instance.PlayPlayerJump(Player.body);

            yield return new WaitForSeconds(0.25f);
        }
       

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

        if (!isEndPostion) {
            GameManager.instance.isPlayerLive = true;
            GameManager.instance.StartSpawningEnemyForCurrentWave();
            cinemachine.Follow = Player.transform;
            CinemachineBasicMultiChannelPerlin camera = cinemachine.
                GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            camera.m_AmplitudeGain = 0;
            camera.m_FrequencyGain = 0;
            rb.useGravity = true;
        }
        else {
            Player.transform.SetParent(GameEndBoat);
           
            rb.useGravity = false;
            cinemachine.Follow = levelEndPostion;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Player.transform.localEulerAngles = Vector3.zero;
            Player.transform.localScale = obj_Player.transform.localScale;
            GameManager.instance.isPlayerLive = true;
            GameManager.instance.IsKillStreak = true;
        }
       
       

    }

}
