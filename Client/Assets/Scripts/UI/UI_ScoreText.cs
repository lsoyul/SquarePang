using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreText : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    public TMPro.TextMeshProEffect tmpEffect;
    // Start is called before the first frame update
    void Awake()
    {
        GameBoard.onChangeScore += OnChangeScore;
    }

    private void OnDestroy()
    {
        GameBoard.onChangeScore -= OnChangeScore;
    }

    void OnChangeScore(int getScore, int totalScore)
    {
        if (totalScore.ToString().Equals(scoreText.text) == false) tmpEffect.Play();

        scoreText.text = totalScore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
