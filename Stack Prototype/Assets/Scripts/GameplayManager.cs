using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    public Button TapButton { get; set; }
    [SerializeField] private int _score = 0;
    [SerializeField] private bool _nextTileDirectionZ = true;
    private TileMovement lastTile;
    [SerializeField] private float _centre;
    [SerializeField] private bool _IsGameOver;
    private Camera _camera;
    private Color _lastColor;

    public static bool IsGameOver = false;

    public event GameOver OnGameEnded;
    public event GameStarted OnGameStarted;

    [SerializeField] private float _movingRange = 6f;

    private void Start() {
        _IsGameOver = false;
    }



}
