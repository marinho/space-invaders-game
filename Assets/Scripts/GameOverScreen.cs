using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Text pointsText;
    [SerializeField] GameScore gameScore;

    void Update()
    {
        pointsText.text = string.Format("{0} points", gameScore.playerScore);
    }
}
