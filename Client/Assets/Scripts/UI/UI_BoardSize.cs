using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BoardSize : MonoBehaviour
{
    public TMPro.TMP_InputField sizeXText;
    public TMPro.TMP_InputField sizeYText;

    public GameBoard gameBoard;

    void Start()
    {
        sizeXText.text = gameBoard.boardWidth.ToString();
        sizeYText.text = gameBoard.boardHeight.ToString();
    }

}
