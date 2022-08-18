using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_UI : MonoBehaviour
{
    public GameBoard gameBoard;

    public UI_BoardSize uiBoardSize;

    public void OnClickRotate()
    {
        BlockControl.OnClickRotate();
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
}
