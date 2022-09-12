using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_ChangeShape : MonoBehaviour
{
    public static Block.Visual CurVisual = Block.Visual.Hard;

    private void Awake()
    {
        BlockControl.onInitBlockControls += OnInitBlockControls;
    }

    private void OnDestroy()
    {
        BlockControl.onInitBlockControls -= OnInitBlockControls;
    }

    void OnInitBlockControls()
    {
        SetVisual();
    }


    Vector3 attachRootDefault = new Vector3(0, 0, -1f);
    Vector3 attachRootPaper = new Vector3(0, 0, -0.1f);

    void SetVisual()
    {
        foreach (List<BlockSlot> slotList in GameBoard.GetBlockSlotsOnBoard())
        {
            foreach (BlockSlot slot in slotList)
            {
                if (slot.curBlock != null)
                {
                    slot.curBlock.SetBlockVisual(CurVisual);
                }

                switch (CurVisual)
                {
                    case Block.Visual.Paper:
                        slot.AttachRoot.transform.localPosition = attachRootPaper;
                        break;
                    default:
                        slot.AttachRoot.transform.localPosition = attachRootDefault;
                        break;
                }
            }
        }

        foreach (GameObject go in BlockControl.NextBlocks)
        {
            PolyominoBase pb = go.GetComponentInChildren<PolyominoBase>();
            if (pb != null)
            {
                foreach (Block block in pb.blocks)
                {
                    block.SetBlockVisual(CurVisual);
                }
            }
        }

        foreach (GameObject go in BlockControl.PrepareBlocks)
        {
            PolyominoBase pb = go.GetComponentInChildren<PolyominoBase>();
            if (pb != null)
            {
                foreach (Block block in pb.blocks)
                {
                    block.SetBlockVisual(CurVisual);
                }
            }
        }

        if (BlockControl.StashBlock != null)
        {
            PolyominoBase pb = BlockControl.StashBlock.GetComponentInChildren<PolyominoBase>();

            if (pb != null)
            {
                foreach (Block block in pb.blocks)
                {
                    block.SetBlockVisual(CurVisual);
                }
            }
        }
    }


    public void OnClickChangeShape()
    {
        int cur = ((int)CurVisual);
        cur = ++cur % (int)Block.Visual.END;

        CurVisual = (Block.Visual)cur;

        SetVisual();
    }
}
