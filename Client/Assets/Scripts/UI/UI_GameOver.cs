using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    public GameObject gameOverTextRoot;
    public TMPro.TMP_Text gameOverText;
    public TMPro.TMP_Text gameOverScoreText;

    private void Awake()
    {
        GameBoard.onGameOver += OnGameOver;
        GameBoard.onInitBoard += OnInitBoard;

        PageControl.onChangePage += OnChangePage;
    }

    private void OnDestroy()
    {
        GameBoard.onGameOver -= OnGameOver;
        GameBoard.onInitBoard -= OnInitBoard;

        PageControl.onChangePage -= OnChangePage;
    }

    void OnInitBoard()
    {
        gameOverTextRoot.SetActive(false);
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
    }

    void Start()
    {
        gameOverTextRoot.SetActive(false);
    }
}
