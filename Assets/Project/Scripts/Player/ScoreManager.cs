using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText;
    public int currentScore;

    private float _initialFontSize;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _initialFontSize = scoreText.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.fontSize = Mathf.Lerp(scoreText.fontSize, _initialFontSize, Time.deltaTime * 15);
    }

    public void UpdateScore(int addition)
    {
        currentScore += addition;
        scoreText.text = "SCORE: " + currentScore;
        scoreText.fontSize = _initialFontSize * 1.1f;
    }
}
