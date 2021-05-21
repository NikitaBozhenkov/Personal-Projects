using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataStorage {
    private const string CoinsKey = "Coins_Count";
    private const string HighscoreKey = "Highscore";
    private const string SoundOnKey = "SoundOn";

    public static int GetCoinsCount() {
        return PlayerPrefs.GetInt(CoinsKey, 0);
    }

    public static void SetCoinsCount(int value) {
        Debug.Log("SetCoins called");
        PlayerPrefs.SetInt(CoinsKey, value);
    }

    public static int GetHighscore() {
        return PlayerPrefs.GetInt(HighscoreKey, 0);
    }

    public static void SetHighscore(int value) {
        PlayerPrefs.SetInt(HighscoreKey, value);
    }

    public static bool GetSoundOn() {
        return PlayerPrefs.GetInt(SoundOnKey, 1) == 1;
    }

    public static void SetSoundOn(int value) {
        PlayerPrefs.SetInt(SoundOnKey, value);
    }
}
