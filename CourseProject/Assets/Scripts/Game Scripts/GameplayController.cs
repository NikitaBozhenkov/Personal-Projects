using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour {
    public float Score { get; set; }
    public bool IsGameOver { get; set; }

    [SerializeField] private float scoreMultiplier = 2f;
    [SerializeField] private TextMeshProUGUI powerupTimeText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI gameoverScoreText;
    [SerializeField] private GameObject gameoverPanel;

    public float powerupTime = 0f;
    public AudioSource audioSource;

    private void Start() {
        audioSource.Play();
    }

    private void Update() {
        if (powerupTime > 0f) {
            powerupTime -= Time.deltaTime;
        }
        
        if (!IsGameOver) Score += scoreMultiplier * Time.deltaTime; 
        else audioSource.Stop();
        if (Score > GameManager.Instance.GetHighscore()) {
            GameManager.Instance.SetHighscore((int) Score);
        }

        GameManager.Instance.GetHighscore();
        scoreText.SetText("Score:\n" + ((int) Score).ToString("0"));
        powerupTimeText.SetText("Powerup\ntime: " + ((int) powerupTime).ToString("0"));
    }

    public IEnumerator ShowGameOverPanel() {
        yield return new WaitForSeconds(1f);
        gameoverPanel.SetActive(true);
        gameoverScoreText.SetText("Score: " + ((int) Score).ToString("0"));
        highscoreText.SetText("Highscore: " + (GameManager.Instance.GetHighscore()));
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit() {
        SceneManager.LoadScene("Main Menu");
    }
}