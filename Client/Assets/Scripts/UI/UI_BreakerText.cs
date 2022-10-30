using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class UI_BreakerText : MonoBehaviour
{
    public TMPro.TMP_Text breakerCountText;
    public TMPro.TextMeshProEffect breakerTitleText;
    public TMPro.TextMeshProEffect breakerTextEffect;

    public RectTransform UIBreakerGauge_BG;
    public RectTransform UIBreakerGauge_Front;

    public DOTweenAnimation breakerIconTween;

    private float breakerGaugeTweenDuration = 0.2f;

    private float breakerGaugeMaxHeight = 1500f;
    private float breakerGaugeBaseUnitHeight = 100f;

    private void Awake()
    {
        GameBoard.onInitBoard += OnInitBoard;
        GameBoard.onChangeScore += OnChangeScore;
        GameBoard.onReviveGameBoard += OnReviveGameBoard;

        breakerGaugeMaxHeight = UIBreakerGauge_BG.rect.height;
        breakerGaugeBaseUnitHeight = breakerGaugeMaxHeight / GameBoard.RemainBreakerMaxCount;

    }

    private void OnDestroy()
    {
        GameBoard.onInitBoard -= OnInitBoard;
        GameBoard.onChangeScore -= OnChangeScore;
        GameBoard.onReviveGameBoard -= OnReviveGameBoard;
    }

    void OnInitBoard()
    {
        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();
        SetBreakerGauge(GameBoard.RemainBreakerCount, false);
    }


    void OnChangeScore(int getScore, int totalScore)
    {
        int.TryParse(breakerCountText.text, out int oldBreakerCount);

        if (oldBreakerCount < GameBoard.RemainBreakerCount)
        {
            // Add Breaker
            breakerTextEffect.Play();
            breakerIconTween.DORestart();
        }
        else if (oldBreakerCount > GameBoard.RemainBreakerCount)
        {
            // Use Breaker
            breakerTextEffect.Play();

            breakerTitleText.Play();
            breakerIconTween.DORestart();
        }

        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();

        SetBreakerGauge(GameBoard.RemainBreakerCount, true);
    }

    void OnReviveGameBoard()
    {
        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();
    }

    float GetBreakerGaugeHeight()
    {
        return GameBoard.RemainBreakerCount * breakerGaugeBaseUnitHeight;
    }

    float tweenHeight;

    void SetBreakerGauge(int targetBreaker, bool animation = true)
    {
        float resHeight = targetBreaker * breakerGaugeBaseUnitHeight;

        if (animation == false)
        {
            UIBreakerGauge_Front.sizeDelta = new Vector2(UIBreakerGauge_Front.rect.width, resHeight);
        }
        else
        {
            tweenHeight = UIBreakerGauge_Front.rect.height;
            DOTween.To(() => tweenHeight, x => tweenHeight = x, resHeight, breakerGaugeTweenDuration)
                .OnUpdate(onUpdateBreakerGauge)
                .OnComplete(onCompleteBreakerGaugeTween)
                .SetEase(Ease.OutExpo);
        }
    }

    void onUpdateBreakerGauge()
    {
        UIBreakerGauge_Front.sizeDelta = new Vector2(UIBreakerGauge_Front.rect.width, tweenHeight);
    }

    void onCompleteBreakerGaugeTween()
    {
        UIBreakerGauge_Front.sizeDelta = new Vector2(UIBreakerGauge_Front.rect.width, GetBreakerGaugeHeight());
    }
}
