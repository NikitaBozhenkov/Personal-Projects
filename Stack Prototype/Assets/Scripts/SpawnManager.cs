using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public delegate void TileSpawned();
public delegate void GameOver();

public class SpawnManager : MonoBehaviour {
    [SerializeField] private TileMovement m_tilePrefab;
    
    [SerializeField] private Button m_spawnButton;

    private TileMovement m_lastTileSpawned;
    private Vector3Int m_nextSpawnPosition;
    private int m_bound;
    private int m_heightAdd;
    private int m_speed = 7;
    private int m_index = 0;
    [SerializeField] private int m_lastTileCoordinate;

    private ColorChanger m_colorChanger;
    private Camera m_camera;

    public event TileSpawned OnTileSpawned;
    public event GameOver OnGameOver;

    // Not from here
    [SerializeField] private GameObject m_startPanel;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private Button m_restartButton;



    private void Start() {
        m_startPanel.SetActive(true);
        GameplayManager.IsGameOver = false;
        m_scoreText.gameObject.SetActive(false);
        m_restartButton.gameObject.SetActive(false);
        m_scoreText.SetText("0");
        m_colorChanger = new ColorChanger(115, 115, 255, 1, 1);
        m_camera = Camera.main;
        //m_spawnButton.onClick.AddListener(SpawnNextTile);
        SetStartParameters();
        SpawnNextTile();
    }

    private void SetStartParameters() {
        var scale = m_tilePrefab.transform.localScale;
        m_bound = (int)(scale.z + scale.y);
        m_heightAdd = (int)scale.y;
        m_lastTileSpawned = m_tilePrefab;
        m_lastTileCoordinate = 0;
    }

    private IEnumerator StopForce(Rigidbody rb) {
        yield return new WaitForSeconds(3f);
        rb.AddRelativeForce(Vector3Int.up * 1000, ForceMode.Impulse);
    }

    private void DropObject(GameObject obj) {
        var rb = obj.AddComponent<Rigidbody>();
        rb.drag = 0;
    }

    private void CutLastTile() {
        var movingCoord = m_lastTileSpawned.GetModeCoordinate();

        if (m_lastTileSpawned.speed > 0) {
            while(Mathf.Abs(movingCoord - m_lastTileCoordinate) % 10 != 0) {
                ++movingCoord;
            }
        } else if (m_lastTileSpawned.speed < 0) {
            while (Mathf.Abs(movingCoord - m_lastTileCoordinate) % 10 != 0) {
                --movingCoord;
            }
        }

        m_lastTileSpawned.SetModeCoordinate(movingCoord);
        int offset = movingCoord - m_lastTileCoordinate;

        if (offset == 0) return;

        if(m_lastTileSpawned.GetModeScale() - Mathf.Abs(offset) > 0) {
            m_lastTileSpawned.SetModeScale(m_lastTileSpawned.GetModeScale() - Mathf.Abs(offset));
            m_lastTileSpawned.SetModeCoordinate(m_lastTileSpawned.GetModeCoordinate() - offset / 2);

            var tileToDrop = Instantiate(m_lastTileSpawned, m_lastTileSpawned.GetIntPosition(), m_lastTileSpawned.transform.rotation);
            
            if (offset > 0) {
                tileToDrop.SetModeScale(Mathf.Abs(offset));
                tileToDrop.SetModeCoordinate(m_lastTileSpawned.GetModeCoordinate() + m_lastTileSpawned.GetModeScale() / 2 + Mathf.Abs(offset) / 2);
                DropObject(tileToDrop.gameObject);
            } else if (offset < 0) {
                tileToDrop.SetModeScale(Mathf.Abs(offset));
                tileToDrop.SetModeCoordinate(m_lastTileSpawned.GetModeCoordinate() - m_lastTileSpawned.GetModeScale() / 2 - Mathf.Abs(offset) / 2);
                DropObject(tileToDrop.gameObject);
            }
        } else {
            DropObject(m_lastTileSpawned.gameObject);
            GameplayManager.IsGameOver = true;
            OnGameOver?.Invoke();
            OverAGame();
        }
        
    }

    private void OverAGame() {
        m_restartButton.onClick.AddListener(Restart);
        m_restartButton.gameObject.SetActive(true);
    }
    
    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    Vector3Int CalculateSpawnPosition(out TileMovement.MovementMode mode, out int speed) {
        Vector3Int spawnPosition = Vector3Int.zero;
        mode = TileMovement.MovementMode.X;
        speed = 0;

        if (m_index != 0) {
            spawnPosition = m_lastTileSpawned.GetIntPosition();
            if (m_index % 2 == 0) {
                spawnPosition.z = m_bound;
                mode = TileMovement.MovementMode.Z;
            } else {
                spawnPosition.x = m_bound;
                mode = TileMovement.MovementMode.X;
            }
            speed = m_speed;
            spawnPosition.y += m_heightAdd;
        } 

        return spawnPosition;
    }

    private void StopLastTile() {
        m_lastTileSpawned.StopMoving();
        CutLastTile();
        if (m_index % 2 == 0) {
            m_lastTileCoordinate = m_lastTileSpawned.GetIntPosition().z;
        } else {
            m_lastTileCoordinate = m_lastTileSpawned.GetIntPosition().x;
        }
    }

    private void SetSpawnedTileParameters(TileMovement.MovementMode mode, int speed) {
        m_lastTileSpawned.movementMode = mode;
        m_lastTileSpawned.speed = -speed;
        m_lastTileSpawned.bound = -m_bound;

        m_lastTileSpawned.GetComponent<Renderer>().material.SetColor("_Color", m_colorChanger.GetCurrentColor());
        m_colorChanger.ChangeColor();
    }

    private void StartAGame() {
        m_startPanel.SetActive(false);
        m_scoreText.gameObject.SetActive(true);
    }

    public void SpawnNextTile() {
        if (GameplayManager.IsGameOver) return;
        StopLastTile();
        if (GameplayManager.IsGameOver) return;

        if (m_index == 1) {
            StartAGame();
        }

        TileMovement tileToSpawn;
        if (m_index != 0) {
            tileToSpawn = m_lastTileSpawned;
        } else {
            tileToSpawn = m_tilePrefab;
        }
        m_lastTileSpawned = Instantiate(
            tileToSpawn, 
            CalculateSpawnPosition(out TileMovement.MovementMode mode, out int speed), 
            m_tilePrefab.transform.rotation
        );
        
        SetSpawnedTileParameters(mode, speed);
        m_lastTileSpawned.StartMoving();
        if(m_index != 0)
        StartCoroutine(MoveCameraUp(m_heightAdd));
        UpdateScore();

        if(m_index != 0 && m_index % 3 == 0) {
            ++m_speed;
        }

        ++m_index;
        OnTileSpawned?.Invoke();
    }

    private void UpdateScore() {
        m_scoreText.SetText((m_index - 1).ToString());
    }

    private IEnumerator MoveCameraUp(int height) {
        int heightBound = (int)m_camera.transform.position.y + height;
        var pos = m_camera.transform.position;
        while (m_camera.transform.position.y != heightBound) {
            pos.y += 4  ;
            m_camera.transform.position = pos;
            yield return new WaitForSeconds(0.001f);
        }
    }

    

}
