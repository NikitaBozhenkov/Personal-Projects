using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour {
    public enum MovementMode {
        X, Z
    }

    public MovementMode movementMode;

    public int bound;
    public int speed;

    [SerializeField] private bool m_isStopped;

    public void StartMoving() {
        m_isStopped = false;
    }

    public void StopMoving() {
        m_isStopped = true;
    }

    public Vector3Int GetIntPosition() {
        var temp = transform.position;
        return new Vector3Int((int)temp.x, (int)temp.y, (int)temp.z);
    }

    public Vector3Int GetIntScale() {
        var temp = transform.localScale;
        return new Vector3Int((int)temp.x, (int)temp.y, (int)temp.z);
    }

    public void SetIntPosition(Vector3Int vec) {
        transform.position = vec;
    }

    public int GetModeCoordinate() {
        var intPos = GetIntPosition();
        switch(movementMode) {
            case MovementMode.X: return intPos.x;
            case MovementMode.Z: return intPos.z;
            default: return 0;
        }
    }

    public void SetModeCoordinate(int value) {
        var intPos = GetIntPosition();
        switch (movementMode) {
            case MovementMode.X:
                intPos.x = value;
                break;
            case MovementMode.Z:
                intPos.z = value;
                break;
            default: break;
        }

        SetIntPosition(intPos);
    }

    public int GetModeScale() {
        var intScale = GetIntScale();
        switch (movementMode) {
            case MovementMode.X: return intScale.x;
            case MovementMode.Z: return intScale.z;
            default: return 0;
        }
    }

    public void SetModeScale(int value) {
        var lScale = transform.localScale;
        switch (movementMode) {
            case MovementMode.X:
                lScale.x = value;
                break;
            case MovementMode.Z:
                lScale.z = value;
                break;
            default: break;
        }

        transform.localScale = lScale;
    }

    

    private Vector3Int GetMovingDirection() {
        switch (movementMode) {
            case MovementMode.X: return new Vector3Int(1, 0, 0);
            case MovementMode.Z: return new Vector3Int(0, 0, 1);
            default: return Vector3Int.zero;
        }
    }

    private void CheckBoundTouch() {
       if (Math.Abs(GetModeCoordinate() - bound) <= Mathf.Abs(speed)) {
            speed *= -1;
            bound *= -1;
       }        
    }

    private void FixedUpdate() {
        if (m_isStopped) return;

        CheckBoundTouch();

        transform.Translate(GetMovingDirection() * speed);
    }
}
