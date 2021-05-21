using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
    private int m_maxZ;

    [SerializeField] Spawner spawner;
    [SerializeField] Vector3 playerStartPosition = new Vector3(0, 5, 40);
    [SerializeField] private MapStorage m_mapStorage;
    [SerializeField] GameStatsMenuController gameStats;
    [SerializeField] private GameObject playerPref;

    [SerializeField] AudioClip[] sound;
    [SerializeField] AudioSource audioSource;

    public GameObject player;

    private IEnumerator Inst() {
        StartCoroutine(spawner.SpawnNonPlayablePart());
        yield return new WaitForSeconds(.5f);
        player = spawner.SpawnPlayer(playerStartPosition);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(SpawnPlayablePart());
    }

    private void Start() {
        m_mapStorage = new MapStorage(new Vector2Int(4, 4));
        StartCoroutine(Inst());

        m_maxZ = 4;

        EventBroker.InputMade += CheckPlayerMove;
        EventBroker.LineDeleted += OnLineDeleted;
        EventBroker.KillPlayer += KillPlayer;
    }

    private void OnLineDeleted() {
        if (m_mapStorage != null && spawner != null)
            if (m_mapStorage.GetStorageSize() < 40) {
                m_mapStorage.UpdateMap(spawner.SpawnParts());
            }
    }

    private void OnDisable() {
        EventBroker.InputMade -= CheckPlayerMove;
        EventBroker.LineDeleted -= OnLineDeleted;
        EventBroker.KillPlayer -= KillPlayer;
    }

    public IEnumerator SpawnPlayablePart() {
        m_mapStorage.UpdateMap(spawner.SpawnGrass(5, true));
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < 10; ++i) {
            m_mapStorage.UpdateMap(spawner.SpawnParts());
            yield return new WaitForSeconds(.1f);
        }
    }

    private bool CheckForObstacle(Vector2Int pos) {
        if (pos.y < 0 || pos.y > 8) return true;
        return m_mapStorage.GetValueAt(pos.x, pos.y) == 1;
    }

    public void CheckPlayerMove() {
        if (player == null || !player.activeSelf) return;

        var direction = InputHandler.Instance.MoveDirection;
        var newPos = m_mapStorage.GetCurPlayerPos() + new Vector2Int(direction.z, direction.x);
        if (CheckForObstacle(newPos)) return;

        EventBroker.CallPlayerCanMove();
        m_mapStorage.SetCurPlayerPos(newPos);
        UpdateMaxZ(newPos.x);

        if(m_mapStorage.GetValueAt(newPos.x, newPos.y) == -1) {
            StartCoroutine(KillPlayerAfter(.1f));
        }

    }

    public void KillPlayer() {
        if (player == null) return;
        var kill = player.GetComponent<PlayerKiller>();
        if (kill == null) return;

        if (kill.killMethod == PlayerKiller.KillMethod.Sink) {
            audioSource.PlayOneShot(sound[1]);
        } else if (kill.killMethod == PlayerKiller.KillMethod.Boom || kill.killMethod == PlayerKiller.KillMethod.Stick) {
            audioSource.PlayOneShot(sound[0]);
        }
    }

    private IEnumerator KillPlayerAfter(float time) {
        yield return new WaitForSeconds(time);
        player.GetComponent<PlayerKiller>().SetKillMethod(PlayerKiller.KillMethod.Sink);
        EventBroker.CallKillPlayer();
    }

    public void UpdateMaxZ(int value) {
        if(m_maxZ < value) {
            m_maxZ = value;
            GameManager.Instance.curScore = value - 4;
        }
    }
}
