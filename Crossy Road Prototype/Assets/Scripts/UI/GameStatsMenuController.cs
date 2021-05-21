using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStatsMenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI coins;

    private void Update() {
        var curScore = GameManager.Instance.curScore;
        if (curScore > DataStorage.GetHighscore()) {
            DataStorage.SetHighscore(curScore);
        }
        score.text = curScore.ToString();
        coins.text = DataStorage.GetCoinsCount().ToString();
    }
}
