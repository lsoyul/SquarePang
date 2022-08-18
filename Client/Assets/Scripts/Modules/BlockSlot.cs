using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
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

    public BlockSlot PutBlock(Block newBlock)
    {
        if (curBlock != null)
        {
            Destroy(curBlock.gameObject);
            curBlock = null;
        }
        else
        {
            curBlock = newBlock;

            curBlock.transform.parent = this.transform;
            curBlock.transform.localPosition = Vector3.zero;
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

    private void Awake()
    {
        if (isInit == false)
            this.gameObject.SetActive(false);
    }
}
