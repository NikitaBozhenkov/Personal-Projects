using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    [SerializeField] AudioSource audioSource;

    private void Start() {
        EventBroker.PlayerCanMove += MakeSound;
    }

    private void MakeSound() {
        if (Random.Range(0, 10) <= 3 && audioSource != null && sound != null) {
            audioSource.PlayOneShot(sound);
        }
    }

    private void OnDisable() {
        EventBroker.PlayerCanMove -= MakeSound;
    }
}
