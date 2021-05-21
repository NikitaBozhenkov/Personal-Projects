using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void GameStarted();

public class GameManager : MonoBehaviour {
    [SerializeField] private Button _tapButton;
    [SerializeField] private GameplayManager _gameplayManager;
    [SerializeField] private GameObject _uiManager;

    public event GameStarted OnGameStarted;

    private void Awake() {  
        PrepareMainMenu();
    }

    private void PrepareMainMenu() {
        //var gm = Instantiate(_gameplayManager);
        //gm.TapButton = _tapButton;
        //gm.OnGameEnded += RestartMainMenu;
        //gm.UpdateListeners();
    }

    private void RestartMainMenu() {
        //_tapButton.onClick.AddListener(StartGame);
        Debug.Log("Called");
    }

    private void StartGame() {
        _tapButton.onClick.RemoveListener(StartGame);
        OnGameStarted?.Invoke();
    }




}
