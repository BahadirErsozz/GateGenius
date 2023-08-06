using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneHandler : MonoBehaviour
{
    public void handleClickStartButton() {
        SceneManager.LoadScene("GameScene");
    }
}
