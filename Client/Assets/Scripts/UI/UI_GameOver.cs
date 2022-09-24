using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    public GameObject gameOverTextRoot;
    public TMPro.TMP_Text gameOverText;
    public TMPro.TMP_Text gameOverScoreText;
    public GameObject reviveBtnObj;

    private void Awake()
    {
        GameBoard.onGameOver += OnGameOver;
        GameBoard.onInitBoard += OnInitBoard;

        PageControl.onChangePage += OnChangePage;

        AdsManager.onEarnedByRewardAd += OnEarnedByRewardAd;
    }

    private void OnDestroy()
    {
        GameBoard.onGameOver -= OnGameOver;
        GameBoard.onInitBoard -= OnInitBoard;

        PageControl.onChangePage -= OnChangePage;

        AdsManager.onEarnedByRewardAd -= OnEarnedByRewardAd;
    }

    void OnInitBoard()
    {
        gameOverTextRoot.SetActive(false);

        UpdateReviveBtn();
    }

    void OnGameOver(GameStatics.GameEndType gameEndType)
    {
        gameOverTextRoot.SetActive(true);
        gameOverText.GetComponent<TMPro.TextMeshProEffect>().Play();
        gameOverScoreText.text = GameBoard.ResultScore.ToString();
        gameOverScoreText.GetComponent<TMPro.TextMeshProEffect>().Play();
    }

    void OnChangePage(GameStatics.GamePage targetPage)
    {
        gameOverTextRoot.SetActive(false);
        UpdateReviveBtn();
    }

    void Start()
    {
        gameOverTextRoot.SetActive(false);
        UpdateReviveBtn();
    }

    void OnEarnedByRewardAd()
    {
        gameOverTextRoot.SetActive(false);
        UpdateReviveBtn();
    }

    void UpdateReviveBtn()
    {
        if ((GameBoard.ReviveRemainCount > 0) == false)
        {
            reviveBtnObj.SetActive(false);
        }
        else
        {
            reviveBtnObj.SetActive(true);
        }
    }
}
