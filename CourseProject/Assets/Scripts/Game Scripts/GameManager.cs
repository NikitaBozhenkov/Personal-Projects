using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private const string HighscoreKey = "Highscore";
    private const string UsernameKey = "Username";
    private Button _startButton;
    [SerializeField] private GameObject loginPanel;

    void Start() {
        MakeSingleton();
        if (!PlayerPrefs.HasKey(UsernameKey)) {
            loginPanel.SetActive(true);
        }
    }

    public void Login(TextMeshProUGUI username) {
        SetUsername(username.text);
        loginPanel.SetActive(false);
        SetHighscore(0);
    }

    public int GetHighscore() {
        return PlayerPrefs.GetInt(HighscoreKey);
    }

    public string GetUsername() {
        return PlayerPrefs.GetString(UsernameKey);
    }

    private void SetUsername(string username) {
        PlayerPrefs.SetString(UsernameKey, username);
    }

    public void SetHighscore(int score) {
        PlayerPrefs.SetInt(HighscoreKey, score);
    }

    private void MakeSingleton() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != null) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}