using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard GameBoard;


    private void Start()
    {
        GameBoard.InitGameBoardBase(GameBoard.boardWidth, GameBoard.boardHeight);
    }
}
