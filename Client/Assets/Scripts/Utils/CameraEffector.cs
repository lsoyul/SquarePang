using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CameraEffector : MonoBehaviour
{
    public GameObject targetGameBoard;

    public float madeShakeDuration = 0.25f;
    public float madeShakeDefaultRatio = 0.02f;

    private void Awake()
    {
        BlockEffector.onEndReleasePolyomino += OnEndReleasePolyomino;
        BlockEffector.onEndMadeSquareEffect += OnEndMadeSquareEffect;
    }

    private void OnDestroy()
    {
        BlockEffector.onEndReleasePolyomino -= OnEndReleasePolyomino;
        BlockEffector.onEndMadeSquareEffect -= OnEndMadeSquareEffect;
    }

    void OnEndReleasePolyomino()
    {
        targetGameBoard.transform.DOLocalMoveZ(0.5f, 0.1f).SetEase(Ease.OutQuad).From();
    }

    void OnEndMadeSquareEffect(List<List<BlockSlot>> madeSlots)
    {
        int slotCount = 0;

        foreach (var slotList in madeSlots)
        {
            foreach (var item in slotList)
            {
                slotCount++;
            }
        }

        if (madeSlots.Count > 0)
        {
            targetGameBoard.transform.DOShakePosition(madeShakeDuration, madeShakeDefaultRatio * slotCount, 60, 90).SetEase(Ease.OutQuad).From();
           
        }
    }
}
