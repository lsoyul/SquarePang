using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScoreText : MonoBehaviour
{
    public Canvas mainCanvas;
    public TMPro.TMP_Text scoreText;
    public TMPro.TextMeshProEffect tmpEffect;
    // Start is called before the first frame update
    void Awake()
    {
        GameBoard.onChangeScore += OnChangeScore;
        GameBoard.onInitBoard += OnInitBoard;
    }

    private void OnDestroy()
    {
        GameBoard.onChangeScore -= OnChangeScore;
        GameBoard.onInitBoard -= OnInitBoard;
    }

    void OnChangeScore(int getScore, int totalScore)
    {
        if (totalScore.ToString().Equals(scoreText.text) == false) tmpEffect.Play();

        scoreText.text = totalScore.ToString();

    }

    void OnInitBoard()
    {
        //Vector3 fixedThisPos = this.transform.position;
        //float offsetY = mainCanvas.GetComponent<RectTransform>().rect.height * 0.027f;
        //fixedThisPos.y = GameBoard.BoardScreenCoordTopY + offsetY;
        //this.transform.position = fixedThisPos;
    }
}
