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


    // Start is called before the first frame update
    void Start()
    {   
        //GameObject.Find("WonText").SetActive(true);
        //wonTextAnimator.Play("ShowWonText", 0, 0.0f);
        //GameObject.Find("WonText").SetActive(false);
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerWonStage(int stageNumber){
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
