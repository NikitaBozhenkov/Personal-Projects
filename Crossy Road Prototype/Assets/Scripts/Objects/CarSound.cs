using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] AudioClip sound;
    [SerializeField] AudioSource audioSource;

    private void Update() {
        if (Random.Range(0, 10000) < 1) {
            audioSource.PlayOneShot(sound);
        }
    }
}
