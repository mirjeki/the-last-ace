using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    int score;

    private void Start()
    {
        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateScoreUI();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"{score}";
    }

    public int GetScore() 
    { 
        return score;
    }
}
