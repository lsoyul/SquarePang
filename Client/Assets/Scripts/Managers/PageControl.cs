using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using DG.Tweening;

using static GameStatics;

public class PageControl : MonoBehaviour
{
    public static GamePage CurPage = GamePage.Title;

    public GameBoard gameBoard;

    public GameObject TitlePage;
    public GameObject GamePagee;

    public DOTweenAnimation settingPopupTweener;
    public Tutorial tutorial;

    public List<DOTweenAnimation> tweenerInTitle;

    bool isInit = false;

    public static Action<GamePage> onChangePage;    // Target Page

    void Awake()
    {
        FB.onReadyFireBase += OnReadyFireBase;

        if (CurPage == GamePage.Title)
        {
            SPSoundManager.PlayMusic(SPSoundManager.Sound_BGM.Title);
        }
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

                SPSoundManager.PlayMusic(SPSoundManager.Sound_BGM.Title);
                break;
            case GamePage.Game:
                TitlePage.SetActive(false);
                GamePagee.SetActive(true);

                SPSoundManager.PlayMusic(SPSoundManager.Sound_BGM.Ingame);
                break;
            default:
                break;
        }

        PlayTweeners(targetPage);

        gameBoard.InitGameBoardBase(gameBoard.boardWidth, gameBoard.boardHeight);
        onChangePage?.Invoke(CurPage);
        settingPopupTweener.DOPlayBackwards();
        AppManager.IsSettingOn = false;
    }

    public void OnClick_ChangePage_GoTitle()
    {
        gameBoard.ClearGameBoardBase();

        //gameBoard.InitGameBoardBase(gameBoard.boardWidth, gameBoard.boardHeight);
        
        ChangePage(GamePage.Title);
    }

    public void OnClickTutorial()
    {
        tutorial.StartTutorial();
        if (AppManager.IsSettingOn)
        {
            settingPopupTweener.DOPlayBackwards();
            AppManager.IsSettingOn = false;
        }
    }

    public void OnClickExitTutorial()
    {
        tutorial.ExitTutorial();
    }

    public void PlayTweeners(GamePage targetPage)
    {
        if (targetPage == GamePage.Title)
        {
            if (tweenerInTitle != null)
            {
                foreach (var tween in tweenerInTitle)
                {
                    tween.DORestart();
                }
            }
        }
    }
}
