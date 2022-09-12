using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using static GameStatics;

public class BlockSlot : MonoBehaviour
{
    public GameObject AttachRoot;
    public DOTweenAnimation putAnimation;

    public int ix = 0;
    public int iy = 0;

    bool isInit = false;

    public Block curBlock = null;

    public void InitBlockSlot(int x, int y)
    {
        ix = x;
        iy = y;
        isInit = true;
        this.gameObject.SetActive(true);
    }

    public BlockSlot PutBlock(Block newBlock, PolyominoRot rot)
    {
        if (curBlock != null)
        {
            Destroy(curBlock.gameObject);
            curBlock = null;
        }
        else
        {
            curBlock = newBlock;

            curBlock.transform.parent = AttachRoot.transform;
            curBlock.transform.localPosition = Vector3.zero;

            float zRot = 0;
            switch (rot)
            {
                case PolyominoRot.Rot0: zRot = 0f;
                    break;
                case PolyominoRot.Rot180: zRot = 180f;
                    break;
                case PolyominoRot.Rot270: zRot = 270f;
                    break;
                case PolyominoRot.Rot90: zRot = 90f;
                    break;
                default:
                    break;
            }
            curBlock.transform.localRotation = Quaternion.Euler(0f, 0f, zRot);

        }

        return this;
    }

    public void RemoveBlock()
    {
        if (curBlock != null)
        {
            Destroy(curBlock.gameObject);
            curBlock = null;
        }
    }

    public void ShakeBoard()
    {
        putAnimation.DOPlayForward();
    }

    private void Awake()
    {
        if (isInit == false)
            this.gameObject.SetActive(false);
    }
}
