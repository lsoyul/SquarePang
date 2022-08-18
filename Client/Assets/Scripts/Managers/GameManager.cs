using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameBoard GameBoard;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GameBoard.InitGameBoardBase(GameBoard.boardWidth, GameBoard.boardHeight);
    }
}
