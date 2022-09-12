using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockReleaseGuide : MonoBehaviour
{
    public List<GameObject> guideObj;
    private List<SpriteRenderer> guideRenderer = new List<SpriteRenderer>();

    bool guideOn = false;
    PolyominoBase curGrabObj;

    Vector3 guideObjOffset_Release = new Vector3(0, 0, 0);
    Vector3 guideObjOffset_Break = new Vector3(0, 0, -1f);

    public Color color_guideRelease = Color.green;
    public Color color_guideBreak = Color.red;
    
    private void Awake()
    {
        PointerManager.onGrabPolyomino += OnGrabPolyomino;
        PointerManager.onReleasePolyomino += OnReleasePolyomino;

        GuideObjActive(false, guideObj.Count);

        foreach (GameObject obj in guideObj)
        {
            SpriteRenderer newRenderer = obj.GetComponent<SpriteRenderer>();
            if (newRenderer != null)
            {
                //newRenderer.sortingOrder = 1;
                guideRenderer.Add(newRenderer);
            }
        }
    }

    private void OnDestroy()
    {
        PointerManager.onGrabPolyomino -= OnGrabPolyomino;
        PointerManager.onReleasePolyomino -= OnReleasePolyomino;
    }

    private void OnGrabPolyomino(PolyominoBase polyominoBase, PointerEventData eventData)
    {
        guideOn = true;
        curGrabObj = polyominoBase;
    }

    private void OnReleasePolyomino(PolyominoBase polyominoBase)
    {
        guideOn = false;
        curGrabObj = null;
        GuideObjActive(false, guideObj.Count);
    }


    private void Update()
    {
        if (guideOn && curGrabObj != null)
        {
            List<BlockSlot> fitSlots = GameBoard.GetOnBoardSlots(curGrabObj);

            if (fitSlots != null)
            {
                GuideObjActive(true, fitSlots.Count);

                for (int i = 0; i < fitSlots.Count; i++)
                {
                    if (fitSlots[i].curBlock == null)
                    {
                        guideRenderer[i].color = color_guideRelease;
                        guideObj[i].transform.position = fitSlots[i].transform.position + guideObjOffset_Release;
                    }
                    else
                    {
                        guideRenderer[i].color = color_guideBreak;
                        guideObj[i].transform.position = fitSlots[i].transform.position + guideObjOffset_Break;
                    }
                }
            }
            else
            {
                GuideObjActive(false, guideObj.Count);
            }
        }
    }

    void GuideObjActive(bool activate, int count)
    {
        for (int i = 0; i < guideObj.Count; i++)
        {
            if (i < count) guideObj[i].SetActive(activate);
            else guideObj[i].SetActive(false);
        }
    }
}
