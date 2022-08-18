using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BreakerText : MonoBehaviour
{
    public TMPro.TMP_Text breakerCountText;
    public TMPro.TextMeshProEffect breakerTitleText;
    public TMPro.TextMeshProEffect breakerTextEffect;

    private void Awake()
    {
        GameBoard.onInitBoard += OnInitBoard;
        GameBoard.onChangeScore += OnChangeScore;
    }

    private void OnDestroy()
    {
        GameBoard.onInitBoard -= OnInitBoard;
        GameBoard.onChangeScore -= OnChangeScore;
    }

    void OnInitBoard()
    {
        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();
    }


    void OnChangeScore(int getScore, int totalScore)
    {
        int.TryParse(breakerCountText.text, out int oldBreakerCount);

        if (oldBreakerCount < GameBoard.RemainBreakerCount)
        {
            // Add Breaker
            breakerTextEffect.Play();
        }
        else if (oldBreakerCount > GameBoard.RemainBreakerCount)
        {
            // Use Breaker
            breakerTextEffect.Play();

            breakerTitleText.Play();
        }

        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();
    }
}
