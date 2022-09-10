using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
     public enum Visual
    {
        Hard = 0,
        Soft = 1,
        Round = 2,
        Paper = 3,
        END = 4,
    }

    public GameObject HardBlocks;
    public GameObject SoftBlocks;
    public GameObject RoundBlocks;
    public GameObject PaperBlocks;

    public Visual CurVisual;

    private void Start()
    {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }
    }

    public void SetBlockVisual(Visual targetVisual)
    {
        CurVisual = targetVisual;

        HardBlocks.SetActive(false);
        
        SoftBlocks.SetActive(false);
        
        RoundBlocks.SetActive(false);
        
        PaperBlocks.SetActive(false);
        

        switch (targetVisual)
        {
            case Visual.Hard:
                HardBlocks.SetActive(true);
                break;
            case Visual.Soft:
                SoftBlocks.SetActive(true);
                break;
            case Visual.Round:
                RoundBlocks.SetActive(true);
                break;
            case Visual.Paper:
                PaperBlocks.SetActive(true);
                break;
            default:
                break;
        }
    }
}
