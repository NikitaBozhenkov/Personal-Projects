using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    public enum KillMethod {
        Bird, Boom, Stick, Sink, Log
    }

    public KillMethod killMethod;

    public void SetKillMethod(KillMethod method) {
        killMethod = method;
    }

    private void Start() {
        EventBroker.KillPlayer += KillPlayer;
    }

    public void KillPlayer() {
        if (this != null) gameObject.SetActive(false);
        GameManager.Instance.SetGameState(GameManager.GameState.End);
    }

    IEnumerator WaitAndDisactive(float sec) {
        yield return new WaitForSeconds(sec);
        if (this != null) gameObject.SetActive(false);
    }

    private void OnDisable() {
        EventBroker.KillPlayer -= KillPlayer;
    }
}
