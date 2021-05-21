using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] GameObject soundButton;
    [SerializeField] GameObject title;
    [SerializeField] Sprite[] images;
    
    void Start()
    {
        SetSound(DataStorage.GetSoundOn());
        soundButton.GetComponent<Button>().onClick.AddListener(() => {
            if(soundButton != null)
            SetSound(!DataStorage.GetSoundOn());
            EventBroker.CallButtonTap();
        });
        EventBroker.GameStateChanged += UpdateUI;
    }

    private void UpdateUI() {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Start) {
            if(soundButton != null)
            soundButton.gameObject.SetActive(false);
            if(title != null)
            title.SetActive(false);
        }
    }

    private void SetSound(bool on) {
        if(on) {
            soundButton.GetComponent<Image>().sprite = images[1];
            DataStorage.SetSoundOn(1);
            var list = Camera.main.gameObject.GetComponent<AudioListener>();
            if(list == null) {
                Camera.main.gameObject.AddComponent<AudioListener>();
            }
        } else {
            soundButton.GetComponent<Image>().sprite = images[0];
            DataStorage.SetSoundOn(0);
            var list = Camera.main.gameObject.GetComponent<AudioListener>();
            if (list != null) {
                Destroy(list);
            }
        }
    }

}
