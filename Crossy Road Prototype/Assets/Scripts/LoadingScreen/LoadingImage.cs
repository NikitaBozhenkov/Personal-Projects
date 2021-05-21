using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImage : MonoBehaviour
{
    [SerializeField] InputHandler inputHandlerPref;

    public void SpawnInputHandler() {
        Instantiate(inputHandlerPref);
    }
}
