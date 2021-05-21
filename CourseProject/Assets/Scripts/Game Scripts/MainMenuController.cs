using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("Gameplay");
    }

    public void ShowLeaders() {
        SceneManager.LoadScene("Leaders Menu");
    }

    public void Quit() {
        Application.Quit();
    }
}