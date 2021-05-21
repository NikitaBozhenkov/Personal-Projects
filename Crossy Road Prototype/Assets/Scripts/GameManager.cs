using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    [SerializeField] GameObject GameplayManager;

    public int curScore = 0;

    private GameplayManager gameplayManager;
    [SerializeField] private StartMenuController startMenu;
    [SerializeField] private GameOverMenuController gameOverMenu;

    public enum GameState {
        Start, Play, Killing, End
    }

    public GameState CurrentGameState;

    public void SetGameState(GameState gameState) {
        CurrentGameState = gameState;
        EventBroker.CallGameStateChanged();
    }

    public bool IsGameStarted() {
        return CurrentGameState == GameState.Play;
    }

    private void CreateSingleton() {
        Instance = this;
    }

    private void Start() {
        CreateSingleton();
        MakeInstantiations();
        EventBroker.GameStateChanged += MenuChanging;
        SetGameState(GameState.Start);
        EventBroker.CoinPicked += UpdateScore;
    }

    private void UpdateScore() {
        DataStorage.SetCoinsCount(DataStorage.GetCoinsCount() + 1);
    }

    private void OnDisable() {
        EventBroker.CoinPicked -= UpdateScore;
        EventBroker.GameStateChanged -= MenuChanging;
    }

    private void MakeInstantiations() {
        gameplayManager = Instantiate(GameplayManager).GetComponent<GameplayManager>();
    }

    private void MenuChanging() {
        if(startMenu != null)
            startMenu.gameObject.SetActive(CurrentGameState == GameState.Start);
        if (gameOverMenu != null)
            gameOverMenu.gameObject.SetActive(CurrentGameState == GameState.End);
    }



}
