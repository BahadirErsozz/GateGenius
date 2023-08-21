using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator wonTextAnimator;
    public TextMeshProUGUI wonText;

    public GameObject gameOverUI;
    
    public GameObject HelpImagesLvl1;
    public GameObject HelpImagesLvl2;
    public GameObject HelpImagesLvl3;
    public GameObject HelpImagesLvl4;
    public GameObject HelpImagesLvl5;

    public GameObject character;
    public int spawnPointCount = 1;
    private int currentLevel = 0;

    public Camera mainCamera;
    public Camera deathCam;

    public Player playerController;

    private Transform[] spawnPoints;


    // Start is called before the first frame update
    void Start()
    {   
        spawnPoints = new Transform[spawnPointCount]; 
        for (int i = 0; i < spawnPointCount; i++) {
            string spawnPointName = "spawnPoint" + i;
            spawnPoints[i] = GameObject.Find(spawnPointName).GetComponent<Transform>();
        }
        //GameObject.Find("WonText").SetActive(true);
        //wonTextAnimator.Play("ShowWonText", 0, 0.0f);
        //GameObject.Find("WonText").SetActive(false);
        gameOverUI.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) {
            switch(currentLevel){
                case 0:  HelpImagesLvl1.SetActive(!HelpImagesLvl1.activeSelf); break;
                case 1:  HelpImagesLvl2.SetActive(!HelpImagesLvl2.activeSelf); break;
                case 2:  HelpImagesLvl3.SetActive(!HelpImagesLvl3.activeSelf); break;
                case 3:  HelpImagesLvl4.SetActive(!HelpImagesLvl4.activeSelf); break;
                case 4:  HelpImagesLvl5.SetActive(!HelpImagesLvl5.activeSelf); break;
            }
        }
    }

    public void TriggerWonStage(int stageNumber){
        currentLevel = stageNumber;
        string doorName = "door" + stageNumber;
        DoorController doorController = GameObject.Find(doorName).GetComponent<DoorController>();
        bool doorIsOpen = doorController.PlayAnimation();
        if(!doorIsOpen)
            ShowWonText(stageNumber);
    }

    void ShowWonText(int stageNumber){
        string wonTextMessage = "Congratz on beating Level " + stageNumber;
        wonText.text = wonTextMessage;
        wonTextAnimator.Play("ShowWonText", 0, 0.0f);
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        

    }

    public void restart()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOverUI.SetActive(false);
        mainCamera.enabled = true;
        deathCam.enabled = false;
        character.transform.position = spawnPoints[currentLevel].position;
        playerController.revive();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
