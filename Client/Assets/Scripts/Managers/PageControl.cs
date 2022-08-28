using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using static GameStatics;

public class PageControl : MonoBehaviour
{
    public static GamePage CurPage = GamePage.Title;

    public GameBoard gameBoard;

    public GameObject TitlePage;
    public GameObject GamePagee;

    bool isInit = false;

    public static Action<GamePage> onChangePage;    // Target Page

    void Awake()
    {
        FB.onReadyFireBase += OnReadyFireBase;
    }

    private void OnDestroy()
    {
        FB.onReadyFireBase -= OnReadyFireBase;
    }

    void OnReadyFireBase(Firebase.FirebaseApp app)
    {
        isInit = true;
    }

    public void OnClickGameStart_Sprint()
    {
        OnClickGameStart(GameMode.Sprint);
    }

    public void OnClickGameStart_Endless()
    {
        OnClickGameStart(GameMode.Endless);
    }

    void OnClickGameStart(GameMode gameMode)
    {
        GameBoard.CurGameMode = gameMode;
        ChangePage(GamePage.Game);
    }

    public void ChangePage(GamePage targetPage)
    {
        CurPage = GamePage.Game;

        switch (targetPage)
        {
            case GamePage.Title:
                TitlePage.SetActive(true);
                GamePagee.SetActive(false);
                break;
            case GamePage.Game:
                TitlePage.SetActive(false);
                GamePagee.SetActive(true);
                break;
            default:
                break;
        }

        gameBoard.InitGameBoardBase(gameBoard.boardWidth, gameBoard.boardHeight);
        onChangePage?.Invoke(CurPage);
    }

    public void OnClick_ChangePage_GoTitle()
    {
        gameBoard.ClearGameBoardBase();

        //gameBoard.InitGameBoardBase(gameBoard.boardWidth, gameBoard.boardHeight);
        
        ChangePage(GamePage.Title);
    }
}
