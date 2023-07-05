using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private int _lineCost;

    private int _score = 0;

    private void Awake()
    {
        LevelBlocks.DestroedLine += AddScoreForLineDestroy;
    }

    private void OnDestroy()
    {
        LevelBlocks.DestroedLine -= AddScoreForLineDestroy;
    }

    private void Start()
    {
        _scoreText.text = "0";
    }

    private void AddScoreForLineDestroy(int desctoyLineCount)
    {
        int score = desctoyLineCount * _lineCost;

        AddScore(score);
    }

    private void AddScore(int score)
    {
        _score += score;
        _scoreText.text = _score.ToString();
    }
}
