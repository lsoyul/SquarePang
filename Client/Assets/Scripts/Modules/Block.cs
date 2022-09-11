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

    public void SetBlockTransparent(float alphaValue)
    {
        Renderer hardMatRenderer = HardBlocks.GetComponent<Renderer>();
        Color hardMatColor = hardMatRenderer.material.color;
        hardMatColor.a = alphaValue;
        hardMatRenderer.material.color = hardMatColor;

        Renderer softMatRenderer = SoftBlocks.GetComponent<Renderer>();
        Color softMatColor = softMatRenderer.material.color;
        softMatColor.a = alphaValue;
        softMatRenderer.material.color = softMatColor;

        Renderer paperMatRenderer = PaperBlocks.GetComponent<Renderer>();
        Color paperMatColor = paperMatRenderer.material.color;
        paperMatColor.a = alphaValue;
        paperMatRenderer.material.color = paperMatColor;

        Renderer roundMatRenderer = RoundBlocks.GetComponent<Renderer>();
        Color roundMatColor = roundMatRenderer.material.color;
        roundMatColor.a = alphaValue;
        roundMatRenderer.material.color = roundMatColor;
        
    }
}
