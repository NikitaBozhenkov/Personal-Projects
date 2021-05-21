using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private Vector2 m_startPos;
    private Vector2 m_touchDirection;

    public Vector3Int MoveDirection;

    private bool directionChosen;

    private bool m_handlingEnabled;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        m_handlingEnabled = true;
        directionChosen = false;

        EventBroker.ButtonTap += OnButtonTap;
        EventBroker.PlayerCanMove += OnPlayerCanMove;
    }

    private void OnDisable() {
        EventBroker.ButtonTap -= OnButtonTap;
        EventBroker.PlayerCanMove -= OnPlayerCanMove;
    }

    private void OnPlayerCanMove() {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Start) {
            GameManager.Instance.SetGameState(GameManager.GameState.Play);
        }
    }

    private void OnButtonTap() {
        if (this != null)
            StopAllCoroutines();
        if (this != null)
            StartCoroutine(DisableHandlingTill(new WaitForSeconds(.2f)));
    }

    public IEnumerator DisableHandlingTill(YieldInstruction enumerator) {
        m_handlingEnabled = false;
        yield return enumerator;
        m_handlingEnabled = true;
    }

    void Update() {
        if (!m_handlingEnabled) return;
        ParseTouches();
        if(directionChosen) {
            UpdateDirection();
            directionChosen = false;
            EventBroker.CallInputMade();
        }
    }

    private void UpdateDirection() {
            if (Mathf.Abs(m_touchDirection.x) > Mathf.Abs(m_touchDirection.y)) {
                if (m_touchDirection.x < 0) {
                MoveDirection = new Vector3Int(-1, 0, 0);
                } else {
                MoveDirection = new Vector3Int(1, 0, 0);
                }
            } else if (Mathf.Abs(m_touchDirection.x) < Mathf.Abs(m_touchDirection.y)) {
                if (m_touchDirection.y < 0) {
                MoveDirection = new Vector3Int(0, 0, -1);
                } else {
                MoveDirection = new Vector3Int(0, 0, 1);
                }
            } else {
                MoveDirection = new Vector3Int(0, 0, 1);
            }   
    }

    private void ParseTouches() {
        List<Touch> touches = InputHelper.GetTouches();

        if (touches.Count > 0) {
            Touch touch = touches[0];

            switch (touch.phase) {
                case TouchPhase.Began:
                    directionChosen = false;
                    m_startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    m_touchDirection = touch.position - m_startPos;
                    break;
                case TouchPhase.Ended:
                    if (touch.position == m_startPos) m_touchDirection = Vector2.zero;
                    directionChosen = true;
                    break;
            }
        }
    }
}
