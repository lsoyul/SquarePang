using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using static GameStatics;

public class Btn_UI : MonoBehaviour
{
    public GameBoard gameBoard;

    public UI_BoardSize uiBoardSize;

    public GameObject GameModeText_sprint;
    public GameObject GameModeText_endless;
    public TMPro.TMP_Text sprintText;

    #region Rotate / Stash Button Tween

    public DOTweenAnimation rotateTweener;
    public DOTweenAnimation stashTweener;

    #endregion

    private void Awake()
    {
        PageControl.onChangePage += OnChangePage;
        GameBoard.onMadeSquare += OnMadeSquare;

        BlockControl.onClickStash += OnClickStash;
        BlockControl.onClickRotate += OnClickRotate;

    }

    
    private void OnDestroy()
    {
        PageControl.onChangePage -= OnChangePage;
        GameBoard.onMadeSquare -= OnMadeSquare;

        BlockControl.onClickStash -= OnClickStash;
        BlockControl.onClickRotate -= OnClickRotate;
    }

    public void OnClickStash()
    {
        stashTweener.DORestart();
    }

    public void OnClickRotate()
    {
        rotateTweener.DORestart();
    }

    public void OnClickBoardClear()
    {
        gameBoard.ClearGameBoardBase();

        int.TryParse(uiBoardSize.sizeXText.text, out int boardWidth);
        int.TryParse(uiBoardSize.sizeYText.text, out int boardHeight);


        if (boardWidth > 0 && boardHeight > 0)
            gameBoard.InitGameBoardBase(boardWidth, boardHeight);
        else
        {
            gameBoard.InitGameBoardBase(gameBoard.boardWidth, gameBoard.boardHeight);
        }
    }

    void OnChangePage(GamePage gamePage)
    {
        if (gamePage == GamePage.Game)
        {
            InitGameModeText(GameBoard.CurGameMode);
        }
    }


    void InitGameModeText(GameMode targetMode)
    {
        switch (targetMode)
        {
            case GameMode.Sprint:
                GameModeText_sprint.SetActive(true);
                GameModeText_endless.SetActive(false);

                sprintText.text = string.Format("{0}/{1}", GameBoard.sprintModeCurMadeSquareCount, GameBoard.SprintModeMaxTargetSquareCount);

                break;

            case GameMode.Endless:
                GameModeText_sprint.SetActive(false);
                GameModeText_endless.SetActive(true);
                break;

            default:
                break;
        }
    }

    void OnMadeSquare(List<List<BlockSlot>> slotLists, int remainBreaker, bool isCleanBoard)
    {
        sprintText.text = string.Format("{0}/{1}", GameBoard.sprintModeCurMadeSquareCount, GameBoard.SprintModeMaxTargetSquareCount);
    }
}
