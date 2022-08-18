using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    public GameObject gameOverTextRoot;
    public TMPro.TMP_Text gameOverText;

    private void Awake()
    {
        GameBoard.onGameOver += OnGameOver;
        GameBoard.onInitBoard += OnInitBoard;
    }

    private void OnDestroy()
    {
        GameBoard.onGameOver -= OnGameOver;
        GameBoard.onInitBoard -= OnInitBoard;
    }

    void OnInitBoard()
    {
        gameOverTextRoot.SetActive(false);
    }

    void OnGameOver()
    {
        gameOverTextRoot.SetActive(true);
        gameOverText.GetComponent<TMPro.TextMeshProEffect>().Play();
    }

    void Start()
    {
        gameOverTextRoot.SetActive(false);
    }
}
