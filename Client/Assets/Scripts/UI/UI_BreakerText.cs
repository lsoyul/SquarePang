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
    public DOTweenAnimation breakerGaugeTween;

    private float breakerGaugeTweenDuration = 0.2f;

    private float breakerGaugeMaxWidth = 1500f;
    private float breakerGaugeBaseUnitWidth = 100f;

    private void Awake()
    {
        GameBoard.onInitBoard += OnInitBoard;
        GameBoard.onChangeScore += OnChangeScore;
        GameBoard.onReviveGameBoard += OnReviveGameBoard;

        BlockControl.onImpossiblePutBlockByBreakerCount += OnImpossiblePutBlockByBreakerCount;

        breakerGaugeMaxWidth = UIBreakerGauge_BG.rect.width;
        breakerGaugeBaseUnitWidth = breakerGaugeMaxWidth / GameBoard.RemainBreakerMaxCount;

    }

    private void OnDestroy()
    {
        GameBoard.onInitBoard -= OnInitBoard;
        GameBoard.onChangeScore -= OnChangeScore;
        GameBoard.onReviveGameBoard -= OnReviveGameBoard;

        BlockControl.onImpossiblePutBlockByBreakerCount -= OnImpossiblePutBlockByBreakerCount;
    }

    void OnInitBoard()
    {
        UIBreakerGauge_BG.sizeDelta = new Vector2(GameBoard.BoardScreenWidthHeight, UIBreakerGauge_BG.sizeDelta.y);
        breakerGaugeMaxWidth = UIBreakerGauge_BG.rect.width;
        breakerGaugeBaseUnitWidth = breakerGaugeMaxWidth / GameBoard.RemainBreakerMaxCount;

        breakerCountText.text = GameBoard.RemainBreakerCount.ToString();
        SetBreakerGauge(GameBoard.RemainBreakerCount, false);

        Vector3 fixedThisPos = this.transform.position;
        fixedThisPos.y = GameBoard.BoardScreenCoordTopY;
        this.transform.position = fixedThisPos;
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

    float GetBreakerGaugeWidth()
    {
        return GameBoard.RemainBreakerCount * breakerGaugeBaseUnitWidth;
    }

    float tweenGauge;

    void SetBreakerGauge(int targetBreaker, bool animation = true)
    {
        float resWidth = targetBreaker * breakerGaugeBaseUnitWidth;

        if (animation == false)
        {
            UIBreakerGauge_Front.sizeDelta = new Vector2(resWidth, UIBreakerGauge_Front.rect.height);
        }
        else
        {
            tweenGauge = UIBreakerGauge_Front.rect.width;
            DOTween.To(() => tweenGauge, x => tweenGauge = x, resWidth, breakerGaugeTweenDuration)
                .OnUpdate(onUpdateBreakerGauge)
                .OnComplete(onCompleteBreakerGaugeTween)
                .SetEase(Ease.OutExpo);
        }
    }

    void onUpdateBreakerGauge()
    {
        UIBreakerGauge_Front.sizeDelta = new Vector2(tweenGauge, UIBreakerGauge_Front.rect.height);
    }

    void onCompleteBreakerGaugeTween()
    {
        UIBreakerGauge_Front.sizeDelta = new Vector2(GetBreakerGaugeWidth(), UIBreakerGauge_Front.rect.height);
    }

    void OnImpossiblePutBlockByBreakerCount(List<BlockSlot> failedSlots)
    {
        breakerGaugeTween.DORestart();
    }
}
