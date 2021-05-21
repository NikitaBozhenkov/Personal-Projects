using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private const int MoveDistance = 10;
    private const float MoveTime = 0.2f;
    private const float RotationTime = 0.1f;

    private bool m_isSwipe;
    private Sequence m_sequence;
    private Vector3 m_newPosition;

    private bool m_isMoving;
    private float birdTime = 5f;

    private void Start() {
        m_sequence = DOTween.Sequence();
        m_newPosition = transform.position;

        EventBroker.PlayerCanMove += MakeStep;
    }

    private void MakeStep() {
        if (this != null) {
            StopMovement();
            Move(InputHandler.Instance.MoveDirection);
            Debug.Log(transform.position);
        }
    }

    private void OnDisable() {
        EventBroker.PlayerCanMove -= MakeStep;
        StopMovement();
    }

    private void Move(Vector3 direction) {
        m_isMoving = true;
        m_sequence.Append(transform.DOJump(m_newPosition = transform.position + direction * MoveDistance, MoveDistance, 1, MoveTime)
            .OnComplete(() => m_isMoving = false));

        // Needed angle depending on given direction
        Vector3 angle = new Vector3(0, direction.x * 90 - direction.z * (direction.z * 90 - 90), 0);

        transform.DORotate(angle, 0);
    }

    private void StopMovement() {
        m_sequence.Kill();
        transform.position = m_newPosition;
    }

    public bool IsMoving() {
        return m_isMoving;
    }
}
