using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] TextMeshProUGUI highscore;

    private void Start() {
        restartButton.onClick.AddListener(() => {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        });  
    }

    private void OnEnable() {
        highscore.text = DataStorage.GetHighscore().ToString();
    }
}
