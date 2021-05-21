using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour 
{ 

    private void OnTriggerEnter(Collider other) {
        if (gameObject == null) return;

        if (other.gameObject.CompareTag("Car")) {
            if (gameObject.GetComponent<PlayerMovement>().IsMoving()) {
                if(gameObject.GetComponent<PlayerKiller>() != null)
                    gameObject.GetComponent<PlayerKiller>().SetKillMethod(PlayerKiller.KillMethod.Stick);
            } else {
                if (gameObject.GetComponent<PlayerKiller>() != null)
                    gameObject.GetComponent<PlayerKiller>().SetKillMethod(PlayerKiller.KillMethod.Boom);
            }
            EventBroker.CallKillPlayer();
        } else if (other.gameObject.CompareTag("Coin")) {
            Debug.Log("Coin take");
            EventBroker.CallCoinPicked();
            other.gameObject.SetActive(false);
        } else if (other.gameObject.CompareTag("Water")) {
            gameObject.GetComponent<PlayerKiller>().SetKillMethod(PlayerKiller.KillMethod.Sink);
            EventBroker.CallKillPlayer();
        }
    }
}
